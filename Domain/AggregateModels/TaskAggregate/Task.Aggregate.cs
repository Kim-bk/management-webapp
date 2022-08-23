using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Domain.Entities
{
    public partial class Task
    {
        public Task(string name, ListTask listTask, int position) : this()
        {
            this.Update(name, listTask, position);
        }
        public void Update([NotNull] string name, ListTask listTask, int position)
        {
            if (String.IsNullOrWhiteSpace(name) || position == 0 || listTask == null)
            {
                throw new ArgumentNullException("Input can not be null or white space");
            }
            Title = name;
            ListTask = listTask;
            Position = position;
        }
        public void AddLabel([NotNull] string nameLabel, string color)
        {
            if (String.IsNullOrWhiteSpace(nameLabel) || String.IsNullOrWhiteSpace(color))
            {
                throw new ArgumentNullException("Input can not be null or white space");
            }
            this.Labels.Add(new Label { 
                Title = nameLabel,
                Color = color
            });
        }
        public void RemoveLabel(int labelId)
        {
            this.Labels.Remove(this.Labels.FirstOrDefault(l => l.Id == labelId));
        }
        public void AddTodo(string nameTodo, int? parentId)
        {
            if (String.IsNullOrWhiteSpace(nameTodo))
            {
                throw new ArgumentNullException("Input can not be null or white space");
            }

            this.Todos.Add(new Todo { 
                Title = nameTodo,
                IsDone = false,
                ParentId = parentId
            });
        }

        public Todo GetToDoItemInTask(int todoId)
        {
            return this.Todos.FirstOrDefault(t => t.Id == todoId);
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
            this.Users.Add(user);
            DoingId = user.Id;
        }
        public void Swap(Task taskToSwap)
        {
            // 1. Swap and update
            var tempPos = this.Position;
            var tempList = this.ListTaskId;

            this.Position = taskToSwap.Position;
            this.ListTaskId = taskToSwap.ListTaskId;

            taskToSwap.Position = tempPos;
            taskToSwap.ListTaskId = tempList;
        }
    }
}
