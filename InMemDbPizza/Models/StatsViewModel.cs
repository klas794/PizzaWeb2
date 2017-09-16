using InMemDbPizza.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPizzaWeb.Models
{
    public class StatsViewModel
    {
        [Display(Name="Number of orders: ")]
        public int NumberOfOrders { get; set; }

        [Display(Name= "Delivered orders")]
        public int NumberOfDeliveredOrders { get; set; }

        [Display(Name = "Delivered dishes")]
        public int NumberOfDeliveredDishes { get; set; }


        [Display(Name= "Orders today")]
        public int OrdersToday { get; set; }

        [Display(Name= "Orders this week")]
        public int OrdersThisWeek { get; set; }

        [Display(Name= "Carts created")]
        public int CartsCreated { get; set; }

        [Display(Name= "Visitor conversion ratio")]
        [DisplayFormat(DataFormatString = "{0:0}")]
        public decimal ConversionRatioCartsOrders { get; set; }

        [Display(Name = "Most popular dish: ")]
        public Dish MostPopularDish { get; set; }

        [Display(Name = "Most popular ingredient: ")]
        public Ingredient MostPopularIngredient { get; set; }


        [Display(Name = "Last order time")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime? LastOrderTime { get; set; }


        [Display(Name = "Number of dishes: ")]
        public int NumberOfDishes { get; set; }

        [Display(Name = "Number of ingredients: ")]
        public int NumberOfIngredients { get; set; }


        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        [Range(1, int.MaxValue)]
        [Display(Name ="Total income")]
        public decimal TotalIncome { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        [Range(1, int.MaxValue)]
        [Display(Name = "Average income per cart")]
        public decimal AverageIncome { get; set; }
    }
}
