using DataBase;
using DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Service.Extensions;
using Service.Middleware;
using Service.Service;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" }); });

builder.Services.AddDbContext<DataBaseContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:5173")
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.Services.AddScoped<IGameEngine, GameEngine>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IETagGenerator, ETagGenerator>();

builder.Services.AddSingleton(sp =>
{
    var config = new GameConfiguration
    {
        DefaultBoardSize = ConfigExtensions.GetEnvInt("GAME_BOARD_SIZE", 3),
        WinCondition = ConfigExtensions.GetEnvInt("WIN_CONDITION", 3),
        RandomMoveChancePercent = ConfigExtensions.GetEnvInt("RANDOM_MOVE_CHANCE_PERCENT", 10),
        RandomMoveInterval = ConfigExtensions.GetEnvInt("RANDOM_MOVE_INTERVAL", 3)
    };

    ConfigExtensions.ValidateConfiguration(config);
    return config;
});
var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var db = services.GetRequiredService<DataBaseContext>();
    db.Database.Migrate();

    if (!db.GameConfigurations.Any())
    {
        var config = services.GetRequiredService<GameConfiguration>();
        db.GameConfigurations.Add(config);
        db.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");
app.UseAuthorization();
app.MapControllers();

app.Run();