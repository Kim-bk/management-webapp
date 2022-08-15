using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.DTOs.Requests;
using Domain.DTOs.Responses;
using Service.Interfaces;

namespace Service
{
    public class TaskTestService : ITaskTestService
    {
        public Task<UserManagerResponse> AddLabelToTask(LabelRequest model)
        {
            throw new NotImplementedException();
        }

        public Task<TaskManagerResponse> AddToDoToTask(ToDoRequest model)
        {
            throw new NotImplementedException();
        }

        public Task<TaskManagerResponse> AssignMember(int taskId, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<TaskManagerResponse> GetTask(int taskId)
        {
            throw new NotImplementedException();
        }

        public Task<TaskManagerResponse> ManageToDoItems(ToDoRequest model)
        {
            throw new NotImplementedException();
        }

        public Task<UserManagerResponse> RemoveLabelInTask(LabelRequest model)
        {
            throw new NotImplementedException();
        }

        public void TestTask()
        {
            throw new NotImplementedException();
        }
    }
}
