﻿namespace EmpresaCadastroApp.Api.Configuration
{
    public static class CorsConfig
    {
        public static WebApplicationBuilder AddCorsConfig(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Development", builder => 
                                    builder
                                        .AllowAnyOrigin() 
                                        .AllowAnyMethod() 
                                        .AllowAnyHeader()); 
            });

            return builder;
        }
    }
}
