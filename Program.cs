using XYZ_Tanks.Engine;
using Microsoft.Extensions.DependencyInjection;

namespace XYZ_Tanks
{
    public class Program
    {
        private static async Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddGame()
                .BuildServiceProvider();


            var game = serviceProvider.GetRequiredService<Game>();
            await game.RunAsync();
        }
    }
}