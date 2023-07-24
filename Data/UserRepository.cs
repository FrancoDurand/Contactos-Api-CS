using ContactosAPI.Connection;
using ContactosAPI.Model;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;

namespace ContactosAPI.Data {
	public class UserRepository {
		private ConnectionDB connectionManager = new ConnectionDB();

		public async Task<List<ModelUser>> ShowUsers() {
			var userList = new List<ModelUser>();

			using (var connection = new MySqlConnection(connectionManager.connection)) {
				await connection.OpenAsync();

				var query = "SELECT id, name, email, pass, hash_pass FROM user";

				using (var command = new MySqlCommand(query, connection)) {
					using (var reader = await command.ExecuteReaderAsync()) {
						while (await reader.ReadAsync()) {
							var user = new ModelUser {
								id = reader.GetInt32("id"),
								name = reader.GetString("name"),
								email = reader.GetString("email"),
								pass = reader.GetString("pass"),
							};
							userList.Add(user);
						}
					}
				}
			}

			return userList;
		}

		public async Task<bool> login([FromBody] ModelUser user) {
			using (var connection = new MySqlConnection(connectionManager.connection)) {
				await connection.OpenAsync();

				var query = "SELECT email, pass FROM user where email = @email and hash_pass = @hash_pass";

				using (var command = new MySqlCommand(query, connection)) {
					command.Parameters.AddWithValue("@email", user.email);
					command.Parameters.AddWithValue("@hash_pass", Hashier.getHash(user.pass));

					using (var reader = await command.ExecuteReaderAsync()) {
						var result = await reader.ReadAsync();

						return result;
					}
				}
			}
		}

		public async Task<int> getId([FromBody] ModelUser user) {
			int userId = 0;

			using (var connection = new MySqlConnection(connectionManager.connection)) {
				await connection.OpenAsync();

				var query = "SELECT id FROM user where email = @email";

				using (var command = new MySqlCommand(query, connection)) {
					command.Parameters.AddWithValue("@email", user.email);

					using (var reader = await command.ExecuteReaderAsync()) {
						if (await reader.ReadAsync())
							userId = reader.GetInt16("id");
						
						return userId;	
					}
				}
			}
		}

		public async Task updateName([FromBody] ModelUser user) {
			using (var connection = new MySqlConnection(connectionManager.connection)) {
				await connection.OpenAsync();

				var query = "update user set name = @name where id= @id";

				using (var command = new MySqlCommand(query, connection)) {
					command.Parameters.AddWithValue("@name", user.name);
					command.Parameters.AddWithValue("@id", user.id);

					await command.ExecuteNonQueryAsync();
				}
			}
		}

		public async Task updateEmail([FromBody] ModelUser user) {
			using (var connection = new MySqlConnection(connectionManager.connection)) {
				await connection.OpenAsync();

				var query = "update user set email = @email where id= @id";

				using (var command = new MySqlCommand(query, connection)) {
					command.Parameters.AddWithValue("@email", user.email);
					command.Parameters.AddWithValue("@id", user.id);

					await command.ExecuteNonQueryAsync();
				}
			}
		}

		public async Task updatePass([FromBody] ModelUser user) {
			using (var connection = new MySqlConnection(connectionManager.connection)) {
				await connection.OpenAsync();

				var query = "update user set pass = @pass where id= @id";

				using (var command = new MySqlCommand(query, connection)) {
					command.Parameters.AddWithValue("@pass", Hashier.getHash(user.pass));
					command.Parameters.AddWithValue("@id", user.id);

					await command.ExecuteNonQueryAsync();
				}
			}
		}
	}
}
