using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineExamSystem.BLL.Dtos;
using OnlineExamSystem.BLL.ServiceInterfaces;
using OnlineExamSystem.BLL.Services;
using System.Security.Claims;

namespace OnlineExamSystem.Web.Controllers
{
    [Authorize]
    public class UserExamsController : Controller
    {
        private readonly IUserExamService _userExamService;

        public UserExamsController(IUserExamService userExamService)
        {
            _userExamService = userExamService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userExams = await _userExamService.GetUserExamsAsync(userId);
            return View(userExams);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(int examId, Dictionary<int, int> userAnswers)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userExam = await _userExamService.SubmitExamAsync(userId, examId, userAnswers);
            return RedirectToAction(nameof(Result), new { id = userExam.Id });
        }

        public async Task<IActionResult> Result(int id)
        {
            var userExam = await _userExamService.GetUserExamByIdAsync(id);
            if (userExam == null)
            {
                return NotFound();
            }

            return View(userExam);
        }
    }
}