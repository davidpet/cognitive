using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLib
{
    public class Menu
    {
        public Menu(string title)
        {
            Title = title;
        }

        private void Print()
        {
            Console.WriteLine("----------------------------------");
            Console.WriteLine(Title);
            Console.WriteLine("----------------------------------");

            for (int i = 0; i < Choices.Count; i++)
            {
                KeyValuePair<string, object> choice = Choices[i];

                Console.WriteLine("\t" + (i+1).ToString() + ".\t" + choice.Key);
            }
            Console.WriteLine("----------------------------------");
            Console.Write("Please make a selection: ");
        }

        private object GetChoice()
        {
            string choiceText = Console.ReadLine().Trim();
            Console.WriteLine("----------------------------------");

            uint choice;
            if (uint.TryParse(choiceText, out choice) && choice <= Choices.Count && choice > 0)
                return Choices[(int)choice - 1].Value;

            return null;
        }

        public void AddChoice(string text, object value)
        {
            Choices.Add(new KeyValuePair<string, object>(text, value));
        }

        public void Execute()
        {
            object choice = null;

            Print();
            choice = GetChoice();

            if (choice == null)
                return;
            if (ShouldProcessChoice(choice))
                ChoiceMade(this, choice);
            else
                ExecuteSubmenu(choice);
            Execute();
        }

        protected virtual void ExecuteSubmenu(object choice)
        {
        }

        protected virtual bool ShouldProcessChoice(object choice)
        {
            return true;
        }

        //TODO: encapsulate this better
        public static uint SolicitValue(string valueName)
        {
            Console.Write(valueName + ": ");
            string valueText = Console.ReadLine().Trim();
            try
            {
                return uint.Parse(valueText);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static bool SolicitFlag(string valueName)
        {
            Console.Write(valueName + ": ");
            string valueText = Console.ReadLine().Trim().ToLower();

            return valueText.Length > 0 && valueText[0] == 'y';
        }

        protected void RaiseChoiceMade(object choice)
        {
            if (ChoiceMade != null)
                ChoiceMade(this, choice);
        }

        protected void CopyChoiceMade(Menu destination)
        {
            if (ChoiceMade != null)
                destination.ChoiceMade += ChoiceMade;
        }

        public List<KeyValuePair<string, object>> Choices = new List<KeyValuePair<string, object>>();

        public string Title { get; set; }

        public event Action<object, object> ChoiceMade;
    }
}
