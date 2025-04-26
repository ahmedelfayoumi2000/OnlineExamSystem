using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineExamSystem.BLL.Dtos;
using OnlineExamSystem.BLL.ServiceInterfaces;
using OnlineExamSystem.BLL.Services;
using System.Threading.Tasks;

namespace OnlineExamSystem.Web.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class QuestionsController : Controller
    {
        private readonly IQuestionService _questionService;
        private readonly IExamRetrievalService _examRetrievalService;

        public QuestionsController(IQuestionService questionService, IExamRetrievalService examRetrievalService)
        {
            _questionService = questionService ?? throw new ArgumentNullException(nameof(questionService));
            _examRetrievalService = examRetrievalService ?? throw new ArgumentNullException(nameof(examRetrievalService));
        }

        public async Task<IActionResult> Index(int examId)
        {
            if (examId <= 0) return BadRequest();
            var questions = await _questionService.GetQuestionsByExamIdAsync(examId);
            ViewBag.ExamId = examId;
            return View(questions);
        }

        public async Task<IActionResult> Create(int examId)
        {
            if (examId <= 0) return BadRequest();
            var exam = await _examRetrievalService.GetExamByIdAsync(examId);
            if (exam == null) return NotFound();

            var questionDto = new QuestionDto
            {
                ExamId = examId,
                Options = new List<OptionDto>
                {
                    new OptionDto(),
                    new OptionDto(),
                    new OptionDto(),
                    new OptionDto()
                }
            };
            return View(questionDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuestionDto questionDto)
        {
            if (!ModelState.IsValid)
            {
                return View(questionDto);
            }

            try
            {
                await _questionService.CreateQuestionAsync(questionDto);
                return RedirectToAction(nameof(Index), new { examId = questionDto.ExamId });
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(questionDto);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0) return BadRequest();
            var question = await _questionService.GetQuestionByIdAsync(id);
            if (question == null) return NotFound();

            while (question.Options.Count < 4)
            {
                question.Options.Add(new OptionDto());
            }

            return View(question);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(QuestionDto questionDto)
        {
            if (!ModelState.IsValid)
            {
                return View(questionDto);
            }

            try
            {
                await _questionService.UpdateQuestionAsync(questionDto);
                return RedirectToAction(nameof(Index), new { examId = questionDto.ExamId });
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(questionDto);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            var question = await _questionService.GetQuestionByIdAsync(id);
            if (question == null) return NotFound();
            return View(question);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var question = await _questionService.GetQuestionByIdAsync(id);
            if (question == null) return NotFound();

            try
            {
                await _questionService.DeleteQuestionAsync(id);
                return RedirectToAction(nameof(Index), new { examId = question.ExamId });
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(question);
            }
        }
    }
}