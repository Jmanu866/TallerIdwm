using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TallerIdwm.src.data;
using TallerIdwm.src.dtos;
using TallerIdwm.src.extensions;
using TallerIdwm.src.helpers;
using TallerIdwm.src.mappers;
using TallerIdwm.src.models;
using TallerIdwm.src.RequestHelpers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
namespace TallerIdwm.src.controllers
{
    public class UserController(ILogger<UserController> logger, UnitOfWork unitOfWork) : BaseController
    {
        private readonly ILogger<UserController> _logger = logger;
        private readonly UnitOfWork _unitOfWork = unitOfWork;



        // GET /user?params...
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserDto>>>> GetAll([FromQuery] UserParams userParams)
        {
            var query = _unitOfWork.UserRepository.GetUsersQueryable();

            if (userParams.IsActive.HasValue)
                query = query.Where(u => u.IsActive == userParams.IsActive.Value);

            if (!string.IsNullOrWhiteSpace(userParams.SearchTerm))
            {
                var term = userParams.SearchTerm.ToLower();
                query = query.Where(u =>
                    u.FirstName.Contains(term) ||
                    u.LastName.Contains(term) ||
                    (u.Email != null && u.Email.ToLower().Contains(term)));
            }

            if (userParams.RegisteredFrom.HasValue)
                query = query.Where(u => u.RegisteredAt >= userParams.RegisteredFrom.Value);

            if (userParams.RegisteredTo.HasValue)
                query = query.Where(u => u.RegisteredAt <= userParams.RegisteredTo.Value);

            var total = await query.CountAsync();

            var users = await query
                .OrderByDescending(u => u.RegisteredAt)
                .Skip((userParams.PageNumber - 1) * userParams.PageSize)
                .Take(userParams.PageSize)
                .ToListAsync();

            var dtos = users.Select(u => UserMapper.UserToUserDto(u)).ToList();

            Response.AddPaginationHeader(new PaginationMetaData
            {
                CurrentPage = userParams.PageNumber,
                TotalPages = (int)Math.Ceiling(total / (double)userParams.PageSize),
                PageSize = userParams.PageSize,
                TotalCount = total
            });

            return Ok(new ApiResponse<IEnumerable<UserDto>>(true, "Usuarios obtenidos correctamente", dtos));
        }

        [Authorize(Roles = "Admin")]
        // GET /users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetById(string id)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(id);
            if (user == null)
                return NotFound(new ApiResponse<string>(false, "Usuario no encontrado"));

            var dto = UserMapper.UserToUserDto(user);
            return Ok(new ApiResponse<UserDto>(true, "Usuario encontrado", dto));
        }
        [Authorize(Roles = "Admin")]
        // PUT /users/{id}/status
        [HttpPut("{id}/status")]
        public async Task<ActionResult<ApiResponse<string>>> ToggleStatus(string id, [FromBody] ToggleStatusDto dto)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(id);
            if (user == null)
                return NotFound(new ApiResponse<string>(false, "Usuario no encontrado"));

            user.IsActive = !user.IsActive;
            user.DeactivationReason = user.IsActive ? null : dto.Reason;

            await _unitOfWork.UserRepository.UpdateUserAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var message = user.IsActive ? "Usuario habilitado correctamente" : "Usuario deshabilitado correctamente";
            return Ok(new ApiResponse<string>(true, message));
        }
         [Authorize(Roles = "User")]
        [HttpPost("address")]
        public async Task<ActionResult<ApiResponse<ShippingAddress>>> CreateShippingAddress([FromBody] CreateShippingAddressDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized(new ApiResponse<string>(false, "Usuario no autenticado"));

            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);
            if (user is null)
                return NotFound(new ApiResponse<string>(false, "Usuario no encontrado"));
            var existing = await _unitOfWork.ShippingAddressRepository.GetByUserIdAsync(userId);
            var hasExistingData = existing != null && !string.IsNullOrWhiteSpace(existing.Street) &&
                !string.IsNullOrWhiteSpace(existing.Number) &&
                !string.IsNullOrWhiteSpace(existing.Commune) &&
                !string.IsNullOrWhiteSpace(existing.Region) &&
                !string.IsNullOrWhiteSpace(existing.PostalCode);

            if (hasExistingData)
                return BadRequest(new ApiResponse<string>(false, "Ya tienes una dirección registrada válida"));

            var address = ShippingAddressMapper.FromDto(dto, userId);

            await _unitOfWork.ShippingAddressRepository.AddAsync(address);
            await _unitOfWork.SaveChangesAsync();
            var addressDto = ShippingAddressMapper.ToDto(address);
            return Ok(new ApiResponse<ShippingAddressDto>(true, "Dirección creada exitosamente", addressDto));
        }


        [Authorize(Roles = "User")]
        [HttpPatch("profile")]
        public async Task<ActionResult<ApiResponse<UserDto>>> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
                return Unauthorized(new ApiResponse<string>(false, "Usuario no autenticado"));

            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);
            if (user is null)
                return NotFound(new ApiResponse<string>(false, "Usuario no encontrado"));

            UserMapper.UpdateUserFromDto(user, dto);

            await _unitOfWork.UserRepository.UpdateUserAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return Ok(new ApiResponse<UserDto>(true, "Perfil actualizado correctamente", UserMapper.UserToUserDto(user)));
        }

        [Authorize(Roles = "User")]
        [HttpPatch("profile/password")]
        public async Task<ActionResult<ApiResponse<string>>> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
                return Unauthorized(new ApiResponse<string>(false, "Usuario no autenticado"));

            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);
            if (user is null)
                return NotFound(new ApiResponse<string>(false, "Usuario no encontrado"));

            if (dto.NewPassword != dto.ConfirmPassword)
                return BadRequest(new ApiResponse<string>(false, "La nueva contraseña y la confirmación no coinciden"));
            if (dto.NewPassword == dto.CurrentPassword) return BadRequest(new ApiResponse<string>(false, "La nueva contraseña no puede ser igual a la actual"));

            var result = await _unitOfWork.UserRepository.UpdatePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(new ApiResponse<string>(
                    false,
                    "Error al cambiar la contraseña",
                    null,
                    result.Errors.Select(e => e.Description).ToList()
                ));
            }

            return Ok(new ApiResponse<string>(true, "Contraseña actualizada correctamente"));
        }
        [Authorize(Roles = "User")]
        [HttpGet("profile")]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
                return Unauthorized(new ApiResponse<string>(false, "Usuario no autenticado"));

            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound(new ApiResponse<string>(false, "Usuario no encontrado"));

            var dto = UserMapper.UserToUserDto(user);
            return Ok(new ApiResponse<UserDto>(true, "Perfil del usuario obtenido", dto));
        }
        [Authorize(Roles = "User")]
        [HttpPut("address")]
        public async Task<ActionResult<ApiResponse<ShippingAddress>>> UpdateShippingAddress([FromBody] CreateShippingAddressDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized(new ApiResponse<string>(false, "Usuario no autenticado"));

            var address = await _unitOfWork.ShippingAddressRepository.GetByUserIdAsync(userId);
            if (address == null)
                return NotFound(new ApiResponse<string>(false, "No tienes una dirección registrada. Usa el método POST para crear una."));


            address.Street = dto.Street;
            address.Number = dto.Number;
            address.Commune = dto.Commune;
            address.Region = dto.Region;
            address.PostalCode = dto.PostalCode;

            await _unitOfWork.SaveChangesAsync();

            return Ok(new ApiResponse<ShippingAddress>(true, "Dirección actualizada correctamente", address));
        }
    }
}