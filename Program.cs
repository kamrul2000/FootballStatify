using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi; // ✔ Only this
using MyApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Football Manager API",
        Version = "v1",
        Description = "A complete Football Management System with Teams, Players, Matches, Results & Stats",
        Contact = new OpenApiContact
        {
            Name = "Football Manager",
            Email = "support@footballmanager.com"
        }
    });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy => policy
            .WithOrigins("http://localhost:4200") // frontend URL
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});
// Controllers
builder.Services.AddControllers();

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngularApp");
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
