using OnlineExamSystem.BLL.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExamSystem.BLL.ServiceInterfaces
{
    public interface IUserExamService
    {
        Task<UserExamDto> SubmitExamAsync(string userId, int examId, Dictionary<int, int> userAnswers);
        Task<IReadOnlyList<UserExamDto>> GetUserExamsAsync(string userId);
        Task<UserExamDto> GetUserExamByIdAsync(int userExamId);
    }
}
