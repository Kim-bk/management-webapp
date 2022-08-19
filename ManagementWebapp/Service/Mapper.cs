using System.Collections.Generic;
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using System.Linq;
using AutoMapper;

namespace Service
{
    public class Mapper : IMapperCustom
    {
        private readonly IMapper _autoMapper;
        public Mapper(IMapper autoMapper)
        {
            _autoMapper = autoMapper;
        }
        public List<LabelDTO> MapLabels(List<Label> labels)
        {
            return _autoMapper.Map<List<Label>, List<LabelDTO>>(labels);
        }
        public List<MemberDTO> MapMembers(List<ApplicationUser> members)
        {
            return _autoMapper.Map<List<ApplicationUser>, List<MemberDTO>>(members);
        }
        public List<TodoDTO> MapTodos(List<Todo> todos)
        {
            return _autoMapper.Map<List<Todo>, List<TodoDTO>>(todos);
        }

        public List<ListTaskDTO> MapListTasks(List<ListTask> listTasks)
        {
            // 1. Init list labels to store label of the task
            var storeListTasks = new List<ListTaskDTO>();
            foreach (var listTask in listTasks)
            {
                var lt = new ListTaskDTO
                {
                    LiskTaskId = listTask.ListTaskId,
                    Title = listTask.Title,
                    Tasks = MapTasks(listTask.Tasks.ToList()).OrderBy(t => t.Position).ToList()
                };
                storeListTasks.Add(lt);
            }
            // 2. Return result
            return storeListTasks;
        }
  
        public List<TaskDTO> MapTasks(List<Task> tasks)
        {
            // 1. Init list tasks to store label of the task
            var storeTasks = new List<TaskDTO>();
            foreach (var task in tasks)
            {
                var t = new TaskDTO
                {
                    TaskId = task.Id,
                    Position = task.Position,
                    Title = task.Title,
                    Members = MapMembers(task.Users.ToList()),
                    Todos = MapTodos(task.Todos.ToList()),
                    Labels = MapLabels(task.Labels.ToList())
                };
                storeTasks.Add(t);
            }
            // 2. Return result
            return storeTasks;
        }
    }
}
