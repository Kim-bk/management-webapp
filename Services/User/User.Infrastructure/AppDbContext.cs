using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Domain.AggregateModels.UserAggregate;
using Domain.Entities.Histories;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using MediatR;
using System;

namespace Infrastructure.Context
{
    public partial class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor )
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public virtual DbSet<History> Histories { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
      /*  public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await _mediator.DispatchDomainEventsAsync(this);
            var result = await base.SaveChangesAsync(cancellationToken);
            return true;
        }*/
        public void OnBeforeSaveChanges()
        {
            string userId = "";
            try
            {
                userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }
            catch (NullReferenceException)
            {
                // because we receive message from rabbitMQ so it dont have the id of the user                         
                // who interact with the database
            }
            ChangeTracker.DetectChanges();
            var auditEntries = new List<HistoryEntry>();
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is History || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;
                var auditEntry = new HistoryEntry();
                auditEntry.TableName = entry.Entity.GetType().Name;
                auditEntry.UserId = userId;
                auditEntries.Add(auditEntry);
                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = HistoryType.Create;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            auditEntry.AuditType = HistoryType.Delete;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.ChangedColumns.Add(propertyName);
                                auditEntry.AuditType = HistoryType.Update;
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
            }

            foreach (var auditEntry in auditEntries)
            {
                Histories.Add(auditEntry.ToHistory());
            }
        }
    }
}
