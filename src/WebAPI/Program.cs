using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Identity;
using URLShortener.Application.DTOs;
using URLShortener.Application.Url.Commands.Create;
using URLShortener.Application.Url.Commands.Delete;
using URLShortener.Application.Url.Queries.Get;
using URLShortener.Web.Extentions;
using URLShortener.Web.Middlewares;
using URLShortener.Web.Validators;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load("./example.env");

builder.Services.AddControllers();

builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddInfrastructureDbContext();
builder.Services.AddInfrastructureRedisCacheServices();

builder.Services.AddScoped<IPasswordHasher<string>, PasswordHasher<string>>();
builder.Services.AddScoped<IRequestHandler<CreateUrlCommand, CreateDto>, CreateUrlCommandHandler>();
builder.Services.AddScoped<IRequestHandler<DeleteUrlCommand>, DeleteUrlCommandHandler>();
builder.Services.AddScoped<IRequestHandler<GetUrlQuery, GetUrlDto>, GetUrlQueryHandler>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateUrlCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<DeleteUrlCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<GetUrlQueryValidator>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorMiddleware>();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();