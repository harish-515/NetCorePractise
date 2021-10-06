using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OdeToFood.Core;
using OdeToFood.Data;

namespace OdeToFood.Pages.Restaurants
{
    public class EditModel : PageModel
    {
        private readonly IRestaurantData restaurantData;
        private readonly IHtmlHelper htmlHelper;

        [BindProperty]
        public Restaurant Restaurant { get; set; }

        public IEnumerable<SelectListItem> Cuisines { get; set; }

        public EditModel(IRestaurantData restaurantData,IHtmlHelper htmlHelper)
        {
            this.restaurantData = restaurantData;
            this.htmlHelper = htmlHelper;
        }


        public IActionResult OnGet(int? restaurantId)
        {
            this.Cuisines = this.htmlHelper.GetEnumSelectList<CuisineType>();

            if (restaurantId.HasValue)
                this.Restaurant = this.restaurantData.GetRestaurantById(restaurantId.Value);
            else
                this.Restaurant = new Restaurant();

            if(this.Restaurant == null)
            {
                return RedirectToPage("./NotFound");
            }
            return Page();
        }


        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                this.Cuisines = this.htmlHelper.GetEnumSelectList<CuisineType>();
                return Page();
            }

            if (Restaurant.Id > 0)
                this.restaurantData.UpdateRestaurant(Restaurant);
            else
                this.restaurantData.CreateRestaurant(Restaurant);


            TempData["Message"] = $"Restaurant is saved !";
            this.restaurantData.Commit();
            return RedirectToPage("./Details", new { restaurantId = Restaurant.Id });

        }

    }
}
