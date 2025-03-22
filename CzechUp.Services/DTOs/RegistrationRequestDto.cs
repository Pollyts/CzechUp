using CzechUp.EF.Models;

namespace CzechUp.Services.DTOs
{
    public class RegistrationRequestDto
    {
        public string Login { get; set; }
        public string Password { get; set; }        
        public string Email { get; set; }
        public int RequiredLanguageLevelId { get; set; }
        public int OriginLanguageId { get; set; }
    }
}
