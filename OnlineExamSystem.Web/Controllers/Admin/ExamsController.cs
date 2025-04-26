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
    public class ExamsController : Controller
    {
        private readonly IExamRetrievalService _examRetrievalService;
        private readonly IExamModificationService _examModificationService;

        public ExamsController(IExamRetrievalService examRetrievalService, IExamModificationService examModificationService)
        {
            _examRetrievalService = examRetrievalService ?? throw new ArgumentNullException(nameof(examRetrievalService));
            _examModificationService = examModificationService ?? throw new ArgumentNullException(nameof(examModificationService));
        }

        public async Task<IActionResult> Index()
        {
            var exams = await _examRetrievalService.GetAllExamsAsync();
            return View(exams);
        }

        public IActionResult Create()
        {
            return View(new ExamDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExamDto examDto)
        {
            if (!ModelState.IsValid)
            {
                return View(examDto);
            }

            try
            {
                await _examModificationService.CreateExamAsync(examDto);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(examDto);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0) return BadRequest();
            var exam = await _examRetrievalService.GetExamByIdAsync(id);
            if (exam == null) return NotFound();
            return View(exam);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ExamDto examDto)
        {
            if (!ModelState.IsValid)
            {
                return View(examDto);
            }

            try
            {
                await _examModificationService.UpdateExamAsync(examDto);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(examDto);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            var exam = await _examRetrievalService.GetExamByIdAsync(id);
            if (exam == null) return NotFound();
            return View(exam);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _examModificationService.DeleteExamAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                var exam = await _examRetrievalService.GetExamByIdAsync(id);
                return View(exam);
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0) return BadRequest();
            var exam = await _examRetrievalService.GetExamByIdAsync(id);
            if (exam == null) return NotFound();
            return View(exam);
        }
    }
}