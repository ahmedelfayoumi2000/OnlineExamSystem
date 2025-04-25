namespace OnlineExamSystem.Web.ViewModel
{
    public class SubmitExamViewModel
    {
        public int ExamId { get; set; }
        public Dictionary<int, int> Answers { get; set; } = new Dictionary<int, int>();
    }
}
