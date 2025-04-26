using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExamSystem.BLL.Dtos
{
    public class UserExamDto
    {
        public int Id { get; set; }
        public int ExamId { get; set; }
        public ExamDto Exam { get; set; }
        public int Score { get; set; }
        public int CorrectAnswers { get; set; }
        public int TotalQuestions { get; set; }
        public bool Passed { get; set; }
        public DateTime TakenAt { get; set; }
    }
}
