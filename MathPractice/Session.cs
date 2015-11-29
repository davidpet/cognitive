using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathPractice
{
    class Session
    {
        public Session()
        {
            Generator = new ProblemGenerator();
            Vertical = true;
        }

        private static char getOperationText(Operation operation)
        {
            switch (operation)
            {
                case Operation.Addition:
                    return '+';
                case Operation.Subtraction:
                    return '-';
                case Operation.Multiplication:
                    return 'x';
                case Operation.DayOfWeek:
                    return ' ';
                case Operation.Square:
                    return '²';
                case Operation.Cube:
                    return '^';
                default:
                    return '-';
            }
        }

        private static string getVerticalAnswerPadding(Problem problem)
        {
            StringBuilder output = new StringBuilder();

            uint padding = 0;
            switch (problem.Operation)
            {
                case Operation.Addition:
                    padding = 1;
                    break;
                case Operation.Subtraction:
                    break;
                case Operation.Multiplication:
                   padding = (uint)problem.Right.ToString().Length;
                   break;
                case Operation.DayOfWeek:
                   break;
            }

            for (uint i = 0; i < padding; i++)
                output.Append(' ');

            return output.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">Bad digit specifications.</exception>
        public void PrintProblem()
        {
            currentProblem = Generator.Generate();

            string leftText = currentProblem.Left.ToString();
            string rightText = currentProblem.Right.ToString();
            char opText = getOperationText(currentProblem.Operation);

            Console.WriteLine("[#" + (TotalAnswers + 1).ToString() + "]");
            if (Generator.Operation == Operation.DayOfWeek) //todo: make this suck less
            {
                Console.Write("What is the day of the week for New Year's Day in the year " + currentProblem.Left.ToString() + "?  ");
            }
            else if (Problem.IsUnary(currentProblem.Operation))
            {
                Console.Write(leftText + opText + " = ");
            }
            else if (Vertical)
            {
                string answerPadding = getVerticalAnswerPadding(currentProblem);

                Console.WriteLine("------------------");
                Console.WriteLine("  " + answerPadding + leftText + "\t");
                Console.Write(opText + " ");
                for (int i = rightText.Length; i < leftText.Length; i++)
                    Console.Write(" ");
                Console.WriteLine(answerPadding + rightText + "\t");
                Console.WriteLine("------------------");
                Console.Write("  ");
            }
            else
                Console.Write(leftText + " " + opText + " " + rightText + " = ");
        }

        public void ReadAnswer()
        {
            uint answer;
            string answerText = Console.ReadLine().Trim();
            uint expectedAnswer = currentProblem.Evaluate();
            string expectedAnswerText = (currentProblem.Operation == Operation.DayOfWeek) ? Problem.UintToDay(expectedAnswer).ToString() : expectedAnswer.ToString();
            TotalAnswers++;

            if (currentProblem.Operation == Operation.DayOfWeek)
                answer = Problem.DayToUint(answerText);
            else
                try
                {
                    answer = uint.Parse(answerText);
                }
                catch (Exception)
                {
                    answer = expectedAnswer + 1;
                }

            if (answer != expectedAnswer)
            {
                Console.WriteLine("----------------------");
                Console.WriteLine("!!!WRONG!!! -> " + expectedAnswerText);
                Console.WriteLine("----------------------");
            }
            else
            {
                Console.WriteLine("----------------------");
                Console.WriteLine("CORRECT!!!");
                CorrectAnswers++;
                Console.WriteLine("----------------------");
            }
        }

        public void PrintResults()
        {
            Console.WriteLine("----------------------------------");
            Console.WriteLine(CorrectAnswers.ToString() + "/" + TotalAnswers.ToString() + " (" + 100 * CorrectAnswers / TotalAnswers + "%)");
            Console.WriteLine("----------------------------------");

            Console.ReadKey(true);
        }

        public bool Vertical { get; set; }

        public ProblemGenerator Generator
        {
            get { return generator; }
            private set { generator = value;}
        }

        public uint CorrectAnswers
        {
            get
            {
                return correctAnswers;
            }

            private set 
            {
                correctAnswers = value;
            }
        }

        public uint TotalAnswers
        {
            get
            {
                return totalAnswers;
            }

            private set
            {
                totalAnswers = value;
            }
        }

        private ProblemGenerator generator;
        private Problem currentProblem;

        private uint correctAnswers = 0;
        private uint totalAnswers = 0;
    }
}
