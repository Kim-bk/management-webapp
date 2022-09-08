using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace API.DTOs.Requests
{
    public class ProjectRequest
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int ProjectId { get; set; }
    }
}
