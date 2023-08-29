namespace Survey.Data
{
    public class Answer
    {
        public string SurveyName { get; set; } = "";
        public string Question { get; set; } = "";
        public string Result { get; set; } = "";
        public DateTime Date { get; set; }
    }
}
