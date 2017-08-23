using InMemDbPizza.Models;

namespace ProjectPizzaWeb.Models
{
    public class CartItemIngredient
    {
        public int CartItemId { get; set; }
        public Cart Cart { get; set; }
        public int CartId { get; set; }
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }
        public bool Enabled { get; set; }
    }
}