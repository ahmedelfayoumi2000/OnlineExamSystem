using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExamSystem.Common.Models
{
    public class UserExam
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!; 
        public int ExamId { get; set; }
        public Exam Exam { get; set; } = null!;
        public DateTime TakenAt { get; set; }
        public int Score { get; set; }
        public bool IsPassed { get; set; }
        public List<UserAnswer> UserAnswers { get; set; } = new();
    }
}


