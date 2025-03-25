using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FRA_Todolist_prj.Models.Request
{
    public class Account
    {
        [Required(ErrorMessage = "Account ID is required.")]
        public int AccountId { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string? AccountUsername { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? AccountEmail { get; set; }

        [Required(ErrorMessage = "Account created date is required.")]
        [JsonIgnore]
        public DateTime? AccountCreatedAt { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "Account updated date is required.")]
        [JsonIgnore]
        public DateTime? AccountUpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
