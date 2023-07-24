using ContactosAPI.Data;
using ContactosAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace ContactosAPI.Controllers {
	[ApiController]
	[Route("api/user")]
	public class UserController: ControllerBase {
		/*[HttpGet] public async Task<ActionResult<List<ModelUser>>> get() {
			var function = new UserRepository();
			var list = await function.ShowUsers();
			return list;
		}*/

		[HttpPost("login")] 
		public async Task<ActionResult<int>> login([FromBody] ModelUser user) {
			var function = new UserRepository();
			var result = await function.login(user);

			if (result)
				return await function.getId(user);
			else
				return Unauthorized();
		}

		[HttpPut("updateName")]
		public async Task<ActionResult> updateName([FromBody] ModelUser user) {
			var function = new UserRepository();
			//user.id = id;
			await function.updateName(user);

			return NoContent();
		}

		[HttpPut("updateEmail")]
		public async Task<ActionResult> updateEmail([FromBody] ModelUser user) {
			var function = new UserRepository();
			//user.id = id;
			await function.updateEmail(user);

			return NoContent();
		}

		[HttpPut("updatePass")]
		public async Task<ActionResult> updatePass([FromBody] ModelUser user) {
			var function = new UserRepository();
			//user.id = id;
			await function.updatePass(user);

			return NoContent();
		}
	}
}
