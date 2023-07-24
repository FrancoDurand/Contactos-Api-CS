using ContactosAPI.Connection;
using ContactosAPI.Model;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;

namespace ContactosAPI.Data {
	public class ContactRepository {
		private ConnectionDB connectionManager = new ConnectionDB();

		public async Task<List<ModelContact>> showContacts(int user_id) {
			var contactList = new List<ModelContact>();

			using (var connection = new MySqlConnection(connectionManager.connection)) {
				await connection.OpenAsync();

				string query = $"select u.id, u.name, u.email from user u left join contact c" +
				$" on c.contact_id = u.id where c.user_id = @id";

				using (var command = new MySqlCommand(query, connection)) {
					command.Parameters.AddWithValue("@id", user_id);

					using (var reader = await command.ExecuteReaderAsync()) {
						while (await reader.ReadAsync()) {
							var contact = new ModelContact();

							contact.id = reader.GetInt32("id");
							contact.contact_name = reader.GetString("name");
							contact.contact_email = reader.GetString("email");

							contactList.Add(contact);
						}
					}
				}
			}

			return contactList;
		}

		public async Task deleteContact(ModelUser user, int contact_id) {
			using (var connection = new MySqlConnection(connectionManager.connection)) {
				await connection.OpenAsync();

				var query = $"delete from contact where user_id = @user_id and contact_id = @contact_id";

				using (var command = new MySqlCommand(query,connection)) {
					command.Parameters.AddWithValue("@user_id", user.id);
					command.Parameters.AddWithValue("@contact_id", contact_id);

					await command.ExecuteNonQueryAsync();
				}
			}
		}

		public async Task addContact(ModelUser user, int contact_id) {
			using (var connection = new MySqlConnection(connectionManager.connection)) {
				await connection.OpenAsync();

				var query = $"insert into contact(user_id, contact_id) values (@user_id, @contact_id)";

				using (var command = new MySqlCommand(query, connection)) {
					command.Parameters.AddWithValue("@user_id", user.id);
					command.Parameters.AddWithValue("@contact_id", contact_id);

					await command.ExecuteNonQueryAsync();
				}
			}
		}
	}
}
