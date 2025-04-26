namespace CzechUp.Services.DTOs
{   
    public class FilterWordDto
    {
        public List<Guid> Tags { get; set; }
        public List<Guid> Topics { get; set; }
    }
}
