using Spectre.Console;
using System.Text.Json.Nodes;

namespace PizzaApp 
{
    public static class Menu 
    {
        public static List<string> Sizes = new List<string>();
        public static List<string> Dough = new List<string>();
        public static List<string> Toppings = new List<string>();
        public static List<string> Sauces = new List<string>();
        public static void DisplayMenu() {
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
                            Sizes.Add(pair.Key);
                        }
                    } 
                    else 
                    {
                        table.AddColumn("Price");
                        foreach (var pair in prop.Value.AsObject())
                        {
                            table.AddRow(pair.Key, $"{pair.Value}");
                            switch (prop.Key) {
                                case "dough": Dough.Add(pair.Key); break;
                                case "topping": Toppings.Add(pair.Key); break;
                                case "base": Sauces.Add(pair.Key); break;
                            }
                        }
                    }
                    AnsiConsole.Render(table.RoundedBorder());
                }
            }
        }              
    }
}
