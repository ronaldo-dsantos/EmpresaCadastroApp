using EmpresaCadastroApp.Api.Configuration;
using EmpresaCadastroApp.Application.Mappings;

var builder = WebApplication.CreateBuilder(args);

builder.AddApiConfig()
       .AddCorsConfig()
       .AddSwaggerConfig()
       .AddDbContextConfig()
       .AddIdentityConfig()
       .AddDIConfig()
       .AddHttpClientsConfig();

builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
