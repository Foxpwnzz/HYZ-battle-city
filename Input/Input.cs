using System;

namespace XYZ_Tanks.Input
{
    // Перечисление для возможных действий пользователя
    public enum InputAction
    {
        Up,
        Down,
        Left,
        Right,
        Fire,
        Exit
    }

    // Аргументы событий ввода
    public class InputEventArgs : EventArgs
    {
        public InputAction InputAction { get; }

        public InputEventArgs(InputAction inputAction)
        {
            InputAction = inputAction;
        }
    }

    // Чтение пользовательского ввода с консоли
    public class ConsoleInputReader : IInputReader
    {
        // Событие, которое вызывается при вводе пользователем команды
        public event EventHandler<InputEventArgs> InputActionCalled = null!;

        public void Update()
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(intercept: true).Key;
                InputAction? action = key switch
                {
                    ConsoleKey.W or ConsoleKey.UpArrow => InputAction.Up,
                    ConsoleKey.S or ConsoleKey.DownArrow => InputAction.Down,
                    ConsoleKey.A or ConsoleKey.LeftArrow => InputAction.Left,
                    ConsoleKey.D or ConsoleKey.RightArrow => InputAction.Right,
                    ConsoleKey.Spacebar => InputAction.Fire,
                    ConsoleKey.Escape => InputAction.Exit,
                    _ => null
                };

                if (action.HasValue)
                {
                    InputActionCalled?.Invoke(this, new(action.Value));
                }
            }
        }
    }
}