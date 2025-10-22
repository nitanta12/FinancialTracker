using FT.Client.Extenstion;
using FT.Client.IdentityConfig.DependencyRegister;
using FT.DependencyResolution;
using Microsoft.AspNetCore.Authentication.Cookies;


var builder = WebApplication.CreateBuilder(args);


var Configuration = builder.Configuration;

// Add services to the container.
builder.Services.ConfigureClientStartupServices(Configuration);
DependencyRegistar.Register(builder.Services, Configuration);
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddAuthentication("ApiKeyAuthorization")
//           .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>("ApiKeyAuthorization", options => { });
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
        .WithOrigins(""));// add client address/URL in origins
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Financial Tracker v1 json");
        c.RoutePrefix = "swagger";
        c.DefaultModelExpandDepth(2);
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);

    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();
