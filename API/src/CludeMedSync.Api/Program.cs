using CludeMedSync.Api.Configuration;
using CludeMedSync.Data.Extensions;
using Dapper;

SqlMapper.AddTypeHandler(new EnumStatusConsultaTypeHandler());

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerConfig();
builder.Services.AddMvcConfiguration();
builder.Services.AddAuthConfig(builder.Configuration);
builder.Services.AddHealthCheckConfig(builder.Configuration);
builder.Services.ResolveDependencies();

builder.Services.AddCors(options =>
{
	options.AddPolicy("CorsPolicy", policy =>
	{
		policy.AllowAnyOrigin()
			  .AllowAnyHeader()
			  .AllowAnyMethod();
	});
});

var app = builder.Build();

app.UseGlobalExceptionHandler();

if (app.Environment.IsDevelopment())
{
	app.UseSwaggerConfig();
}

app.UseHttpsRedirection();
app.UseRequestLocalization();
app.UseRouting();
app.UseHealthCheckConfig();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("CorsPolicy");
app.MapControllers();



app.Run();
