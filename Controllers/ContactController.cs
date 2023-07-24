using ContactosAPI.Data;
using ContactosAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace ContactosAPI.Controllers {
	[ApiController]
	[Route("api/contact")]
	public class ContactController {
		[HttpGet] 
		public async Task<ActionResult<List<ModelContact>>> get(int user_id) {
			var function = new ContactRepository();
			var result = await function.showContacts(user_id);
			return result;
		}

		[HttpDelete("delete")]
		public async Task DeleteContact([FromBody] DeleteContactRequest request) {
			var function = new ContactRepository();
			await function.deleteContact(request.user, request.contact_id);
		}

		[HttpPost("add")]
		public async Task AddContact([FromBody] AddContactRequest request) {
			var function = new ContactRepository();
			await function.addContact(request.user, request.contact_id);
		}

	}
}
