using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Repositories;
using ToDo.Data;

namespace Todo.Services.Implementations
{
    public class TodoService : ITodoService
    {
        #region Property  
        private IRepository<TodoItem> _repository;
        #endregion

        #region Constructor  
        public TodoService(IRepository<TodoItem> repository)
        {
            _repository = repository;
        }

        public void DeleteTodoItem(int id)
        {
            _repository.Delete(GetTodoItem(id));
            _repository.SaveChanges();
        }

        public IEnumerable<TodoItem> GetAllTodoItems()
        {
            return _repository.GetAll();
        }

        public TodoItem GetTodoItem(int id)
        {
            return _repository.Get(id);
        }

        public void InsertTodoItem(TodoItem todoitem)
        {
            _repository.Insert(todoitem);
            _repository.SaveChanges();
        }

        public void UpdateTodoItem(TodoItem todoitem)
        {
            _repository.Update(todoitem);
            _repository.SaveChanges();
        }
        #endregion
    }
}
