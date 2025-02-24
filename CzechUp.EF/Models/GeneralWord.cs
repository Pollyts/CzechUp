namespace CzechUp.EF.Models
{
    public class GeneralWord
    {
        public int Id { get; set; }
        public string Original { get; set; }
        public string Translation { get; set; }
        public int LanguageId { get; set; }
        public int LanguageLevelId { get; set; }
        public int GeneralTopicId { get; set; }
        public GeneralTopic GeneralTopic { get; set; }
        public Language Language { get; set; }
        public LanguageLevel LanguageLevel { get; set; }

    }
}
