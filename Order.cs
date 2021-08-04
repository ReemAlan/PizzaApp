using Spectre.Console;
using System.Text.Json;

namespace PizzaApp
{
    public class Order 
    {
        public List<Pizza> Pizzas { get; } = new List<Pizza>();

        public void PlaceOrder() 
        {
            var size = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What pizza size would you like?")
                    .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
                    .AddChoices(Menu.Sizes.ToArray()));
            var dough = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What pizza dough would you like?")
                    .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
                    .AddChoices(Menu.Dough.ToArray()));
            var sauce = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Pick your favourite sauce!")
                    .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
                    .AddChoices(Menu.Sauces.ToArray()));
            var toppings = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .Title("What toppings would you like for your pizza?")
                    .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
                    .InstructionsText(
                        "[grey](Press [blue]<space>[/] to toggle a topping, " + 
                        "[green]<enter>[/] to accept)[/]")
                    .AddChoices(Menu.Toppings.ToArray()));

           if (AnsiConsole.Confirm("Confirm pizza order")) 
           {
               Pizzas.Add(new Pizza(size, dough, toppings, sauce));
           }
        }

        public void WriteToDatabase()
        {
            using (var outputStream = File.OpenWrite("orders.json"))
            {
                JsonSerializer.Serialize<IEnumerable<Pizza>>(
                    new Utf8JsonWriter(outputStream, new JsonWriterOptions
                    {
                        SkipValidation = true,
                        Indented = true
                    }),
                    this.Pizzas
                );
            }
        }
    }
}