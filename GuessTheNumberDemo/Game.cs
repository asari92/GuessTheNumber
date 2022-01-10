using System;
using System.Collections.Generic;
using System.IO;

namespace GuessTheNumberNS
{
    public class Game
    {
        private const string _incorrectInput = "Incorrect input, try again.";
        private const string _defaultValues = "\nThe default values will be used.\n";
        private Dictionary<int, string> _messages = new Dictionary<int, string>
        {
            {0, "You guessed it!\nIf you want to play again press 'y'\n"},
            {1, "Your number is out of range!"},
            {2, "Your number is greater than the conceived:)"},
            {3, "Your number is much greater than the conceived:("},
            {4, "Your number is less than the conceived(:"},
            {5, "Your number is much less than the conceived):"}
        };
        private GameLogic _gameLogic;

        public Game(string file)
        {
            try
            {
                _gameLogic = new GameLogic(file);
                Console.Write(_gameLogic.MinValueInvalidMessage);
                Console.Write(_gameLogic.MaxValueInvalidMessage);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message + _defaultValues);
                _gameLogic = new GameLogic();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + _defaultValues);
                _gameLogic = new GameLogic();
            }

            do
            {
                _gameLogic.ConceiveNewNumber();
            }
            while (Run());
        }

        private bool Run()
        {
            bool runAgain = true;

            while (true)
            {
                Console.Write($"Enter a number between {_gameLogic.MinValue} and {_gameLogic.MaxValue - 1}: ");

                if (!Int32.TryParse(Console.ReadLine(), out int enteredNumber))
                {
                    Console.WriteLine(_incorrectInput);
                    continue;
                }

                Message messageToPlayer = _gameLogic.CheckEnteredNumber(enteredNumber);
                Console.WriteLine(_messages[(int)messageToPlayer]);

                if (messageToPlayer == Message.NumberIsGuessed)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);

                    if (key.Key != ConsoleKey.Y)
                    {
                        runAgain = false;
                    }

                    break;
                }
            }

            return runAgain;
        }
    }
}
