using Backend.Data;
using Backend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Dodanie DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Rejestracja LdapSyncService jako Scoped
builder.Services.AddScoped<LdapSyncService>();

// Dodanie pozostałych usług
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Wywołanie LDAP Sync po uruchomieniu aplikacji
using (var scope = app.Services.CreateScope())
{
    var ldapSyncService = scope.ServiceProvider.GetRequiredService<LdapSyncService>();
    var ldapConfig = builder.Configuration.GetSection("LDAP");
    var username = ldapConfig["Username"];
    var password = ldapConfig["Password"];
    
    try
    {
        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            Console.WriteLine("Starting LDAP Sync...");
            ldapSyncService.SyncUsersFromLdap(username, password);
        }
        else
        {
            Console.WriteLine("LDAP credentials are missing.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error during LDAP sync: {ex.Message}");
    }
}

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware
app.UseCors();
app.MapControllers();
app.Run();
