using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineExamSystem.BLL.Dtos;
using OnlineExamSystem.BLL.ServiceInterfaces;
using OnlineExamSystem.BLL.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineExamSystem.Web.Controllers
{
    [Authorize]
    public class ExamsController : Controller
    {
        private readonly IExamRetrievalService _examRetrievalService;
        private readonly IUserExamService _userExamService;

        public ExamsController(IExamRetrievalService examRetrievalService, IUserExamService userExamService)
        {
            _examRetrievalService = examRetrievalService;
            _userExamService = userExamService;
        }

        public async Task<IActionResult> Index()
        {
            var exams = await _examRetrievalService.GetAllExamsAsync();
            return View(exams);
        }

        public async Task<IActionResult> Take(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userExams = await _userExamService.GetUserExamsAsync(userId);
            if (userExams.Any(ue => ue.ExamId == id))
            {
                TempData["ErrorMessage"] = "You have already taken this exam.";
                return RedirectToAction(nameof(Index));
            }

            var exam = await _examRetrievalService.GetExamByIdAsync(id);
            if (exam == null)
            {
                return NotFound();
            }
            return View(exam);
        }
    }
}