using System;
using System.Text;
using System.Text.Json.Serialization;
using ApiWgold.Context;
using ApiWow.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);

var OrigensComAcessoPermitido = "_origensComAcessoPermitido";

builder.Services.AddCors(options =>

    options.AddPolicy(name: OrigensComAcessoPermitido,
        policy =>
        {
            policy.WithOrigins("http://localhost:5000", "https://localhost:3000");
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
            policy.AllowCredentials();
        })
);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Bearer").AddJwtBearer();

//config do jwt
var secretKey = builder.Configuration["JWT: SecretKey"]
    ?? throw new ArgumentNullException("invalid secret key");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    { 
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidAudience = builder.Configuration["JWT: ValidAudience"],
        ValidIssuer = builder.Configuration["JWT: ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secretKey))
    };
});

// config do banco
string postgresConnection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(postgresConnection));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.ConfigureExceptionHandler();
}

app.UseHttpsRedirection();
app.UseCors(OrigensComAcessoPermitido);
app.UseAuthorization();
app.MapControllers();
Stripe.StripeConfiguration.ApiKey = "sk_test_51QAywMF7dsl0WybTTkH6QCusCxcHck4mg4e9cOUrNo5XnqMvsjJKzxe8zIks281kVkFu0UJNmOOW11913gyyPHH200dHTsa9Ss";
app.Run();
