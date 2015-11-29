using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathPractice
{
    enum Operation
    {
        Addition,
        Subtraction,
        Multiplication,

        DayOfWeek,   //for now, this is new year's day of given year (answer is 0-6 relative to sunday)
        Square,
        Cube
    }

    struct Problem
    {
        public uint Left;
        public uint Right;

        public Operation Operation;

        public uint Evaluate()
        {
            uint val = 0;

            switch (Operation)
            {
                case MathPractice.Operation.Addition:
                    val = Left + Right;
                    break;
                case MathPractice.Operation.Subtraction:
                    val = Left - Right;
                    break;
                case MathPractice.Operation.Multiplication:
                    val = Left * Right;
                    break;
                case MathPractice.Operation.DayOfWeek:
                    DateTime date = new DateTime((int)Left, 1, 1, 0, 0, 0, 0);
                    DayOfWeek day = date.DayOfWeek;
                    val = DayToUint(day);
                    break;
                case MathPractice.Operation.Square:
                    val = Left * Left;
                    break;
                case MathPractice.Operation.Cube:
                    val = Left * Left * Left;
                    break;
            }

            return val;
        }

        public static uint DayToUint(DayOfWeek day)
        {
            for (int i = 0; i < days.Length; i++)
                if (days[i] == day)
                    return (uint)i;
            return 0;
        }

        public static uint DayToUint(string day)
        {
            DayOfWeek tmp = DayOfWeek.Sunday;
            DayOfWeek.TryParse(day, true, out tmp);

            return DayToUint(tmp);
        }

        public static DayOfWeek UintToDay(uint val)
        {
            return days[val];
        }

        public static bool IsUnary(Operation operation)
        {
            return operation == Operation.Square || 
                   operation == Operation.Cube || 
                   operation == Operation.DayOfWeek;
        }

        private static DayOfWeek[] days = {DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday};
    }
}
