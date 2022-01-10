using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GuessTheNumberTests")]

namespace GuessTheNumberNS
{
    public class GameLogic
    {
        internal const int _delimiterValue = 100;
        public int MinValue { private set; get; }
        public int MaxValue { private set; get; }
        public string MinValueInvalidMessage { private set; get; }
        public string MaxValueInvalidMessage { private set; get; }
        internal int _conceivedNumber;
        public GameLogic()
        {
            FileService fileService = new FileService();
            MinValue = fileService.MinValue;
            MaxValue = fileService.MaxValue;
        }
        public GameLogic(string file)
        {
            FileService fileService = new FileService(file);
            MinValue = fileService.MinValue;
            MaxValue = fileService.MaxValue;
            MinValueInvalidMessage = fileService.MinValueInvalidMessage;
            MaxValueInvalidMessage = fileService.MaxValueInvalidMessage;
        }

        public void ConceiveNewNumber()
        {
            _conceivedNumber = new Random().Next(MinValue, MaxValue);
        }

        public Message CheckEnteredNumber(int enteredNumber)
        {
            if (enteredNumber >= MaxValue || enteredNumber < MinValue)
            {
                return Message.OutOfRange;
            }
            else if (enteredNumber - _delimiterValue > _conceivedNumber)
            {
                return Message.NumberIsMuchGreater;
            }
            else if (enteredNumber > _conceivedNumber)
            {
                return Message.NumberIsGreater;
            }
            else if (enteredNumber + _delimiterValue < _conceivedNumber)
            {
                return Message.NumberIsMuchLess;
            }
            else if (enteredNumber < _conceivedNumber)
            {
                return Message.NumberIsLess;
            }
            else
            {
                return Message.NumberIsGuessed;
            }
        }
    }
}
