using System;
using System.Linq;
using Domain.AggregateModels.ProjectAggregate;
using Domain.AggregateModels.UserAggregate;
using Domain.Base;

namespace Domain.AggregateModels.TaskAggregate
{
    public partial class Task : IAggregateRoot
    {
        public Task(string name, ListTask listTask, int position) : this()
        {
            Update(name, listTask, position);
        }
        public void Update(string name, ListTask listTask, int position)
        {
            if (String.IsNullOrWhiteSpace(name) || position == 0 || listTask == null)
            {
                throw new ArgumentNullException("Input can not be null or white space");
            }

            Title = name;
            ListTask = listTask;
            Position = position;
        }
        public void AddLabel(string nameLabel, string color)
        {
            if (String.IsNullOrWhiteSpace(nameLabel) || String.IsNullOrWhiteSpace(color))
            {
                throw new ArgumentNullException("Input can not be null or white space");
            }

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
            Todos.Clear();
            Labels.Clear();
            Users.Clear();

            // 1. Remove all todos
           /* foreach(var todo in Todos)
            {
                Todos.Remove(todo);
            }

            // 2. Remove all labels
            foreach(var label in Labels)
            {
                Labels.Remove(label);
            }

            // 3. Remove all members
            foreach(var user in Users)
            {
                Users.Remove(user);
            }
           */
        }
        public void AddTodo(string nameTodo, int? parentId)
        {
            if (String.IsNullOrWhiteSpace(nameTodo))
            {
                throw new ArgumentNullException("Input can not be null or white space");
            }

            Todos.Add(new Todo 
            { 
                Title = nameTodo,
                IsDone = false,
                ParentId = parentId
            });
        }

        public Todo GetToDoItemInTask(int todoId)
        {
            return Todos.FirstOrDefault(t => t.Id == todoId);
        }
        public void UpdateStatusTodoItem(Todo todoItem)
        {
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
            // 1. Swap and update
            var tempPos = Position;
            var tempList = ListTaskId;

            Position = taskToSwap.Position;
            ListTaskId = taskToSwap.ListTaskId;

            taskToSwap.Position = tempPos;
            taskToSwap.ListTaskId = tempList;
        }
    }
}
