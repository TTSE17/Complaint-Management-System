using AspectCore.Extensions.DependencyInjection;
using Business_Layer.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new DynamicProxyServiceProviderFactory());

var jwtOptions = builder.Configuration.GetSection("JWT").Get<JwtOptions>()!;
builder.Services.AddSingleton(jwtOptions);

builder.Services.AddControllers();

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
    options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_0;
});

builder.Services
    .AddGeneralDependencyInjection(builder.Configuration)
    .AddRepositoryDependencyInjection()
    .AddIdentityDependencyInjection()
    .AddAuthenticationDependencyInjection(builder.Configuration)
    ;

var app = builder.Build();

//builder.Services.AddScoped<ITraceLogService, TraceLogService>();
//builder.Services.AddScoped<LoggerHelper>();

// app.UseExceptionHandler();
// app.UseHsts();
app.UseHttpsRedirection();
app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapOpenApi();

app.UseSwaggerUI(opt => opt.SwaggerEndpoint("/openapi/v1.json", "Complaint Management System 1.0"));

#region Seed

// await app.SeedData();

#endregion

app.Run();