using System;
using System.Linq;
using XYZ_Tanks.Map;

namespace XYZ_Tanks.Extensions
{
    public static class CharExtensions
    {
        // Преобразование символов в объекты типа StaticObject
        public static StaticObject ToStaticObject(this char character)
        {
            return character switch
            {
                'W' => StaticObject.Wall,          // Стена
                'R' => StaticObject.River,         // Вода (река)
                ' ' => StaticObject.Empty,         // Пустое место
                _ => throw new ArgumentOutOfRangeException(nameof(character), $"Unknown map character: {character}")
            };
        }
    }

    public static class RandomExtensions
    {
        // Возвращает случайный элемент из коллекции
        public static T NextElement<T>(this Random random, T[] array)
        {
            return array[random.Next(array.Length)];
        }

        // Возвращает случайное значение из перечисления
        public static T NextEnum<T>(this Random random) where T : Enum
        {
            var values = Enum.GetValues(typeof(T)).Cast<T>().ToArray();
            return random.NextElement(values);
        }
    }
}