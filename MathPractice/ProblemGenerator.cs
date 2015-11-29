using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathPractice
{
    [Flags]
    enum Constraints    //constraints will override digit counts if incompatible (ignored for unary operators for now)
    {
        None = 0,
        SecondNumberIsEleven = 1,
        BothNumbersAreSame = 2, //right value copied to left
        LastDigitsAddTo10 = 4, //incompatible with both numbers are same
        FirstDigitIsSame = 8,   //incompatible with 1-digit numbers if other constraints used
        LastDigitCycle = 16    //incompatible with everything else
    }
    //todo: for now DayOfWeek operation acts as a mega-constraint that takes over and changes everything and ignores all the above
    //need to make this cleaner and add support for unary operators

    class ProblemGenerator
    {
        public ProblemGenerator()
        {
            LeftDigits = 1;
            RightDigits = 1;
            Operation = MathPractice.Operation.Addition;
            Constraints = MathPractice.Constraints.None;

            digitsSeen = new List<bool[]>();
            for (int i = 0; i < 10; i++)
                digitsSeen.Add(new bool[10]);
            resetDigitsSeen();
        }

        private void resetDigitsSeen()
        {
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    digitsSeen[i][j] = false;
        }

        private bool allDigitsSeen()
        {
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    if (digitsSeen[i][j] == false)
                        return false;
            return true;
        }

        private static void reverseOperands(ref Problem problem)
        {
            uint temp = problem.Left;
            problem.Left = problem.Right;
            problem.Right = temp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisDigits"></param>
        /// <param name="otherDigits"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Something is wrong with the digits you specified.</exception>
        private static uint generateValue(uint thisDigits, uint otherDigits)
        {
            if (thisDigits == 0)
                thisDigits = (uint)(random.Next(1, (int)otherDigits + 1));

            return (uint)random.Next((int)(Math.Pow(10.0, thisDigits - 1)), (int)(Math.Pow(10.0, thisDigits)));
        }

        private void applySequenceConstraint(ref uint left,ref uint right)
        {
            uint leftdigit = left % 10;
            uint rightdigit = right % 10;

            while (digitsSeen[(int)leftdigit][(int)rightdigit])
            {
                leftdigit = (uint)random.Next(0, 10);
                rightdigit = (uint)random.Next(0, 10);
            }

            digitsSeen[(int)leftdigit][(int)rightdigit] = true;
            if (allDigitsSeen())
                resetDigitsSeen();

            left = left / 10 * 10 + leftdigit;
            right = right / 10 * 10 + rightdigit;
        }

        private void applyConstraints(ref Problem problem)
        {
            if (Operation == MathPractice.Operation.DayOfWeek)
            {
                problem.Right = 0;
                problem.Left = (uint)(random.Next(2000, 2100));
                
                return;
            }

            if (this.Constraints.HasFlag(Constraints.LastDigitCycle))
            {
                applySequenceConstraint(ref problem.Left, ref problem.Right);
                return;
            }
            if (this.Constraints.HasFlag(Constraints.SecondNumberIsEleven))
                problem.Right = 11;
            if (this.Constraints.HasFlag(Constraints.BothNumbersAreSame))
                problem.Left = problem.Right;
            if (this.Constraints.HasFlag(Constraints.LastDigitsAddTo10))
            {
                uint rightDigit = problem.Right % 10;
                uint leftDigit = 10 - rightDigit;
                if (leftDigit == 10)
                {
                    problem.Right++;
                    leftDigit = 9;
                }

                problem.Left = (problem.Left / 10) * 10 + leftDigit;
            }
            if (this.Constraints.HasFlag(Constraints.FirstDigitIsSame))
            {
                StringBuilder leftText = new StringBuilder(problem.Left.ToString());
                string rightText = problem.Right.ToString();

                leftText[0] = rightText[0];
                problem.Left = uint.Parse(leftText.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Something is wrong with the digits you specified.</exception>
        public Problem Generate()
        {
            Problem problem = new Problem();
            problem.Left = generateValue(LeftDigits, RightDigits);
            if (!Problem.IsUnary(Operation))
                problem.Right = generateValue(RightDigits, LeftDigits);
            problem.Operation = Operation;
            if (!Problem.IsUnary(Operation))
                applyConstraints(ref problem);

            switch (Operation)
            {
                case MathPractice.Operation.Addition:
                    if (problem.Left.ToString().Length < problem.Right.ToString().Length)
                        reverseOperands(ref problem);
                    break;
                case MathPractice.Operation.Subtraction:
                    if (problem.Left < problem.Right)
                        reverseOperands(ref problem);
                    break;
                case MathPractice.Operation.Multiplication:
                    if (problem.Left.ToString().Length < problem.Right.ToString().Length)
                        reverseOperands(ref problem);
                    break;
            }

            return problem;
        }

        public Operation Operation { get; set; }
        public Constraints Constraints { get; set; }

        public uint LeftDigits { get; set; }        //digits may get swapped to display correctly
        public uint RightDigits { get; set; }       //0 means random up to # of other digits

        private static Random random = new Random();
        private List<bool[]> digitsSeen;
    }
}
