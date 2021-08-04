// See https://aka.ms/new-console-template for more information
using Spectre.Console;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace PizzaApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AnsiConsole.Render(new FigletText("The Pizza Place")
                    .LeftAligned()
                    .Color(Color.Red));

            Menu.DisplayMenu();

            Order order = new Order();
            bool keepActive = true;

            while (keepActive) 
            {
                order.PlaceOrder();
                if (!AnsiConsole.Confirm("Would you like another pizza?"))
                {
                    keepActive = false;
                }
            }
            
            if (AnsiConsole.Confirm("Place this order?")) 
            {
                order.WriteToDatabase();
            }
        }
    }
}
