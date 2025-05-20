using Cwiczenie_11.Data;
using Cwiczenie_11.Services;
using Tutorial5.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AddDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();