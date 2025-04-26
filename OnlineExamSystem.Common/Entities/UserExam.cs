using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExamSystem.Common.Entities
{
    public class UserExam : BaseEntity
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public int ExamId { get; set; }
        public Exam Exam { get; set; }
        public int Score { get; set; } // Percentage
        public int CorrectAnswers { get; set; }
        public int TotalQuestions { get; set; }
        public bool Passed { get; set; }
        public DateTime TakenAt { get; set; }
    }
}


