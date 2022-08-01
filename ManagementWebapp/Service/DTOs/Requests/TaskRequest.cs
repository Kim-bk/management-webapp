using System;
using System.Collections.Generic;
using System.Text;

namespace Service.DTOs.Requests
{
    public class TaskRequest
    {
        public int BeforeTaskId { get; set; }
        public int AfterTaskId { get; set; }
        public int BeforeListId { get; set; }
        public int AfterListId { get; set; }
    }
}
