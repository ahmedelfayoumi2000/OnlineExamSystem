using OnlineExamSystem.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExamSystem.Common.Entities
{
    public class Question : BaseEntity
    {
        public string Text { get; set; }
        public int Points { get; set; } = 1; // Each question worth 1 point by default
        public int ExamId { get; set; }
        public Exam Exam { get; set; }
        public ICollection<Option> Options { get; set; } = new List<Option>();
    }
}
