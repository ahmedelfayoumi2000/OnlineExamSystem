namespace OnlineExamSystem.BLL.Dtos
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int ExamId { get; set; }
        public List<OptionDto> Options { get; set; }
    }
}