using CzechUp.EF.Models;
using CzechUp.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CzechUp.Services.Interfaces
{
    public interface IUserService
    {
        Guid CreateUser(RegistrationRequestDto request);
        User? Login(LoginRequestDto loginRequest);
    }
}
