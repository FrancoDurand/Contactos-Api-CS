var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.UseCors(builder => builder.AllowAnyOrigin()
							  .AllowAnyMethod()
							  .AllowAnyHeader());

app.Run();
