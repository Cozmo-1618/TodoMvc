using System.ComponentModel.DataAnnotations;

namespace TodoMvc.Models
{
    public class TodoItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Due date is required.")]
        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }

        public bool IsComplete { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
