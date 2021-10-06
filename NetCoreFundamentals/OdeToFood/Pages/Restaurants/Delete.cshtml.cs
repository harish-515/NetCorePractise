using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OdeToFood.Core;
using OdeToFood.Data;

namespace OdeToFood.Pages.Restaurants
{
    public class DeleteModel : PageModel
    {
        private readonly IRestaurantData restaurantData;

        public Restaurant Restaurant { get; set; }

        public DeleteModel(IRestaurantData restaurantData)
        {
            this.restaurantData = restaurantData;
        }

        public IActionResult OnGet(int restaurantId)
        {
            this.Restaurant = this.restaurantData.GetRestaurantById(restaurantId);
            if(this.Restaurant == null)
            {
                return RedirectToPage("./NotFound"); 
            }
            return Page();
        }

        public IActionResult OnPost(int restaurantId)
        {
            this.Restaurant = this.restaurantData.DeleteRestaurant(restaurantId);
            this.restaurantData.Commit();
            if(this.Restaurant == null)
            {
                return RedirectToPage("./NotFound");
            }
            return RedirectToPage("./List"); ;
        }
    }
}
