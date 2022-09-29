using PDF_Randomizer;
using PDF_Seperator;
using static System.Console;

namespace IHK_Exam_Sheet_Converter
{
    public class Menu
    {
        private int SelectIndex;
        private string[] Options;

        public Menu()
        {
            
        }

        private void DisplayOptions()
        {

            for (int i = 0; i < Options.Length; i++)
            {
                string currentOption = Options[i];
                string prefix;
                if (i == SelectIndex)
                {
                    prefix = "*";
                    ForegroundColor = ConsoleColor.Black;
                    BackgroundColor = ConsoleColor.White;
                }
                else
                {
                    prefix = " ";
                    ForegroundColor = ConsoleColor.White;
                    BackgroundColor = ConsoleColor.Black;
                }
                WriteLine($"{prefix}<< {currentOption} >>");
            }
            ResetColor();
        }

        public int Run()
        {
            ConsoleKey keyPressed;
            do
            {
                Clear();
                DisplayOptions();

                ConsoleKeyInfo keyInfo = ReadKey(true);
                keyPressed = keyInfo.Key;

                // Update SelectedIndex based on arrow keys
                if (keyPressed == ConsoleKey.UpArrow)
                {
                    SelectIndex--;
                    if (SelectIndex == -1)
                    {
                        SelectIndex = Options.Length - 1;
                    }
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    SelectIndex++;
                    if (SelectIndex == Options.Length)
                    {
                        SelectIndex = 0;
                    }
                }
            }
            while (keyPressed != ConsoleKey.Enter);

            return SelectIndex;
        }

        public void MainMenu()
        {
            do
            {
                string prompt = "This is a Fancy Menu";

                string[] options = { "IHK Randomizer", "PDF Seperator", "Exit" };
                Options = options;
                SelectIndex = 0;

                SubMenu(Run());
            }
            while (true);
        }

        private void SubMenu(int selectedIndex)
        {
            string filePath = "Files";

            Randomizer randoms = new();
            Seperator seperator = new(filePath);

            string[] options = { "GA1_AE", "GA1_SI", "GA2", "AP1", "Exit" };
            for (int i = 0; i < options.Length-1; i++)
            {
                randoms.CheckForDirectory($"{filePath}\\"+options[i]);
            }

            randoms.CheckForDirectory(filePath);

            switch (selectedIndex)
            {
                case 0:
                    string prompt = "This is a Fancy Menu";

                    Options = options;
                    SelectIndex = 0;

                    int choice = Run();

                    filePath = filePath + "\\" + options[choice];
                    randoms.Start(filePath, options[choice]);

                    break;
                case 1:
                    seperator.Start("GA1_AE");
                    seperator.Start("GA1_SI");
                    seperator.Start("GA2");
                    seperator.Start("AP1");

                    break;
                case 2:
                    Environment.Exit(0);
                    break;
            }
        }
    }
}
