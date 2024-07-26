using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Identity;
using URLShortener.Application.Url.Behaviour;
using URLShortener.Application.Url.Commands.Create;
using URLShortener.Web.Extentions;
using URLShortener.Web.Middlewares;
using URLShortener.Web.Validators;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load("./example.env");

builder.Services.AddControllers();

builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssembly(typeof(CreateUrlCommandHandler).Assembly);
});

builder.Services.AddInfrastructureDbContext();
builder.Services.AddInfrastructureRedisCacheServices();

builder.Services.AddScoped<IPasswordHasher<string>, PasswordHasher<string>>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateUrlCommandValidator>();

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

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