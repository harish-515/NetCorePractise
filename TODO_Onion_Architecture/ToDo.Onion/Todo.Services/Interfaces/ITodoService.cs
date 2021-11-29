using System.Collections.Generic;
using ToDo.Data;

namespace Todo.Services
{
    public interface ITodoService
    {
        IEnumerable<TodoItem> GetAllTodoItems();
        TodoItem GetTodoItem(int id);
        void InsertTodoItem(TodoItem todoitem);
        void UpdateTodoItem(TodoItem todoitem);
        void DeleteTodoItem(int id);
    }
}
