using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineExamSystem.BLL.Dtos;
using OnlineExamSystem.BLL.ServiceInterfaces;
using OnlineExamSystem.BLL.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineExamSystem.Web.Controllers
{
    [Authorize]
    public class ExamsController : Controller
    {
        private readonly IExamRetrievalService _examRetrievalService;
        private readonly IQuestionService _questionService;
        private readonly IUserExamService _userExamService;

        public ExamsController(IExamRetrievalService examRetrievalService, IQuestionService questionService, IUserExamService userExamService)
        {
            _examRetrievalService = examRetrievalService ?? throw new ArgumentNullException(nameof(examRetrievalService));
            _questionService = questionService ?? throw new ArgumentNullException(nameof(questionService));
            _userExamService = userExamService ?? throw new ArgumentNullException(nameof(userExamService));
        }

        public async Task<IActionResult> TakeExam(int id)
        {
            if (id <= 0) return BadRequest();
            var exam = await _examRetrievalService.GetExamByIdAsync(id);
            if (exam == null) return NotFound();

            var questions = await _questionService.GetQuestionsByExamIdAsync(id);
            ViewBag.Exam = exam;
            return View(questions);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitExam(int examId, Dictionary<int, int> answers)
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                var userExamDto = await _userExamService.SubmitExamAsync(userId, examId, answers);
                return Json(new { success = true, redirectUrl = Url.Action("Result", new { id = userExamDto.Id }) });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> Result(int id)
        {
            if (id <= 0) return BadRequest();
            var userExam = await _userExamService.GetUserExamByIdAsync(id);
            if (userExam == null) return NotFound();
            return View(userExam);
        }

        public async Task<IActionResult> History()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var userExams = await _userExamService.GetUserExamsAsync(userId);
            return View(userExams);
        }
    }
}