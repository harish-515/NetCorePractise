using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

using OdeToFood.Core;
using OdeToFood.Data;

namespace OdeToFood.Pages.Restaurants
{
    public class ListModel : PageModel
    {

        private readonly IConfiguration config;
        private readonly IRestaurantData restaurant;

        [BindProperty(SupportsGet = true)]
        public string SearchText { get; set; }
        public IEnumerable<Restaurant> Restaurants { get; set; }
        public string Message { get; set; }
        public ListModel(IConfiguration config, IRestaurantData restaurant)
        {
            this.restaurant = restaurant;
            this.config = config;
        }
        public void OnGet()
        {
            this.Message = this.config["Message"];
            this.Restaurants = this.restaurant.GetRestaurantByName(SearchText);
        }
    }
}
