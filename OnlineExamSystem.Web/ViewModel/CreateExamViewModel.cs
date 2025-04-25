using System.ComponentModel.DataAnnotations;

namespace OnlineExamSystem.Web.ViewModel
{
    public class CreateExamViewModel
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
        public string Title { get; set; } = string.Empty;
    }
}
