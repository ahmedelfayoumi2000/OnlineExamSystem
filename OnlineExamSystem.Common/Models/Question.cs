using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExamSystem.Common.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        public int CorrectOption { get; set; } // 1, 2, 3, or 4
        public int ExamId { get; set; }
        public Exam Exam { get; set; }
        public ICollection<UserAnswer> UserAnswers { get; set; }
    }
}
