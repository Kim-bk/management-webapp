﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace API.DTOs.Requests
{
    public class ProjectRequest
    {
        public string Name { get; set; }
        public string UserId { get; set; }
        public int ProjectId { get; set; }
    }
}