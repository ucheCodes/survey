namespace Survey.Data
{
    public class Questions
    {
        public string Id { get; set; } = "";
        public string Question { get; set; } = string.Empty;
        public bool Answer { get; set; } = false;
        public Surveys Survey { get; set; } = new();
        public string AnswerId { get; set; } = "";
        public DateTime Date { get; set; }
    }
}
