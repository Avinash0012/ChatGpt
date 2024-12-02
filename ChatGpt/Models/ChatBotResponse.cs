namespace ADOCRUD.Models
{

    public class SafetyRating
    {
        public string Category { get; set; }
        public string Probability { get; set; }
    }

    public class ContentPart
    {
        public string Text { get; set; }
    }

    public class Content
    {
        public List<ContentPart> Parts { get; set; }
        public string Role { get; set; }
    }

    public class Candidate
    {
        public Content Content { get; set; }
        public string FinishReason { get; set; }
        public int Index { get; set; }
        public List<SafetyRating> SafetyRatings { get; set; }
    }

    public class RootObject
    {
        public List<Candidate> Candidates { get; set; }
    }



}
