using CzechUp.EF.Models;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CzechUp.Services.DTOs
{
    public class RegistrationRequestDto
    {
        public string Password { get; set; }        
        public string Email { get; set; }

        public Guid RequiredLanguageLevelGuid { get; set; }

        public Guid OriginLanguageGuid { get; set; }
    }
}
