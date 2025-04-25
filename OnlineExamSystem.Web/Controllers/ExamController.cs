using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineExamSystem.BLL.Services;
using OnlineExamSystem.Common.Models;
using OnlineExamSystem.Web.ViewModel;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OnlineExamSystem.BLL.Interfaces;

namespace OnlineExamSystem.Web.Controllers
{
    [Authorize]
    public class ExamController : Controller
    {
        private readonly IExamService _examService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ExamController> _logger;

        public ExamController(
            IExamService examService,
            UserManager<ApplicationUser> userManager,
            ILogger<ExamController> logger)
        {
            _examService = examService;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var exams = await _examService.GetExamsWithQuestionsAsync();
            return View(exams);
        }

        public async Task<IActionResult> TakeExam(int id)
        {
            var exam = await _examService.GetExamByIdAsync(id);
            if (exam == null)
            {
                _logger.LogWarning($"Exam with ID {id} not found.");
                return NotFound("Exam not found.");
            }

            var questions = await _examService.GetQuestionsByExamIdAsync(id);
            if (!questions.Any())
            {
                _logger.LogWarning($"No questions found for Exam ID {id}.");
                return View("Error", new ErrorViewModel { Message = "This exam has no questions." });
            }

            ViewBag.Exam = exam;
            return View(questions);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitExam(SubmitExamViewModel model)
        {
            if (model == null || model.ExamId <= 0)
            {
                _logger.LogWarning("Invalid SubmitExam request: Model or ExamId is invalid.");
                return Json(new { success = false, message = "Invalid exam submission." });
            }

            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("User ID could not be retrieved for exam submission.");
                return Json(new { success = false, message = "User authentication failed." });
            }

            var result = await _examService.SubmitExamAsync(userId, model);
            return Json(new { success = result.Success, message = result.Message, redirectUrl = result.RedirectUrl });
        }

        public async Task<IActionResult> Result(int userExamId)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("User ID could not be retrieved for fetching exam result.");
                    return Unauthorized("User authentication failed.");
                }

                var model = await _examService.GetExamResultAsync(userId, userExamId);
                if (model == null)
                {
                    _logger.LogWarning($"Exam result for UserExam ID {userExamId} could not be retrieved.");
                    return NotFound("Exam result not found.");
                }

                return View(model);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error while fetching exam result for UserExam ID {userExamId}.");
                return View("Error", new ErrorViewModel { Message = "An unexpected error occurred while fetching the exam result." });
            }
        }
    }
}