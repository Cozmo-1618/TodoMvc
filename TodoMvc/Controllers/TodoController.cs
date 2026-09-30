using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoMvc.Data;
using TodoMvc.Models;

namespace TodoMvc.Controllers
{
    public class TodoController : Controller
    {
        private readonly AppDbContext _context;
        //public TodoController(AppDbContext context) => _context = context;
        public TodoController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index() 
        { 
            var items = await _context.TodoItems.OrderByDescending(t => t.CreatedAt).ToListAsync(); 
            return View(items);
        }

        [HttpGet] 
        public IActionResult Create() 
        { 
            return View(); 
        }

        [HttpPost][ValidateAntiForgeryToken] 
        public async Task<IActionResult> Create(TodoItem item) 
        { 
            if (ModelState.IsValid)//checks any validation rules on your model
            { 
                _context.TodoItems.Add(item); 
                await _context.SaveChangesAsync(); 
                return RedirectToAction(nameof(Index)); //after a successful POST, you redirect rather than directly returning a view. This is the Post-Redirect-Get pattern: it stops the browser from resubmitting the form if the user hits refresh.
            } 
            return View(item); 
        }

        /*
         async/await let that thread go do other work (serve a different user's request) while waiting for the database to respond, 
        then come back and finish this one when the data arrives. 
        Task<T> is .NET's representation of "a value of type T that will exist eventually, not immediately."
         */

        /*public IActionResult Index()
        {
        public IActionResult Index() calling .ToList() instead of .ToListAsync() — it would still work correctly for one user testing locally. 
        The problem shows up under real traffic: every concurrent visitor ties up a full thread waiting on the database, 
        and your server can handle far fewer simultaneous users before running out of threads.
            var items = _context.TodoItems.OrderByDescending(t => t.CreatedAt).ToList();
            return View(items);
        }*/

    }
}
