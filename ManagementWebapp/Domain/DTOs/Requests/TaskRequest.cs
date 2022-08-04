using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTOs.Requests
{
    public class TaskRequest
    {
        public int Id { get; set; }
        public int BeforeTaskId { get; set; }
        public int AfterTaskId { get; set; }
        public int BeforeListId { get; set; }
        public int AfterListId { get; set; }
    }
}
