using Application.DTOs;
using Application.Url.Commands.Create;
using Application.Url.Commands.Delete;
using Application.Url.Queries.Get;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Web.Extentions;
using Web.Middlewares;
using Web.Validators;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load("./example.env");

builder.Services.AddControllers();

builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddInfrastructure();
builder.Services.AddInfrastructureServices();

builder.Services.AddScoped<IPasswordHasher<string>, PasswordHasher<string>>();
builder.Services.AddScoped<IRequestHandler<CreateUrlCommand, CreateDto>, CreateUrlCommandHandler>();
builder.Services.AddScoped<IRequestHandler<DeleteUrlCommand>, DeleteUrlCommandHandler>();
builder.Services.AddScoped<IRequestHandler<GetUrlQuery, GetUrlDto>, GetUrlQueryHandler>();

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