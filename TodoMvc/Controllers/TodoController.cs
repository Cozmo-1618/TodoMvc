using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoMvc.Data;
using TodoMvc.Models;
using TodoMvc.Repositories;

namespace TodoMvc.Controllers
{
    public class TodoController : Controller
    {
        private readonly ITodoRepository _repository;
        //public TodoController(AppDbContext context) => _context = context;
        public TodoController(ITodoRepository repository)
        {
            _repository = repository;
        }
        public async Task<IActionResult> Index() 
        { 
            var items = await _repository.GetAllAsync();
            return View(items);
        }

        [HttpGet] 
        public IActionResult Create() 
        { 
            return View(); 
        }

        [HttpPost][ValidateAntiForgeryToken] 
        public async Task<IActionResult> Create(TodoItem item) //async on the method says "I'm allowed to pause inside this method."
        { 
            if (ModelState.IsValid)//checks any validation rules on your model
            { 
                await _repository.AddAsync(item);
                return RedirectToAction(nameof(Index)); //after a successful POST, you redirect rather than directly returning a view. This is the Post-Redirect-Get pattern: it stops the browser from resubmitting the form if the user hits refresh.
            } 
            return View(item); 
        }

        /*
         async/await let that thread go do other work (serve a different user's request) while waiting for the database to respond, 
        then come back and finish this one when the data arrives. 
        Task<T> is .NET's representation of "a value of type T that will exist eventually, not immediately."

        Task<IActionResult> is what the method hands back to ASP.NET while it's paused. When the method reaches return, 
        the task completes and the IActionResult comes out of it.

        The GET method isn't async because it does nothing slow. It just returns a view. 
        The POST method talks to a database, which is slow compared to CPU work, so it benefits from async. 
        Without it, the thread would sit idle waiting for the database. 
        With it, that thread can handle other requests, so your app handles more users with the same resources.
         */

        /*public IActionResult Index()
        {
        public IActionResult Index() calling .ToList() instead of .ToListAsync() — it would still work correctly for one user testing locally. 
        The problem shows up under real traffic: every concurrent visitor ties up a full thread waiting on the database, 
        and your server can handle far fewer simultaneous users before running out of threads.
            var items = _context.TodoItems.OrderByDescending(t => t.CreatedAt).ToList();
            return View(items);
        }*/

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            await _repository.DeleteAsync(id);    
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost][ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TodoItem item)
        {
            if (id != item.Id) return BadRequest();

            var existingItem = await _repository.GetByIdAsync(id);

            if (existingItem == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // _context.Update(item);
                existingItem.Title = item.Title;
                existingItem.DueDate = item.DueDate;
                existingItem.IsComplete = item.IsComplete;
                await _repository.UpdateAsync(existingItem);
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

    }
}
