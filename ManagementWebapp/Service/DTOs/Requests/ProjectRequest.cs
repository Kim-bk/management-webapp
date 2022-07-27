using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace Service.DTOS.Requests
{
    public class ProjectRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
