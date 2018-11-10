using System.Collections.Generic;
using System.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace HighscoreServer {
    public static class SwaggerServiceExtensions {
        public static IServiceCollection AddSwaggerDocumentation (this IServiceCollection services) {
            return services.AddSwaggerGen (opt => {
                opt.SwaggerDoc ("v1", new Info { Title = "Highscore API", Version = "v1" });
            });
        }

        public static IApplicationBuilder UseSwaggerDocumentation (this IApplicationBuilder app) {
            return app
            .UseSwagger()
            .UseSwaggerUI (opt => {
                opt.SwaggerEndpoint ("v1/swagger.json", "Highscore API");
            });

        }
    }
}