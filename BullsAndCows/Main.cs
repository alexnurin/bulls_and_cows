using System;
using System.Net.Mime;

namespace BullsAndCows
{
    class Program
    {
        static void EndWithComment(string text)
        {
            Console.WriteLine(text);
            Environment.Exit(0);
        }

        static string GetUniqDigit(ref bool[] used, int currentLength)
        {
            if (currentLength > 9)
            {
                EndWithComment("Запрошено число больше 10");
            }

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

        static string GenerateNumber(int length)
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
        static bool digitalCheck(string input, string number)
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

        static int NextTurn(string number)
        {
            string userNumber;
            Console.WriteLine($"Введите число из {number.Length} цифр, а я скажу сколько в нём быков и коров. " +
                                                $"Введите \"exit\" чтобы выйти");
            do
            {
                userNumber = Console.ReadLine();
            } while (!(userNumber == "exit" || digitalCheck(userNumber, number)));

            if (userNumber == "exit")
            {
                EndWithComment("Завершение работы программы");
            }
            if (userNumber == number)
            {
                Console.WriteLine($"\nВы верно угадали число {number}!\nПоиграем ещё?\n");
                return 1;
            }

            int cows = 0, bulls = 0;
            for (int i = 0; i < number.Length; i++)
            {
                if (number[i] == userNumber[i])
                {
                    bulls++;
                }

                for (int j = 0; j < number.Length; j++)
                {
                    if (i != j && number[i] == userNumber[j])
                    {
                        cows++;
                    }
                }
            }

            Console.WriteLine($"{bulls} быков и {cows} коров. Попробуйте снова!");
            return 0;
        }

        static int SafeInputInt()
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

        static void Main(string[] args)
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