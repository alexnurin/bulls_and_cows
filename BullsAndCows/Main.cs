using System;
using System.Net.Mime;

namespace BullsAndCows
{
    class Program
    {
        static void EndWithException(string text)
        {
            Console.WriteLine(text);
            Environment.Exit(0);
        }

        static string GetUniqDigit(ref bool[] used, int currentLength)
        {
            if (currentLength > 9)
            {
                EndWithException("Запрошено число больше 10");
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

        static int GameTurn(string number)
        {
            string userNumber;
            do
            {
                Console.WriteLine($"Введите число из {number.Length} цифр, а я скажу сколько в нём быков и коров");
                userNumber = Console.ReadLine();
            } while (userNumber.Length != number.Length);

            if (userNumber == number)
            {
                Console.WriteLine($"Вы верно угадали число {number}!");
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

        static void Main(string[] args)
        {
            Console.WriteLine("Введите одно число - длину");
            int lenght = Convert.ToInt32(Console.ReadLine());
            string randomNumber = GenerateNumber(lenght);
            Console.WriteLine(randomNumber);
            int gameResult;
            do
            {
                gameResult = GameTurn(randomNumber);
            } while (gameResult != 1);
        }
    }
}