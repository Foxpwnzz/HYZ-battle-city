using Microsoft.Extensions.DependencyInjection;
using XYZ_Tanks.Engine;
using XYZ_Tanks.Input;
using XYZ_Tanks.Map;
using XYZ_Tanks.Rendering;
using XYZ_Tanks.Units;

namespace XYZ_Tanks;
public static class DependencyInjection
{
    public static IServiceCollection AddGame(this IServiceCollection services) => services
        .AddSingleton<IInputReader, ConsoleInputReader>()
        .AddSingleton<IRenderer, ConsoleRenderer>()
        .AddSingleton<ILevelMapManager, LevelMapManager>()
        .AddSingleton<ShowTextState>()
        .AddSingleton<Game>()
        .AddUnits();

    public static IServiceCollection AddUnits(this IServiceCollection services) => services
        .AddTransient<EnemyTank>()
        .AddTransient<Tank>()
        .AddTransient<Projectile>();
}