using EmpresaCadastroApp.Application.Interfaces;
using EmpresaCadastroApp.Infrastructure.Services;

namespace EmpresaCadastroApp.Api.Configuration
{
    public static class HttpClientsConfig
    {
        public static WebApplicationBuilder AddHttpClientsConfig(this WebApplicationBuilder builder)
        {
            builder.Services.AddHttpClient<IReceitaWsService, ReceitaWsService>(client =>
            {
                client.BaseAddress = new Uri("https://www.receitaws.com.br/v1/");
            });

            return builder;
        }
    }
}
