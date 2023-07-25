using ContactosAPI.Data;
using ContactosAPI.Model.Contact;
using ContactosAPI.Model.User;
using Microsoft.AspNetCore.Mvc;

namespace ContactosAPI.Controllers {
	[ApiController]
	[Route("api/contact")]
	public class ContactController {
		[HttpPost]
		public async Task<ActionResult<List<Contact>>> get([FromBody] User user) {
			var function = new ContactRepository();
			var result = await function.showContacts(user);
			return result;
		}

		[HttpDelete("delete")]
		public async Task DeleteContact([FromBody] DeleteContactRequest request) {
			var function = new ContactRepository();
			await function.deleteContact(request);
		}

		[HttpPost("add")]
		public async Task AddContact([FromBody] AddContactRequest request) {
			var function = new ContactRepository();
			await function.addContact(request);
		}

	}
}
