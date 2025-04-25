using System.ComponentModel.DataAnnotations;

namespace OnlineExamSystem.Web.ViewModel
{
    public class CreateQuestionViewModel
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Option 1 is required.")]
        [StringLength(100, ErrorMessage = "Option 1 cannot be longer than 100 characters.")]
        public string Option1 { get; set; } = string.Empty;

        [Required(ErrorMessage = "Option 2 is required.")]
        [StringLength(100, ErrorMessage = "Option 2 cannot be longer than 100 characters.")]
        public string Option2 { get; set; } = string.Empty;

        [Required(ErrorMessage = "Option 3 is required.")]
        [StringLength(100, ErrorMessage = "Option 3 cannot be longer than 100 characters.")]
        public string Option3 { get; set; } = string.Empty;

        [Required(ErrorMessage = "Option 4 is required.")]
        [StringLength(100, ErrorMessage = "Option 4 cannot be longer than 100 characters.")]
        public string Option4 { get; set; } = string.Empty;

        [Required(ErrorMessage = "Correct option is required.")]
        [Range(1, 4, ErrorMessage = "Correct option must be between 1 and 4.")]
        public int CorrectOption { get; set; }

        [Required(ErrorMessage = "Exam ID is required.")]
        public int ExamId { get; set; }
    }
}
