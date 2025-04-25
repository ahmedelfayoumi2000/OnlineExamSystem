using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineExamSystem.BLL.Services;
using OnlineExamSystem.Web.ViewModel;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OnlineExamSystem.BLL.Interfaces;

namespace OnlineExamSystem.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IAdminService adminService, ILogger<AdminController> logger)
        {
            _adminService = adminService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var exams = await _adminService.GetAllExamsAsync();
            return View(exams);
        }

        public IActionResult CreateExam()
        {
            return View(new CreateExamViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateExam(CreateExamViewModel model)
        {
            if (!ModelState.IsValid)
            {
                LogModelStateErrors();
                return View(model);
            }

            await _adminService.CreateExamAsync(model);
            TempData["Success"] = "Exam created successfully!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> EditExam(int id)
        {
            var exam = await _adminService.GetExamByIdAsync(id);
            if (exam == null)
            {
                return NotFound();
            }

            var model = new CreateExamViewModel { Title = exam.Title };
            ViewBag.Id = id;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditExam(int id, CreateExamViewModel model)
        {
            if (!ModelState.IsValid)
            {
                LogModelStateErrors();
                return View(model);
            }

            try
            {
                await _adminService.UpdateExamAsync(id, model);
                TempData["Success"] = "Exam updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> DeleteExam(int id)
        {
            try
            {
                await _adminService.DeleteExamAsync(id);
                TempData["Success"] = "Exam deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> Questions(int examId)
        {
            var exam = await _adminService.GetExamByIdAsync(examId);
            if (exam == null)
            {
                return NotFound();
            }

            var questions = await _adminService.GetQuestionsByExamIdAsync(examId);
            ViewBag.ExamId = examId;
            ViewBag.ExamTitle = exam.Title;
            return View(questions);
        }

        public IActionResult CreateQuestion(int examId)
        {
            var model = new CreateQuestionViewModel { ExamId = examId };
            ViewBag.ExamId = examId;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuestion(CreateQuestionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                LogModelStateErrors();
                ViewBag.ExamId = model.ExamId;
                return View(model);
            }

            await _adminService.CreateQuestionAsync(model);
            TempData["Success"] = "Question created successfully!";
            return RedirectToAction(nameof(Questions), new { examId = model.ExamId });
        }

        public async Task<IActionResult> EditQuestion(int id)
        {
            var question = await _adminService.GetQuestionByIdAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            var model = new CreateQuestionViewModel
            {
                Title = question.Title,
                Option1 = question.Option1,
                Option2 = question.Option2,
                Option3 = question.Option3,
                Option4 = question.Option4,
                CorrectOption = question.CorrectOption,
                ExamId = question.ExamId
            };
            ViewBag.ExamId = question.ExamId;
            ViewBag.QuestionId = id;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditQuestion(int id, CreateQuestionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                LogModelStateErrors();
                ViewBag.ExamId = model.ExamId;
                return View(model);
            }

            try
            {
                await _adminService.UpdateQuestionAsync(id, model);
                TempData["Success"] = "Question updated successfully!";
                return RedirectToAction(nameof(Questions), new { examId = model.ExamId });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> DeleteQuestion(int id)
        {
            try
            {
                var question = await _adminService.GetQuestionByIdAsync(id);
                if (question == null)
                {
                    return NotFound();
                }

                await _adminService.DeleteQuestionAsync(id);
                TempData["Success"] = "Question deleted successfully!";
                return RedirectToAction(nameof(Questions), new { examId = question.ExamId });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        public IActionResult CreateUser()
        {
            return View(new CreateUserViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                LogModelStateErrors();
                return View(model);
            }

            try
            {
                await _adminService.CreateUserAsync(model);
                TempData["Success"] = "User created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        private void LogModelStateErrors()
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                _logger.LogWarning($"ModelState Error: {error.ErrorMessage}");
            }
        }
    }
}