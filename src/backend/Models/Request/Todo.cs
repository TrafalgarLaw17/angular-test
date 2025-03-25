using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.ComponentModel;

namespace FRA_Todolist_prj.Models.Request
{
    public class Todo
    {
        [Required(ErrorMessage = "Todo ID is required.")]
        [JsonIgnore]  
        public int TodoId { get; set; }

        [StringLength(255, ErrorMessage = "Todo title cannot exceed 255 characters.")]
        public string? TodoTitle { get; set; }

        [StringLength(1000, ErrorMessage = "Todo description cannot exceed 1000 characters.")]
        public string? TodoDescription { get; set; }

        [Required(ErrorMessage = "TodoTaskStatus ID is required.")]
        public int TodoTaskStatusId { get; set; }

        [JsonIgnore]  
        public string? TodoTaskStatusName { get; set; }

        [JsonPropertyName("todoTaskStatusName")]
        public string? RetrieveTodoTaskStatusName => TodoTaskStatusName;

        [JsonIgnore] 
        public bool TodoIsArchive { get; set; } = false;

        private DateTime? _todoCreatedAt;
        private DateTime? _todoUpdatedAt;

        [JsonIgnore]  
        public DateTime? TodoCreatedAt
        {
            get => _todoCreatedAt;
            set => _todoCreatedAt = value?.ToUniversalTime(); 
        }

        [JsonIgnore]  
        public DateTime? TodoUpdatedAt
        {
            get => _todoUpdatedAt;
            set => _todoUpdatedAt = value?.ToUniversalTime(); 
        }

        [JsonPropertyName("todoCreatedAt")]
        public string? RetrieveTodoCreatedAt => _todoCreatedAt?.ToString("yyyy-MM-ddTHH:mm:ssZ"); 

        [JsonPropertyName("todoUpdatedAt")]
        public string? RetrieveTodoUpdatedAt => _todoUpdatedAt?.ToString("yyyy-MM-ddTHH:mm:ssZ");
    }
}