using System;
using System.Linq;
using API.DTOs.Requests;
using Domain.AggregateModels.ProjectAggregate;
using Domain.AggregateModels.UserAggregate;
using Domain.Base;
using Domain.SeedWork;

namespace Domain.AggregateModels.TaskAggregate
{
    public partial class Task : EntityBase, IAggregateRoot
    {
        public Task(string name, ListTask listTask, int position) : this()
        {
            Update(name, listTask, position);
        }
        public void Update(string name, ListTask listTask, int position)
        {
            Title = name;
            ListTask = listTask;
            Position = position;
        }
        public void AddLabel(string nameLabel, string color)
        {
            Labels.Add(new Label 
            { 
                Title = nameLabel,
                Color = color
            });
        }
        public void RemoveLabel(int labelId)
        {
            Labels.Remove(Labels.FirstOrDefault(l => l.Id == labelId));
        }
        public void RemoveTodo(int todoId)
        {
            Todos.Remove(Todos.FirstOrDefault(td => td.Id == todoId));
        }
        public void RemoveMember(string userId)
        {
            Users.Remove(Users.FirstOrDefault(u => u.Id == userId));
        }

        public void DeleteTask()
        {
            // 1. Clear all components in the task
            Todos.Clear();
            Labels.Clear();
            Users.Clear();
        }
        public void AddTodo(string nameTodo, int? parentId)
        {
            Todos.Add(new Todo 
            { 
                Title = nameTodo,
                IsDone = false,
                ParentId = parentId
            });
        }

        public Todo GetToDoItem(int todoId)
        {
            return Todos.FirstOrDefault(t => t.Id == todoId);
        }
        public void UpdateTodoItem(Todo todoItem, ToDoRequest todoUpdate)
        {
            todoItem.Title = todoUpdate.Title;
            if (Convert.ToBoolean(todoItem.IsDone))
            {
                todoItem.IsDone = false;
            }

            todoItem.IsDone = true;
        }
        public void AssignMember(ApplicationUser user)
        {
            Users.Add(user);
            DoingId = user.Id;
        }
        public void Swap(Task taskToSwap)
        {
            // 1. Swap and update position
            var tempPos = Position;
            var tempList = ListTaskId;

            Position = taskToSwap.Position;
            ListTaskId = taskToSwap.ListTaskId;

            taskToSwap.Position = tempPos;
            taskToSwap.ListTaskId = tempList;
        }
    }
}
