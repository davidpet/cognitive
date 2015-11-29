using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CommonLib
{
    public class FolderMenu: Menu
    {
        public FolderMenu(string path) :
            base(Path.GetFileName(path))
        {
            string[] entries = Directory.GetDirectories(path);
            if (entries.Length == 0)
            {
                entries = Directory.GetFiles(path);
                IsBottomLevel = true;
            }

            foreach (string entry in entries)
                AddChoice(Path.GetFileNameWithoutExtension(entry), entry);
        }

        protected override void ExecuteSubmenu(object choice)
        {
            FolderMenu submenu = new FolderMenu((string)choice);
            CopyChoiceMade(submenu);
            submenu.Execute();
        }

        protected override bool ShouldProcessChoice(object choice)
        {
            return IsBottomLevel;
        }

        protected bool IsBottomLevel = false;
    }
}
