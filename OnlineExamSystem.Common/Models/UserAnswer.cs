using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExamSystem.Common.Models
{
    public class UserAnswer
    {
        public int Id { get; set; }
        public int UserExamId { get; set; }
        public UserExam UserExam { get; set; } = null!;
        public int QuestionId { get; set; }
        public Question Question { get; set; } = null!;
        public int SelectedOption { get; set; }
    }
}
