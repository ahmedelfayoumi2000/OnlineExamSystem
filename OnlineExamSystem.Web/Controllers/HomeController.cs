using Microsoft.AspNetCore.Mvc;
using OnlineExamSystem.BLL.ServiceInterfaces;

public class HomeController : Controller
{
    private readonly IExamRetrievalService _examRetrievalService;

    public HomeController(IExamRetrievalService examRetrievalService)
    {
        _examRetrievalService = examRetrievalService ?? throw new ArgumentNullException(nameof(examRetrievalService));
    }

    public async Task<IActionResult> Index()
    {
        var exams = await _examRetrievalService.GetAllExamsAsync();
        return View(exams);
    }
}