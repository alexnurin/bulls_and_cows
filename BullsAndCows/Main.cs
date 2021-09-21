using System;
using System.Net.Mime;

namespace BullsAndCows
{
    class Program
    {
        /// <summary>
        /// Выводит сообщение и завершает выполнение программы
        /// </summary>
        /// <param name="text">Выводимый пользователю текст</param>
        private static void EndWithComment(string text)
        {
            Console.WriteLine(text);
            Environment.Exit(0);
        }

        /// <summary>
        /// Генерирует очередную цифру для числа, чтобы в нём не было повторяющихся
        /// </summary>
        /// <param name="used">Массив уже использованных цифр, от 0 до 9</param>
        /// <param name="currentLength">Текущая длина числа (влияет на ведущий ноль)</param>
        /// <returns>Очередная цифра для числа в виде строки длины 1</returns>
        private static string GetUniqDigit(ref bool[] used, int currentLength)
        {
            Random randomizer = new Random();
            int minNumber = currentLength > 0 ? 0 : 1;
            ulong newNumber;
            do
            {
                newNumber = Convert.ToUInt64(randomizer.Next(minNumber, 10));
            } while (used[newNumber]);

            used[newNumber] = true;
            return newNumber.ToString();
        }

        /// <summary>
        /// Генерирует число без повторяющихся цифр заданной длины
        /// </summary>
        /// <param name="length">Длина требуемого числа</param>
        /// <returns>Возвращает требуемое число в виде строки</returns>
        private static string GenerateNumber(int length)
        {
            string result = "";
            bool[] used = new bool[10];
            for (int i = 0; i < 10; i++)
            {
                used[i] = false;
            }

            for (int i = 0; i < length; i++)
            {
                result = result + GetUniqDigit(ref used, i);
            }

            return result;
        }

        /// <summary>
        /// Проверка формат введённого числа - его длину и состав только из цифр
        /// </summary>
        /// <param name="input">Введённое число</param>
        /// <param name="number">Загаданное число</param>
        /// <returns>Возвращает true если число соответствует, иначе возвращает false
        /// и выводит пользователю причину</returns>
        private static bool DigitalCheck(string input, string number)
        {
            if (input.Length != number.Length)
            {
                Console.WriteLine($"Проверьте корректность данных. Необходимо ввести число длины {number.Length}");
                return false;
            }

            for (int i = 0; i < input.Length; i++)
            {
                if (!(input[i] >= '0' && input[i] <= '9'))
                {
                    Console.WriteLine("Проверьте корректность данных. Число не должно содержать посторонних символов");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Вычисляет число коров в числе (аргументы симметричны)
        /// </summary>
        /// <param name="number1">Загаданное число</param>
        /// <param name="number2">Введённое пользователем число</param>
        /// <returns>Число коров в числе</returns>
        private static int CountCows(string number1, string number2)
        {
            int cows = 0;
            for (int i = 0; i < number1.Length; i++)
            {
                for (int j = 0; j < number2.Length; j++)
                {
                    if (i != j && number1[i] == number2[j])
                    {
                        cows++;
                    }
                }
            }

            return cows;
        }

        /// <summary>
        /// Вычисляет число быков в числе (аргументы симметричны)
        /// </summary>
        /// <param name="number1">Загаданное число</param>
        /// <param name="number2">Введённое пользователем число</param>
        /// <returns>Число быков в числе</returns>
        private static int CountBulls(string number1, string number2)
        {
            int bulls = 0;
            for (int i = 0; i < number1.Length; i++)
            {
                if (number1[i] == number2[i])
                {
                    bulls++;
                }
            }

            return bulls;
        }

        /// <summary>
        /// Принимает новую попытку от пользователя либо команду завершения выполнения
        /// </summary>
        /// <param name="number">Загаданное число</param>
        /// <returns>Корректное введённое число, либо завершение программы</returns>
        private static string NewGuess(string number)
        {
            string userNumber;
            Console.WriteLine($"Введите число из {number.Length} цифр, а я скажу сколько в нём быков и коров. " +
                              "Введите \"exit\" чтобы выйти");
            do
            {
                userNumber = Console.ReadLine();
            } while (!(userNumber == "exit" || DigitalCheck(userNumber, number)));

            if (userNumber == "exit")
            {
                EndWithComment("Завершение работы программы");
            }

            return userNumber;
        }

        /// <summary>
        /// Запускает новый ход пользователя
        /// </summary>
        /// <param name="number">Загаданное число</param>
        /// <returns>0 если число ещё не угадано, 1 если угадано</returns>
        private static int NextTurn(string number)
        {
            string userNumber = NewGuess(number);

            if (userNumber == number)
            {
                Console.WriteLine($"\nВы верно угадали число {number}!\nПоиграем ещё?\n");
                return 1;
            }

            int cows = CountCows(number, userNumber),
                bulls = CountBulls(number, userNumber);

            Console.WriteLine($"{bulls} быков и {cows} коров. Попробуйте снова!");
            return 0;
        }

        /// <summary>
        /// Корректный ввод новой длины для загадываемого числа
        /// </summary>
        /// <returns></returns>
        /// <exception cref="FormatException"></exception>
        private static int SafeInputInt()
        {
            int number = -1;
            Console.WriteLine("Введите число от 1 до 10 - длину загадываемого числа. \n" +
                              "Введите 0, чтобы выйти.");
            do
            {
                try
                {
                    number = Convert.ToInt32(Console.ReadLine());
                    if (number > 10 || number < 0)
                        throw new FormatException();
                }
                catch (FormatException)
                {
                    Console.WriteLine("Пожалуйста, вводите число от 1 до 10.");
                }
            } while (number < 0 || number > 10);

            return number;
        }

        /// <summary>
        /// Точка входа в программу. Запускает новую игру, контролирует перезапуск
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            int lenght = SafeInputInt();
            while (lenght != 0)
            {
                string randomNumber = GenerateNumber(lenght);
                // Console.WriteLine(randomNumber);
                int gameResult;
                do
                {
                    gameResult = NextTurn(randomNumber);
                } while (gameResult != 1);

                lenght = SafeInputInt();
            }

            EndWithComment("Завершение работы программы");
        }
    }
}