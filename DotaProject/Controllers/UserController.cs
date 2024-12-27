using DotaProject.Identity;
using DotaProject.Models;
using DotaProject.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotaProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IUserService userService) : ControllerBase
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
            {
                return BadRequest("Username and password are required.");
            }

            var newUser = await userService.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
        }

        
        [HttpGet]
        [Authorize(Policy = IdentityPolicyConstants.VerifiedUserPolicyName)]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await userService.GetAllUsersAsync();
            return Ok(users);
        }

        
        [HttpGet("{id:int}")]
        [Authorize(Policy = IdentityPolicyConstants.VerifiedUserPolicyName)]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            return Ok(user);
        }

        
        [HttpPut("{id:int}")]
        [Authorize(Policy = IdentityPolicyConstants.AdminUserPolicyName)]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User updatedUser)
        {
            var user = await userService.UpdateUserAsync(id, updatedUser);
            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            return NoContent();
        }

        
        [HttpDelete("{id:int}")]
        [Authorize(Policy = IdentityPolicyConstants.AdminUserPolicyName)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await userService.DeleteUserAsync(id);
            if (!result)
            {
                return NotFound($"User with ID {id} not found.");
            }

            return NoContent();
        }

        
        [HttpPost("{id:int}/setAdmin")]
        [Authorize(Policy = IdentityPolicyConstants.AdminUserPolicyName)]
        public async Task<IActionResult> SetAdmin(int id)
        {
            var result = await userService.SetAdminAsync(id);
            if (!result)
            {
                return BadRequest($"Failed to promote user with ID {id} to 'admin'.");
            }

            return NoContent();
        }

        
        [HttpPost("{id:int}/setVerifiedUser")]
        [Authorize(Policy = IdentityPolicyConstants.AdminUserPolicyName)]
        public async Task<IActionResult> SetVerifiedUser(int id)
        {
            var result = await userService.SetVerifiedUserAsync(id);
            if (!result)
            {
                return BadRequest($"Failed to demote user with ID {id} to 'verifiedUser'.");
            }

            return NoContent();
        }
    }
}
