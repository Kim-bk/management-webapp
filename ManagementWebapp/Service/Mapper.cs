using System;
using System.Collections.Generic;
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using System.Linq;

namespace Service
{
    public class Mapper : IMapper
    {
        public List<LabelDTO> MapLabels(Domain.Entities.Task task)
        {
            // 1. Init list labels to store label of the task
            var storeLabels = new List<LabelDTO>();
            foreach (var label in task.Labels)
            {
                var l = new LabelDTO
                {
                    LabelId = label.Id,
                    Title = label.Title,
                    Color = label.Color,
                };
                storeLabels.Add(l);
            }
            // 2. Return result
            return storeLabels;
        }

        public List<ListTaskDTO> MapListTasks(Project project)
        {
            // 1. Init list labels to store label of the task
            var storeListTasks = new List<ListTaskDTO>();
            foreach (var listTask in project.ListTasks)
            {
                var lt = new ListTaskDTO
                {
                    LiskTaskId = listTask.ListTaskId,
                    Title = listTask.Title,
                    Tasks = MapTasks(listTask).OrderBy(t => t.Position).ToList()
                };
                storeListTasks.Add(lt);
            }
            // 2. Return result
            return storeListTasks;
        }

        public List<MemberDTO> MapMembers(Domain.Entities.Task task)
        {
            // 1. Init list members to store label of the task
            var storeMembers = new List<MemberDTO>();
            foreach (var member in task.Users)
            {
                var m = new MemberDTO
                {
                    UserId = member.Id,
                    UserName = member.UserName,
                };
                storeMembers.Add(m);
            }
            // 2. Return result
            return storeMembers;
        }

        public List<TaskDTO> MapTasks(ListTask listTask)
        {
            // 1. Init list tasks to store label of the task
            var storeTasks = new List<TaskDTO>();
            foreach (var task in listTask.Tasks)
            {
                var t = new TaskDTO
                {
                    TaskId = task.Id,
                    Position = task.Position,
                    Title = task.Title,
                    Members = MapMembers(task),
                    Todos = MapTodos(task),
                    Labels = MapLabels(task)
                };
                storeTasks.Add(t);
            }
            // 2. Return result
            return storeTasks;
        }

        public List<TodoDTO> MapTodos(Task task)
        {

            // 1. Init list todos to store label of the task
            var storeTodos = new List<TodoDTO>();
            foreach (var todoItem in task.Todos)
            {
                var t = new TodoDTO
                {
                    TodoId = todoItem.Id,
                    Title = todoItem.Title,
                    IsDone = todoItem.IsDone,
                    ParentId = todoItem.ParentId,
                };
                storeTodos.Add(t);
            }
            // 2. Return result
            return storeTodos;
        }
    }
}
