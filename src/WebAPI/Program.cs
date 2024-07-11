using Application.Commands;
using Application.Commands.Handlers;
using Application.DTOs;
using Application.Validators;
using DotNetEnv;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load("./example.env");

builder.Services.AddControllers();

builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddInfrastructure();
builder.Services.AddScoped<IPasswordHasher<string>, PasswordHasher<string>>();
builder.Services.AddScoped<IRequestHandler<CreateUrlCommand, CreateDataDto>, CreateUrlCommandHandrel>();
builder.Services.AddScoped<IRequestHandler<DeleteUrlCommand>, DeleteUrlCommandHandler>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateUrlCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<DeleteUrlCommandValidator>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<RedirectMiddleware>();
app.UseMiddleware<ErrorMiddleware>();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();