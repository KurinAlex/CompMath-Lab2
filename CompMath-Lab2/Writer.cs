﻿namespace CompMath_Lab2
{
    public class Writer
    {
        public Writer(StreamWriter fileWriter) => _fileWriter = fileWriter;

        public void WriteLine(string line)
        {
            Console.WriteLine(line);
            _fileWriter.WriteLine(line);
        }

        public void WriteDivider()
        {
            WriteLine(_divider);
        }

        private readonly StreamWriter _fileWriter;

        private const int _dividerLength = 79;
        private static readonly string _divider = new('-', _dividerLength);
    }
}
