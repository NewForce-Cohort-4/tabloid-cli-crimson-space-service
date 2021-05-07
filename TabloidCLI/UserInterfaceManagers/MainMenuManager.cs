using System;

namespace TabloidCLI.UserInterfaceManagers
{
    public class MainMenuManager : IUserInterfaceManager
    {
        private const string CONNECTION_STRING = 
            //change to Express02 for Matt's local machine
            @"Data Source=localhost\SQLEXPRESS;Database=TabloidCLI;Integrated Security=True";

        public IUserInterfaceManager Execute()
        {

            Console.WriteLine();
            Console.WriteLine("Welcome to Earth! We are glad to have you here.");
            Console.WriteLine("Main Menu");

            Console.WriteLine(" 1) Journal Management");
            Console.WriteLine(" 2) Blog Management");
            Console.WriteLine(" 3) Author Management");
            Console.WriteLine(" 4) Post Management");
            Console.WriteLine(" 5) Tag Management");
            Console.WriteLine(" 6) Search by Tag");
            Console.WriteLine(" 7) Display Options");
            Console.WriteLine(" 8) Note Management");
            Console.WriteLine(" 0) Exit");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1": return new JournalManager(this, CONNECTION_STRING);
                case "2": return new BlogManager(this, CONNECTION_STRING);
                case "3": return new AuthorManager(this, CONNECTION_STRING);
                case "4": return new PostManager(this, CONNECTION_STRING);
                case "5": return new TagManager(this, CONNECTION_STRING);
                case "6": return new SearchManager(this, CONNECTION_STRING);
                case "7": DisplayOptions();  return this;
                case "8": return new NoteManager(this, CONNECTION_STRING);
                case "0":
                    Console.WriteLine("Good bye");
                    return null;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
         }
        public void DisplayOptions()
        {

            Console.WriteLine("Taste the rainbow and choose a background color. Enter a number.");
            Console.WriteLine(" 1) Black");
            Console.WriteLine(" 2) Red");
            Console.WriteLine(" 3) Yellow");
            Console.WriteLine(" 4) Green");
            Console.WriteLine(" 5) Blue");

            Console.Write("> ");
            string color = Console.ReadLine();

            if (color == "1")
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Clear();
            }

            else if (color == "2")
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.Clear();
            }

            else if (color == "3")
            {
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Clear();
            }

            else if (color == "4")
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Clear();
            }

            else if (color == "5")
            {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.Clear();
            }

            else
            {
                Console.WriteLine("Invalid selection. Default color chosen.");

            }
        }
    }
}
