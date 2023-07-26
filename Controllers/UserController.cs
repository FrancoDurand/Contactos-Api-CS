using ContactosAPI.Data;
using ContactosAPI.Model.User;
using Microsoft.AspNetCore.Mvc;

namespace ContactosAPI.Controllers {
	[ApiController]
	[Route("api/user")]
	public class UserController : ControllerBase {
		[HttpGet]
		public async Task<ActionResult<List<User>>> get() {
			var function = new UserRepository();
			var list = await function.ShowUsers();
			return list;
		}

		[HttpPost("login")]
		public async Task<ActionResult<LoginUserResponse>> login([FromBody] LoginUserRequest user) {
			var function = new UserRepository();
			var result = await function.login(user);

			if (result)
				return await function.getId(user);
			else
				return Unauthorized();
		}

		[HttpPut("updateName")]
		public async Task<ActionResult> updateName([FromBody] UpdateNameRequest user) {
			var function = new UserRepository();
			//user.id = id;
			await function.updateName(user);

			return NoContent();
		}

		[HttpPut("updateEmail")]
		public async Task<ActionResult> updateEmail([FromBody] UpdateEmailRequest user) {
			var function = new UserRepository();
			//user.id = id;
			await function.updateEmail(user);

			return NoContent();
		}

		[HttpPut("updatePass")]
		public async Task<ActionResult> updatePass([FromBody] UpdatePasswordRequest user) {
			var function = new UserRepository();
			//user.id = id;
			await function.updatePass(user);

			return NoContent();
		}

		[HttpPost("register")]
		public async Task<ActionResult> registerUser([FromBody] RegisterUserRequest user) {
			var function = new UserRepository();
			await function.createUser(user);
			return StatusCode(201);
		}
	}
}
