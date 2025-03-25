using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FRA_Todolist_prj.Models.Request
{
    public class TodoTaskStatus
    {
        [Required(ErrorMessage = "TodoTaskStatus ID is required.")]
        public int TodoTaskStatusId { get; set; }
        public string? TodoTaskStatusCode { get; set; }
        public string? TodoTaskStatusName { get; set; }

        [Required(ErrorMessage = "TodoTaskStatus created date is required.")]
        [JsonIgnore]
        public DateTime? TodoTaskStatusCreatedAt { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "TodoTaskStatus updated date is required.")]
        [JsonIgnore]
        public DateTime? TodoTaskStatusUpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
