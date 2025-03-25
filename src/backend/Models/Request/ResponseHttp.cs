using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FRA_Todolist_prj.Models.Request
{
    public class ResponseHttp
    {
        [Required(ErrorMessage = "ResponseHttp ID is required.")]
        public long ResponseHttpId { get; set; }

        [Required(ErrorMessage = "ResponseHttp status code is required.")]
        public int ResponseHttpStatusCode { get; set; }

        [Required(ErrorMessage = "ResponseHttp title is required.")]
        [StringLength(255, ErrorMessage = "ResponseHttp title cannot exceed 255 characters.")]
        public string? ResponseHttpTitle { get; set; }

        [Required(ErrorMessage = "ResponseHttp status is required.")]
        [StringLength(255, ErrorMessage = "ResponseHttp status cannot exceed 255 characters.")]
        public string? ResponseHttpStatus { get; set; }

        [Required(ErrorMessage = "ResponseHttp message is required.")]
        [StringLength(255, ErrorMessage = "ResponseHttp message cannot exceed 255 characters.")]
        public string? ResponseHttpMessage { get; set; }

        [Required(ErrorMessage = "ResponseHttp created date is required.")]
        [JsonIgnore]
        public DateTime? ResponseHttpCreatedAt { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "ResponseHttp updated date is required.")]
        [JsonIgnore]
        public DateTime? ResponseHttpUpdatedAt { get; set; } = DateTime.UtcNow;
    }
}