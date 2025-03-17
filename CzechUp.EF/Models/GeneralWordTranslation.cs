namespace CzechUp.EF.Models
{
    public class GeneralWordTranslation
    {
        public int Id { get; set; }
        public string Translation { get; set; }
        public int GeneralOriginalWordId { get; set; }
        public int LanguageId { get; set; }
        public GeneralOriginalWord GeneralOriginalWord { get; set; }
        public Language Language { get; set; }
    }
}
