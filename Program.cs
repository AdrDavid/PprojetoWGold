using System;
using System.Text.Json.Serialization;
using ApiWgold.Context;
using ApiWow.Extensions;
using Microsoft.EntityFrameworkCore;
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
//app.UseAuthentication();

app.UseCors(OrigensComAcessoPermitido);
app.UseAuthorization();

app.MapControllers();
Stripe.StripeConfiguration.ApiKey = "sk_test_51QAywMF7dsl0WybTTkH6QCusCxcHck4mg4e9cOUrNo5XnqMvsjJKzxe8zIks281kVkFu0UJNmOOW11913gyyPHH200dHTsa9Ss";
app.Run();
