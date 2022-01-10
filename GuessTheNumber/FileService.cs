using System;
using System.Runtime.CompilerServices;
using System.Xml;

[assembly: InternalsVisibleTo("GuessTheNumberTests")]

namespace GuessTheNumberNS
{
    public class FileService
    {
        private const string _invalidValues = "minValue and maxValue are invalid!";
        private const string _incorrectValues = "minValue can't be equal or greater than maxValue!";
        private const string _xNodeNameMinValue = "minValue";
        private const string _xNodeNameMaxValue = "maxValue";
        private const int _defMinValue = 0;
        private const int _defMaxValue = 101;
        internal int MinValue => _minValue;
        internal int MaxValue => _maxValue;
        internal string MinValueInvalidMessage => _minValueInvalidMessage;
        internal string MaxValueInvalidMessage => _maxValueInvalidMessage;
        private int _minValue;
        private int _maxValue;
        private string _minValueInvalidMessage = "";
        private string _maxValueInvalidMessage = "";

        internal FileService()
        {
            _maxValue = _defMaxValue;
            _minValue = _defMinValue;
        }
        internal FileService(string file)
        {
            XmlDocument xDoc = new XmlDocument();

            xDoc.Load(file);
            
            foreach (XmlNode xnode in xDoc.DocumentElement)
            {
                if(xnode.Name == _xNodeNameMinValue)
                {
                    CheckNodeValue(xnode, _xNodeNameMinValue, ref _minValueInvalidMessage, ref _minValue);
                }
                else
                {
                    CheckNodeValue(xnode, _xNodeNameMaxValue, ref _maxValueInvalidMessage, ref _maxValue);
                }
            }

            if (!string.IsNullOrEmpty(_minValueInvalidMessage) && !string.IsNullOrEmpty(_maxValueInvalidMessage))
            {
                throw new Exception(_invalidValues);
            }

            if (_minValue >= _maxValue)
            {
                throw new Exception(_incorrectValues);
            }
        }

        private static void CheckNodeValue(XmlNode xnode, string valueForCheck, ref string valueMessage, ref int outputValue)
        {
            if (!Int32.TryParse(xnode.InnerText, out outputValue))
            {
                valueMessage = $"Wrong {valueForCheck}!\nThe default {valueForCheck} will be used.\n";
                outputValue = (valueForCheck == _xNodeNameMaxValue) ? _defMaxValue : _defMinValue;
            }
        }
    }
}
