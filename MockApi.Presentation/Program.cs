using FluentValidation;
using FluentValidation.AspNetCore;
using MockApi.Application;
using MockApi.Infrastructure;

namespace MockApi.Presentation;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthorization();

        builder.Services.AddControllers();
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddValidatorsFromAssemblyContaining<MockDataRequestValidator>();
        builder.Services.AddApplicationServices();
        builder.Services.AddInfrastructureServices();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        var app = builder.Build();

        app.UseSwagger(options => { options.RouteTemplate = "api/swagger/{documentName}/swagger.json"; });
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/api/swagger/v1/swagger.json", "RefSystem Backend V1");
            options.RoutePrefix = "api/swagger";
        });


        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}