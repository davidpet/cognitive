using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLib;

namespace MathPractice
{
    class Configuration
    {
        public Configuration(Operation operation, Constraints constraints = MathPractice.Constraints.None, Nullable<uint> leftDigits = null, Nullable<uint> rightDigits = null, bool skipVerticalPrompt = false, uint? numberOfQuestions = null)
        {
            Operation = operation;
            Constraints = constraints;
            LeftDigits = leftDigits;
            RightDigits = rightDigits;
            SkipVerticalPrompt = skipVerticalPrompt;
            NumberOfQuestions = numberOfQuestions;
        }

        public Operation Operation;
        public Constraints Constraints;

        public uint? LeftDigits;
        public uint? RightDigits;

        public bool SkipVerticalPrompt;
        public uint? NumberOfQuestions;
    }

    class Program
    {
        static void runSession(Session session, uint questions)
        {
            for (uint i = 0; i < questions; i++)
            {
                session.PrintProblem();
                session.ReadAnswer();
            }

            session.PrintResults();
        }

        static void processChoice(object menu, object choice)
        {
            Configuration config = (Configuration)choice;

            Session session = new Session();
            session.Generator.Operation = config.Operation;
            session.Generator.Constraints = config.Constraints;
            session.Generator.LeftDigits = (config.LeftDigits == null) ? Menu.SolicitValue("Left Digits") : (uint)config.LeftDigits;
            session.Generator.RightDigits = (config.RightDigits == null) ? Menu.SolicitValue("Right Digits") : (uint)config.RightDigits;
            session.Vertical = config.SkipVerticalPrompt ? true : Menu.SolicitFlag("Vertical");

            runSession(session, config.NumberOfQuestions == null ? Menu.SolicitValue("Questions") : (uint)config.NumberOfQuestions);
        }

        static void Main(string[] args)
        {
            Menu mainMenu = new Menu("Test Type");

            mainMenu.AddChoice("Multiplication Table",                                         new Configuration(Operation.Multiplication, Constraints.LastDigitCycle, 1, 1, false, 100));
            mainMenu.AddChoice("Addition Table",                                               new Configuration(Operation.Addition, Constraints.LastDigitCycle, 1, 1, false, 100));
            mainMenu.AddChoice("Subtraction Table",                                            new Configuration(Operation.Subtraction, Constraints.LastDigitCycle, 1, 1, false, 100));
            
            mainMenu.AddChoice("Addition",                                                      new Configuration(Operation.Addition));
            mainMenu.AddChoice("Subtraction",                                                   new Configuration(Operation.Subtraction));
            mainMenu.AddChoice("Multiplication",                                                new Configuration(Operation.Multiplication));
            
            mainMenu.AddChoice("Multiplication by 11",                                          new Configuration(Operation.Multiplication, Constraints.SecondNumberIsEleven, null, 2));
            mainMenu.AddChoice("Multiplication of same first digit and second adding to 10",    new Configuration(Operation.Multiplication, Constraints.LastDigitsAddTo10 | Constraints.FirstDigitIsSame, 2, 2));
            
            mainMenu.AddChoice("Square",                                                        new Configuration(Operation.Square, Constraints.None, null, 1, true));
            mainMenu.AddChoice("Cube",                                                          new Configuration(Operation.Cube, Constraints.None, null, 1, true));
            
            mainMenu.AddChoice("Day of the week on New Year's Day in the 2000s",                new Configuration(Operation.DayOfWeek, Constraints.None, 1, 1, true));


            mainMenu.ChoiceMade += processChoice;
            mainMenu.Execute();
        }
    }
}
