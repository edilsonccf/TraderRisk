using Microsoft.EntityFrameworkCore;
using TradeRisk.Application.Services;
using TradeRisk.Domain.Rules;
using TradeRisk.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=trades.db"));

builder.Services.AddSingleton<IRiskRule, LowRiskRule>();
builder.Services.AddSingleton<IRiskRule, MediumRiskRule>();
builder.Services.AddSingleton<IRiskRule, HighRiskRule>();
builder.Services.AddScoped<ITradeService, TradeService>();

var app = builder.Build();


app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TradeRisk API V1");    
    c.RoutePrefix = string.Empty;
});


app.UseRouting();
app.UseAuthorization();
app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.Run();