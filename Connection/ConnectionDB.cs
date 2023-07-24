namespace ContactosAPI.Connection {
	public class ConnectionDB {
		public string connection { get; private set; }
		public ConnectionDB() {
			var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
			connection = builder.GetSection("ConnectionStrings:connection").Value;
		}
	}
}
