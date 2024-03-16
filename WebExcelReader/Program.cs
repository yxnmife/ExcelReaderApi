using Microsoft.EntityFrameworkCore;
using WebExcelReader.Data;
using WebExcelReader.Repository; // Import the namespace where IReadDatabase is defined

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("MyConnectionString")));

// Add service registration for IReadDatabase
builder.Services.AddScoped<IReadDatabase, ReadDatabase>(); // Assuming ReadDatabase implements IReadDatabase

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();

app.Run();
