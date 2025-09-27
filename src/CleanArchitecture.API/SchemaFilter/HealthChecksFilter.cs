using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CleanArchitecture.API.SchemaFilter
{
    public class HealthChecksFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            // Add the /healthz endpoint to Swagger
            swaggerDoc.Paths.Add("/synthetic-check", new OpenApiPathItem
            {
                Operations =
            {
                [OperationType.Get] = new OpenApiOperation
                {
                    Summary = "Synthetic health Check",
                    Description = "Displays the application's health status.",
                    Responses = new OpenApiResponses
                    {
                        ["200"] = new OpenApiResponse { Description = "Healthy" },
                    }
                }
            }
            });

            // Add the /health endpoint to Swagger
            swaggerDoc.Paths.Add("/health", new OpenApiPathItem
            {
                Operations =
            {
                [OperationType.Get] = new OpenApiOperation
                {
                    Summary = "Health check Endpoint",
                    Description = "Returns a plain text health status.",
                    Responses = new OpenApiResponses
                    {
                        ["200"] = new OpenApiResponse { Description = "Health" }
                    }
                }
            }
            });
        }
    }
}
