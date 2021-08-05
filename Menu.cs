using Spectre.Console;
using System.Text.Json.Nodes;

namespace PizzaApp 
{
    public static class Menu 
    {
        public static IDictionary<string, double> Sizes { get; set; } = new Dictionary<string, double>();
        public static IDictionary<string, double> Dough { get; set; } = new Dictionary<string, double>();
        public static IDictionary<string, double> Toppings { get; set; } = new Dictionary<string, double>();
        public static IDictionary<string, double> Sauces { get; set; } = new Dictionary<string, double>();
        
        public static void DisplayMenu() 
        {
           using (var jsonFileReader = File.OpenText("pizzaconfigurations.json"))
            {
                string file = jsonFileReader.ReadToEnd();
                JsonObject menu = JsonNode.Parse(file).AsObject();
                foreach (var prop in menu) 
                {
                    var table = new Table().RoundedBorder().BorderColor(Color.BlueViolet);
                    table.AddColumn(char.ToUpper(prop.Key[0]) + prop.Key.Substring(1));
                    if (prop.Key == "size") 
                    {
                        table.AddColumn("Dough Multiplier");
                        foreach (var pair in prop.Value.AsObject()) 
                        {
                            table.AddRow(pair.Key, $"{pair.Value}");
                            Sizes.Add(pair.Key, (double)pair.Value);
                        }
                    } 
                    else 
                    {
                        table.AddColumn("Price");
                        foreach (var pair in prop.Value.AsObject())
                        {
                            table.AddRow(pair.Key, $"{pair.Value}");
                            switch (prop.Key) {
                                case "dough": Dough.Add(pair.Key, (double)pair.Value); break;
                                case "topping": Toppings.Add(pair.Key, (double)pair.Value); break;
                                case "base": Sauces.Add(pair.Key, (double)pair.Value); break;
                            }
                        }
                    }
                    AnsiConsole.Render(table.RoundedBorder());
                }
            }
        }              
    }
}