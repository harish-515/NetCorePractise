using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Todo.Services;
using ToDo.Data;

namespace Todo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {

        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            this._todoService = todoService;
        }

        [HttpGet(nameof(GetTodoItem))]
        public IActionResult GetTodoItem(int id)
        {
            var result = _todoService.GetTodoItem(id);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest("No record found");
        }

        [HttpGet(nameof(GetAllTodoItems))]
        public IActionResult GetAllTodoItems()
        {
            var result = _todoService.GetAllTodoItems();
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest("No records found");
        }

        [HttpPost(nameof(InsertTodoItem))]
        public IActionResult InsertTodoItem(TodoItem TodoItem)
        {
            _todoService.InsertTodoItem(TodoItem);
            return Ok("Data inserted");

        }
        
        [HttpPut(nameof(UpdateTodoItem))]
        public IActionResult UpdateTodoItem(TodoItem TodoItem)
        {
            _todoService.UpdateTodoItem(TodoItem);
            return Ok("Updation done");

        }

        [HttpDelete(nameof(DeleteTodoItem))]
        public IActionResult DeleteTodoItem(int Id)
        {
            _todoService.DeleteTodoItem(Id);
            return Ok("Data Deleted");
        }
    }
}
