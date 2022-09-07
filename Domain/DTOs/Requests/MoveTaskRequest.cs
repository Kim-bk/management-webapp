using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Requests
{
    public class MoveTaskRequest
    {
        public int TaskId { get; set; }
        public int CurrentListId { get; set; }
        public int AfterListId { get; set; }
    }
}
