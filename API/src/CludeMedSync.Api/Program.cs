using CludeMedSync.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvcConfiguration();
builder.Services.ResolveDependencies();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfig();
builder.Services.AddAuthConfig(builder.Configuration);

var app = builder.Build();

app.UseGlobalExceptionHandler();

if (app.Environment.IsDevelopment())
{
	app.UseSwaggerConfig();
}

app.UseRequestLocalization();

app.UseHttpsRedirection();

app.UseAuthConfig();

app.UseAuthorization();

app.MapControllers();

app.Run();