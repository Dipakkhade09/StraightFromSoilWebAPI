using KhadeFarm_Web_API.CustomMiddleware;
using KhadeFarm_Web_API.DBContext;
using KhadeFarm_Web_API.DBContexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var ConnectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(ConnectionString));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowKhadeFarmUser", policy =>
    {
        policy
             .WithOrigins("https://localhost:4200") // 👈 your Angular app
             .AllowAnyHeader()
             .AllowAnyMethod()
             .AllowCredentials(); // 👈 REQUIRED for cookies
    });
});

var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context => {

                // 1) Authorization header (default) will be used if present
                if (context.Request.Headers.ContainsKey("Authorization")) return Task.CompletedTask;

                // 2) fallback: read HttpOnly cookie named "jwt"
                if (context.Request.Cookies.TryGetValue("jwt", out var token) && !string.IsNullOrEmpty(token))
                {
                    Console.WriteLine("JWT from cookie: " + token); // 👈 DEBUG
                    context.Token = token;
                }
                else
                {
                    Console.WriteLine("JWT cookie NOT FOUND"); // 👈 DEBUG
                }
                    return Task.CompletedTask;
            }
        };

    });

builder.Services.AddAuthorization();

/*===================================================================================================================*/

var app = builder.Build();

app.UseCors("AllowKhadeFarmUser");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // Production-safe error handling
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// 2. HTTPS redirections
app.UseHttpsRedirection();


//Custome Middleware for logging request and response
app.UseMiddleware<RequestLoggingMiddleware>();

// 3. Authentication FIRST
app.UseAuthentication();

// 4. Authorization SECOND
app.UseAuthorization();

//Rate Limiting middleware
//app.UseRateLimiter();

// 5. Routing
// 6. Endpoints/Controllers execution LAST
app.MapControllers();
app.MapFallbackToFile("/index.html");
app.Run();

