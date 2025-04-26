using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExamSystem.Common.Entities
{
    public class Exam : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int PassingScore { get; set; } = 60;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<Question> Questions { get; set; } = new List<Question>();
    }
}
