using ContactosAPI.Connection;
using ContactosAPI.Model.User;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System.Data;
using System.Security.Policy;

namespace ContactosAPI.Data {
	public class UserRepository {
		private ConnectionDB connectionManager = new ConnectionDB();

		public async Task<List<User>> ShowUsers() {
			var userList = new List<User>();

			using (var connection = new MySqlConnection(connectionManager.connection)) {
				await connection.OpenAsync();

				var query = "SELECT id, name, email, pass, hash_pass FROM user";

				using (var command = new MySqlCommand(query, connection)) {
					using (var reader = await command.ExecuteReaderAsync()) {
						while (await reader.ReadAsync()) {
							var user = new User {
								id = reader.GetInt32("id"),
								name = reader.GetString("name"),
								email = reader.GetString("email"),
								password = reader.GetString("pass")
							};

							userList.Add(user);
						}
					}
				}
			}

			return userList;
		}

		public async Task<bool> login(LoginUserRequest user) {
			using (var connection = new MySqlConnection(connectionManager.connection)) {
				await connection.OpenAsync();

				var query = "SELECT email, pass FROM user where email = @email and hash_pass = @hash_pass";

				using (var command = new MySqlCommand(query, connection)) {
					command.Parameters.AddWithValue("@email", user.email);
					command.Parameters.AddWithValue("@hash_pass", Hashier.getHash(user.password));

					using (var reader = await command.ExecuteReaderAsync()) {
						var result = await reader.ReadAsync();

						return result;
					}
				}
			}
		}

		public async Task<LoginUserResponse> getId(LoginUserRequest user) {
			LoginUserResponse response = new LoginUserResponse();

			using (var connection = new MySqlConnection(connectionManager.connection)) {
				await connection.OpenAsync();

				var query = "SELECT id, name FROM user where email = @email";

				using (var command = new MySqlCommand(query, connection)) {
					command.Parameters.AddWithValue("@email", user.email);

					using (var reader = await command.ExecuteReaderAsync()) {
						if (await reader.ReadAsync()) {
							response.id = reader.GetInt16("id");
							response.name = reader.GetString("name");
						}

						return response;
					}
				}
			}
		}

		public async Task updateName(UpdateNameRequest user) {
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

		public async Task updateEmail(UpdateEmailRequest user) {
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

		public async Task updatePass(UpdatePasswordRequest user) {
			using (var connection = new MySqlConnection(connectionManager.connection)) {
				await connection.OpenAsync();

				var query = $"update user set pass = @pass, hash_pass = @hash_pass where id = @id";

				using (var command = new MySqlCommand(query, connection)) {
					command.Parameters.AddWithValue("@pass", user.password);
					command.Parameters.AddWithValue("@hash_pass", Hashier.getHash(user.password));
					command.Parameters.AddWithValue("@id", user.id);

					await command.ExecuteNonQueryAsync();
				}
			}
		}

		public async Task createUser(RegisterUserRequest user) {
			using (var connection = new MySqlConnection(connectionManager.connection)) {
				await connection.OpenAsync();

				var query = "insert into user(name, email, pass, hash_pass) values (@name, @email, @password, @hash)";

				using (var command = new MySqlCommand(query, connection)) {
					command.Parameters.AddWithValue("@name", user.name);
					command.Parameters.AddWithValue("@email", user.email);
					command.Parameters.AddWithValue("@password", user.password);
					command.Parameters.AddWithValue("@hash", Hashier.getHash(user.password));

					await command.ExecuteNonQueryAsync();
				}
			}
		}
	}
}
