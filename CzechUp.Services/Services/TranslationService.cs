using CzechUp.EF;
using CzechUp.EF.Models;
using CzechUp.Services.DTOs;
using CzechUp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CzechUp.Services.Services
{
    public class TranslationService : ITranslationService
    {
        private readonly DatabaseContext _databaseContext;

        public TranslationService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
    }
}
