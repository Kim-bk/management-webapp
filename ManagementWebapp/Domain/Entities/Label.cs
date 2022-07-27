using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Entities
{
    public partial class Label
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Color { get; set; }
    }
}
