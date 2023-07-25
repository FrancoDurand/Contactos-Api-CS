using ContactosAPI.Connection;
using ContactosAPI.Model.Contact;
using ContactosAPI.Model.User;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Data;

namespace ContactosAPI.Data {
	public class ContactRepository {
		private ConnectionDB connectionManager = new ConnectionDB();

		public async Task<List<Contact>> showContacts(User user) {
			var contactList = new List<Contact>();

			using (var connection = new MySqlConnection(connectionManager.connection)) {
				await connection.OpenAsync();

				string query = $"select u.id, u.name, u.email from user u left join contact c" +
				$" on c.contact_id = u.id where c.user_id = @id";

				using (var command = new MySqlCommand(query, connection)) {
					command.Parameters.AddWithValue("@id", user.id);

					using (var reader = await command.ExecuteReaderAsync()) {
						while (await reader.ReadAsync()) {
							var contact = new Contact {
								id = reader.GetInt32("id"),
								name = reader.GetString("name"),
								email = reader.GetString("email"),
							};

							contactList.Add(contact);
						}
					}
				}
			}

			return contactList;
		}

		public async Task deleteContact(DeleteContactRequest request) {
			using (var connection = new MySqlConnection(connectionManager.connection)) {
				await connection.OpenAsync();

				var query = $"delete from contact where user_id = @user_id and contact_id = @contact_id";

				using (var command = new MySqlCommand(query, connection)) {
					command.Parameters.AddWithValue("@user_id", request.userId);
					command.Parameters.AddWithValue("@contact_id", request.contactId);

					await command.ExecuteNonQueryAsync();
				}
			}
		}

		public async Task addContact(AddContactRequest request) {
			using (var connection = new MySqlConnection(connectionManager.connection)) {
				await connection.OpenAsync();

				var query = $"insert into contact(user_id, contact_id) values (@user_id, @contact_id)";

				using (var command = new MySqlCommand(query, connection)) {
					command.Parameters.AddWithValue("@user_id", request.userId);
					command.Parameters.AddWithValue("@contact_id", request.contactId);

					await command.ExecuteNonQueryAsync();
				}
			}
		}
	}
}
