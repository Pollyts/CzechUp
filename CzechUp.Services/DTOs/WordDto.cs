namespace CzechUp.Services.DTOs
{
    public class WordDto
    {
        public Guid Guid { get; set; }
        public string Word { get; set; }
        public string LanguageLevel { get; set; }
        public List<string> Topics { get; set; }
        public List<string> Translations { get; set; }
        public List<WordExampleDto> WordExamples { get; set; }
        public List<WordFormDto> WordForms { get; set; }
        public List<string> Tags { get; set; }
    }

    public class WordFormDto
    {
        public Guid Guid { get; set; }
        public string Tag { get; set; }
        public string Form { get; set; }
    }

    public class WordExampleDto
    {
        public Guid Guid { get; set; }
        public string OriginalExample { get; set; }
        public string TranslatedExample { get; set; }
    }

}
