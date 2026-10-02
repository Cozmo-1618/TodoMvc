using TodoMvc.Models;

namespace TodoMvc.Repositories
{
    // This interface describes WHAT operations exist, with zero mention of EF Core, SQL, or AppDbContext
    public interface ITodoRepository 
    { 
        Task<List<TodoItem>>  GetAllAsync(); 
        Task<TodoItem?> GetByIdAsync(int id); 
        Task AddAsync(TodoItem todoItem); 
        Task UpdateAsync(TodoItem todoItem); 
        Task DeleteAsync(int id);}
}
