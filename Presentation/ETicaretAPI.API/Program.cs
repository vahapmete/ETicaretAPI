 using ETicaretAPI.Application.Validators.Products;
using ETicaretAPI.Infrastructure.Filters;
using ETicaretAPI.Infrastructure;

using ETicaretAPI.Persistence;
using FluentValidation.AspNetCore;
using ETicaretAPI.Infrastructure.Sevices.Storage.Local;
using ETicaretAPI.Infrastructure.Sevices.Storage.Azure;
using ETicaretAPI.Application;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();

builder.Services.AddStorage<AzureStorage>();

builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.WithOrigins("http://localhost:4200", "https://localhost:4200").AllowAnyHeader().AllowAnyMethod()));

builder.Services
    .AddControllers(options=>options.Filters.Add<ValidationFilter>())
    .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>())
    .ConfigureApiBehaviorOptions(options=>options.SuppressModelStateInvalidFilter=true);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Admin", options =>
{
    options.TokenValidationParameters = new() 
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        
        ValidAudience= builder.Configuration["TokenParameters:Audience"],
        ValidIssuer = builder.Configuration["TokenParameters:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenParameters:SecurityKey"]))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

 app.Run();
