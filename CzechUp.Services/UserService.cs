using CzechUp.EF;
using CzechUp.EF.Models;
using CzechUp.Services.DTOs;
using CzechUp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CzechUp.Services
{
    public class UserService: IUserService
    {
        private readonly PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
        private readonly DatabaseContext _databaseContext;

        public UserService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public int CreateUser(RegistrationRequestDto request)
        {
            var createUser = new User()
            {
                Email = request.Email,
                Login = request.Login,
                Password = HashPassword(request.Password),
                RequiredLanguageLevelId = request.RequiredLanguageLevelId,
                TranslatedLanguageId = request.OriginLanguageId
            };

            _databaseContext.Users.Add(createUser);
            _databaseContext.SaveChanges();

            return createUser.Id;
        }

        public bool Login(LoginRequestDto loginRequest)
        {
            var user = _databaseContext.Users.FirstOrDefault(u => u.Login == loginRequest.Login);
            if (user != null && VerifyPassword(user.Password, loginRequest.Password))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            return passwordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword) == PasswordVerificationResult.Success;
        }

        private string HashPassword(string password)
        {
            return passwordHasher.HashPassword(null, password);
        }
    }
}
