using System;
using System.IO;
using Xunit;

namespace GuessTheNumberNS
{
    public class GuessTheNumberTests
    {
        private const int _numberIsGuessed = 0;
        private const int _outOfRange = 1;
        private const int _numberIsGreater = 2;
        private const int _numberIsMuchGreater = 3;
        private const int _numberIsLess = 4;
        private const int _numberIsMuchLess = 5;
        private const string _invalidValues = "minValue and maxValue are invalid!";

        [Theory]
        [InlineData("..\\..\\..\\ValidValues.xml")]
        public void CheckNewConceivedNumber(string validValuesFile)
        {
            GameLogic gameLogic = new GameLogic(validValuesFile);
            bool isValidConceivedNumber = true;

            for (int i = 0; i < 1000; i++)
            {
                gameLogic.ConceiveNewNumber();

                if (gameLogic.MinValue > gameLogic._conceivedNumber || gameLogic._conceivedNumber >= gameLogic.MaxValue)
                {
                    isValidConceivedNumber = false;
                }
            }

            Assert.True(isValidConceivedNumber);
        }

        [Theory]
        [InlineData(_numberIsGuessed, "..\\..\\..\\ValidValues.xml")]
        public void CheckNumberIsGuessedMessage(int message, string valuesFile)
        {
            GameLogic gameLogic = new GameLogic(valuesFile);
            gameLogic.ConceiveNewNumber();

            Assert.Equal(message, (int)gameLogic.CheckEnteredNumber(gameLogic._conceivedNumber));
        }

        [Theory]
        [InlineData(_outOfRange, "..\\..\\..\\ValidValues.xml")]
        public void CheckOutOfRangeMessage(int message, string valuesFile)
        {
            GameLogic gameLogic = new GameLogic(valuesFile);

            Assert.Equal(message, (int)gameLogic.CheckEnteredNumber(gameLogic.MaxValue));
        }

        [Theory]
        [InlineData(_numberIsGreater, "..\\..\\..\\ValidValues.xml")]
        public void CheckNumberIsGreaterMessage(int message, string valuesFile)
        {
            GameLogic gameLogic = new GameLogic(valuesFile);
            gameLogic.ConceiveNewNumber();

            while (!(gameLogic._conceivedNumber != gameLogic.MaxValue - 1 && gameLogic._conceivedNumber > gameLogic.MaxValue - GameLogic._delimiterValue))
            {
                gameLogic.ConceiveNewNumber();
            }

            Assert.Equal(message, (int)gameLogic.CheckEnteredNumber(gameLogic.MaxValue - 1));
        }

        [Theory]
        [InlineData(_numberIsMuchGreater, "..\\..\\..\\ValidValues.xml")]
        public void CheckNumberIsMuchGreaterMessage(int message, string valuesFile)
        {
            GameLogic gameLogic = new GameLogic(valuesFile);
            gameLogic.ConceiveNewNumber();

            while (gameLogic._conceivedNumber >= gameLogic.MaxValue - GameLogic._delimiterValue)
            {
                gameLogic.ConceiveNewNumber();
            }

            Assert.Equal(message, (int)gameLogic.CheckEnteredNumber(gameLogic.MaxValue - 1));
        }

        [Theory]
        [InlineData(_numberIsLess, "..\\..\\..\\ValidValues.xml")]
        public void CheckNumberIsLessMessage(int message, string valuesFile)
        {
            GameLogic gameLogic = new GameLogic(valuesFile);
            gameLogic.ConceiveNewNumber();

            while (!(gameLogic._conceivedNumber != gameLogic.MinValue && gameLogic._conceivedNumber < gameLogic.MinValue + GameLogic._delimiterValue))
            {
                gameLogic.ConceiveNewNumber();
            }

            Assert.Equal(message, (int)gameLogic.CheckEnteredNumber(gameLogic.MinValue));
        }

        [Theory]
        [InlineData(_numberIsMuchLess, "..\\..\\..\\ValidValues.xml")]
        public void CheckNumberIsMuchLessMessage(int message, string valuesFile)
        {
            GameLogic gameLogic = new GameLogic(valuesFile);
            gameLogic.ConceiveNewNumber();

            while (gameLogic._conceivedNumber <= gameLogic.MinValue + GameLogic._delimiterValue)
            {
                gameLogic.ConceiveNewNumber();
            }

            Assert.Equal(message, (int)gameLogic.CheckEnteredNumber(gameLogic.MinValue));
        }

        [Theory]
        [InlineData(0, 101)]
        public void CheckLoadDefaultValues(int expectedMinValue, int expectedMaxValue)
        {
            FileService fileService = new FileService();

            Assert.True(expectedMinValue == fileService.MinValue && expectedMaxValue == fileService.MaxValue);
        }

        [Theory]
        [InlineData("..\\..\\..\\ValidValues.xml", -1000, 1000)]
        public void CheckValidValuesInFile(string validValuesFile, int expectedMinValue, int expectedMaxValue)
        {
            GameLogic gameLogic = new GameLogic(validValuesFile);

            Assert.True(expectedMinValue == gameLogic.MinValue && expectedMaxValue == gameLogic.MaxValue);
        }

        [Theory]
        [InlineData("..\\..\\..\\MaxInvalidMinValid.xml", -100, 101)]
        public void CheckMaxInvalidMinValidInFile(string maxInvalidMinValidFile, int expectedMinValue, int expectedMaxValue)
        {
            GameLogic gameLogic = new GameLogic(maxInvalidMinValidFile);

            Assert.True(expectedMinValue == gameLogic.MinValue && expectedMaxValue == gameLogic.MaxValue);
        }

        [Theory]
        [InlineData("..\\..\\..\\MinInvalidMaxValid.xml", 0, 2000)]
        public void CheckMinInvalidMaxValidInFile(string minInvalidMaxValidFile, int expectedMinValue, int expectedMaxValue)
        {
            GameLogic gameLogic = new GameLogic(minInvalidMaxValidFile);

            Assert.True(expectedMinValue == gameLogic.MinValue && expectedMaxValue == gameLogic.MaxValue);
        }

        [Theory]
        [InlineData("..\\..\\..\\InvalidValues.xml")]
        public void CheckInvalidValuesInFile(string invalidFile)
        {
            string invalidValuesMessage = "invalidValues";

            try
            {
                new FileService(invalidFile);
            }
            catch (Exception e)
            {
                invalidValuesMessage = e.Message;
            }

            Assert.Equal(_invalidValues, invalidValuesMessage);
        }

        [Theory]
        [InlineData("..\\..\\..\\EqualValues.xml", "..\\..\\..\\MinGreaterThanMax.xml")]
        public void CheckIncorrectValuesInFile(string equalFile, string minGreaterThanMaxFile)
        {
            string equalValuesMessage = "equal", minGreaterThanMaxMessage = "MinGreaterThanMax";

            try
            {
                new FileService(equalFile);
            }
            catch (Exception e)
            {
                equalValuesMessage = e.Message;
            }

            try
            {
                new FileService(minGreaterThanMaxFile);
            }
            catch (Exception e)
            {
                minGreaterThanMaxMessage = e.Message;
            }

            Assert.Equal(equalValuesMessage, minGreaterThanMaxMessage);
        }

        [Theory]
        [InlineData("\\throw")]
        public void CheckWrongFileWay(string wrongFile)
        {
            Assert.Throws<FileNotFoundException>(() => new FileService(wrongFile));
        }

        [Theory]
        [InlineData(null)]
        public void CheckNull(string nullString)
        {
            Assert.Throws<ArgumentNullException>(() => new FileService(nullString));
        }

        [Theory]
        [InlineData("")]
        public void CheckEmptyFileString(string emptyString)
        {
            Assert.Throws<ArgumentException>(() => new FileService(emptyString));
        }
    }
}
