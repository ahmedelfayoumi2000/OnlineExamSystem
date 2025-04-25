using OnlineExamSystem.Common.Models;

namespace OnlineExamSystem.Web.ViewModel
{
    public class ExamResultViewModel
    {
        public UserExam UserExam { get; set; } = null!;
        public List<Question> Questions { get; set; } = new();
        public List<UserAnswer> UserAnswers { get; set; } = new();
        public int CorrectAnswers { get; set; }
        public int TotalQuestions { get; set; }
    }
}
