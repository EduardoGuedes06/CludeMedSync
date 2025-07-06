using CludeMedSync.Api.Configuration;
using CludeMedSync.Data.Extensions;
using Dapper;
SqlMapper.AddTypeHandler(new EnumStatusConsultaTypeHandler());

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwaggerConfig();
builder.Services.AddMvcConfiguration();
builder.Services.AddAuthConfig(builder.Configuration);
builder.Services.ResolveDependencies();

var app = builder.Build();

app.UseGlobalExceptionHandler();

if (app.Environment.IsDevelopment())
{
	app.UseSwaggerConfig();
}

app.UseHttpsRedirection();
app.UseRequestLocalization();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
