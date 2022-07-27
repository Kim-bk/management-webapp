using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace Domain.Shared
{
    public class ProjectRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
