namespace XYZ_Tanks.Rendering;
using System;
using System.Threading.Tasks;

internal class ShowTextState
{
    private readonly float _displayDuration;

    public ShowTextState(float displayDuration = 2f)
    {
        _displayDuration = displayDuration;
    }

    public static void RenderGameOverScreen(bool win)
    {
        Console.Clear();
        if (win)
        {
            Console.WriteLine("You win!");
        }
        else
        {
            Console.WriteLine("Game Over!");
        }
    }

    // Метод для отображения уровня по центру экрана
    public void RenderLevel(int level)
    {
        Console.Clear();
        string levelText = $"Level {level}";

        // Рассчитываем позицию для центрирования текста
        int centerX = (Console.WindowWidth - levelText.Length) / 2;
        int centerY = Console.WindowHeight / 2;

        // Устанавливаем курсор в центр экрана
        Console.SetCursorPosition(centerX, centerY);
        Console.WriteLine(levelText);

        // Пауза для отображения текста
        Task.Delay((int)(_displayDuration * 1000)).Wait();
    }
}