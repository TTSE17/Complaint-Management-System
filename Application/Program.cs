using Application.Seeds;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

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

// app.UseExceptionHandler();
// app.UseHsts();
app.UseHttpsRedirection();
app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.MapOpenApi();

app.UseSwaggerUI(opt => opt.SwaggerEndpoint("/openapi/v1.json", "Demo"));

#region Seed

// await app.SeedData();

#endregion

app.Run();