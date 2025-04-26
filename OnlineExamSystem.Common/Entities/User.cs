using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace OnlineExamSystem.Common.Entities
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public ICollection<UserExam> UserExams { get; set; } = new List<UserExam>();
    }
}