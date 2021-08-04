using System.Text.Json;

namespace PizzaApp
{

    public class Pizza
    {
        private string Size { get; set; }
        private string Dough { get; set; }
        private List<string> Toppings { get; set; }
        private string Base { get; set; }

        public Pizza(string size, string dough, List<string> toppings, string sauce) 
        {
            Size = size;
            Dough = dough;
            Toppings = toppings;
            Base = sauce;
        }

        public override string ToString() => JsonSerializer.Serialize<Pizza>(this);
    }
}