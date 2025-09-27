using CleanArchitecture.API.Common.Exceptions;
using CleanArchitecture.API.Extensions;
using CleanArchitecture.Shared;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration.Get<AppSettings>()
    ?? throw ProgramException.AppsettingNotSetException();

builder.Services.AddSingleton(configuration);
var app = await builder.ConfigureServices(configuration).ConfigurePipelineAsync(configuration);

await app.RunAsync();

// This line for intergration test
public partial class Program { }
