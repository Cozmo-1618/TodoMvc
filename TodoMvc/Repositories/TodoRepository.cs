using Microsoft.EntityFrameworkCore;
using TodoMvc.Data;
using TodoMvc.Models;

namespace TodoMvc.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly AppDbContext _context; 
        public TodoRepository(AppDbContext context) => _context = context; 
        public async Task<List<TodoItem>> GetAllAsync() => await _context.TodoItems.OrderByDescending(t => t.CreatedAt).ToListAsync();
        public async Task<TodoItem?> GetByIdAsync(int id) => await _context.TodoItems.FindAsync(id);

        public async Task AddAsync(TodoItem item) 
        { 
            _context.TodoItems.Add(item); 
            await _context.SaveChangesAsync(); 
        }

        public async Task UpdateAsync(TodoItem item) 
        { 
            var existingItem = await _context.TodoItems.FindAsync(item.Id);
            if (existingItem == null) return; 
            existingItem.Title = item.Title; 
            existingItem.DueDate = item.DueDate; 
            existingItem.IsComplete = item.IsComplete; 
            await _context.SaveChangesAsync(); 
        }

        public async Task DeleteAsync(int id) 
        { 
            var item = await _context.TodoItems.FindAsync(id); 
            if (item == null) return; 
            _context.TodoItems.Remove(item); 
            await _context.SaveChangesAsync(); 
        }
    }
}
