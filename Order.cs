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
                    .AddChoices(Menu.Sizes.Keys.ToArray()));

            var dough = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What pizza dough would you like?")
                    .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
                    .AddChoices(Menu.Dough.Keys.ToArray()));

            var sauce = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Pick your favourite sauce!")
                    .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
                    .AddChoices(Menu.Sauces.Keys.ToArray()));

            var toppings = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .Title("What toppings would you like for your pizza?")
                    .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
                    .InstructionsText(
                        "[grey](Press [blue]<space>[/] to toggle a topping, " + 
                        "[green]<enter>[/] to accept)[/]")
                    .AddChoices(Menu.Toppings.Keys.ToArray()));

           if (AnsiConsole.Confirm("Confirm pizza order")) 
           {
               Pizza p = new Pizza(size, dough, toppings.ToArray(), sauce);
               p.Price = CalculatePrice(p);
               Pizzas.Add(p);
           }
        }

        public void GetOrderPrice()
        {
            double total = 0;
            foreach (var pizza in this.Pizzas)
            {
                total += pizza.Price;
            }
            AnsiConsole.Markup($"Total price: [green]{total}[/]\n\n");
        }
        public double CalculatePrice(Pizza pizza) 
        {
            double sum = 0;
            foreach (var topping in pizza.Toppings)
            {
                sum += Menu.Toppings[topping];
            }

            sum += Menu.Sizes[pizza.Size] * Menu.Dough[pizza.Dough] + Menu.Sauces[pizza.BaseSauce];
            return sum;
        }
        public IEnumerable<Pizza> GetOrders()
        {
            using (var jsonFileReader = File.OpenText("orders.json"))
            {
                string content = jsonFileReader.ReadToEnd();
                if (content.Length == 0)
                {
                    return null;
                }
                return JsonSerializer.Deserialize<Pizza[]>(content,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
        }

        public void WriteToDatabase()
        {
            var pizzas = GetOrders();
            if (pizzas == null) 
            {
                 pizzas = this.Pizzas.ToArray();
            }
            else
            {
                var allOrders = pizzas.ToList();
                allOrders.AddRange(this.Pizzas);
                pizzas = allOrders.ToArray();
            }
            using (var outputStream = File.OpenWrite("orders.json"))
            {
                JsonSerializer.Serialize<IEnumerable<Pizza>>(
                    new Utf8JsonWriter(outputStream, new JsonWriterOptions
                    {
                        SkipValidation = true,
                        Indented = true
                    }),
                    pizzas
                );
            }
        }
    }
}