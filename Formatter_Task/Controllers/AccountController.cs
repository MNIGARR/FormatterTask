﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Formatter_Task.Dtos;
using Formatter_Task.Services.Abstract;

namespace Formatter_Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public AccountController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost("SignIn")]
        public string SignIn(SignInDto dto)
        {
            var student = _studentService.Get(x => x.Username == dto.Username && x.Password == dto.Password);
            if (student != null)
            {
                var token = student.Username + ":" + student.Password;
                var returnedToken=Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
                return returnedToken;
            }
            return "";
        }

    }
}
