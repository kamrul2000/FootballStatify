using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
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

// Controllers
builder.Services.AddControllers();

// ? CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev",
        policy => policy
            .WithOrigins("http://localhost:4200") // Angular dev server
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// ? Use CORS
app.UseCors("AllowAngularDev");

app.UseHttpsRedirection();
app.MapControllers();

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
