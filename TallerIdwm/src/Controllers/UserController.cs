using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


using TallerIdwm.src.dtos;
using TallerIdwm.src.models;
using TallerIdwm.src.data;
using TallerIdwm.src.helpers; 

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TallerIdwm.src.controllers
{
    public class UserController(ILogger<UserController> logger, UnitOfWork unitOfWork) : BaseController
    {
        private readonly ILogger<UserController> _logger = logger;
        private readonly UnitOfWork _context = unitOfWork;

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.UserRepository.GetAllUsersAsync();
            return Ok(users);
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser( CreateUserDto userDto)
        {
            if (userDto.ConfirmPassword != userDto.Password)
            {
                return BadRequest("Passwords do not match");
            }
            var user = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Password = userDto.Password,
                Phone = userDto.Phone,
                ShippingAddress = new ShippingAddress
                {
                    Street = userDto.Street ?? string.Empty,
                    Number = userDto.Number?? string.Empty,
                    Commune = userDto.Commune?? string.Empty,
                    Region = userDto.Region?? string.Empty,
                    PostalCode = userDto.PostalCode?? string.Empty
                }
            };
            await _context.UserRepository.CreateUserAsync(user, user.ShippingAddress);
            await _context.SaveChangesAsync();
            return Ok(user);
        }
    }
}