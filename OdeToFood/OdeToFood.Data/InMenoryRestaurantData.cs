using System;
using System.Collections.Generic;
using System.Linq;
using OdeToFood.Core;

namespace OdeToFood.Data
{
    public class InMemoryRestaurantData : IRestaurantData
    {
        private IList<Restaurant> Restaurants;

        public InMemoryRestaurantData()
        {
            this.Restaurants = new List<Restaurant>(){
                new Restaurant() {Id = 1,Name= "Boyle's Pizzas", Location="Brooklyn",Cuisine=CuisineType.Italian},
                new Restaurant() {Id = 2,Name= "Rajesh's Samosas", Location="California",Cuisine=CuisineType.Indian},
                new Restaurant() {Id = 3,Name= "Delgado's Tacos", Location="Florida",Cuisine=CuisineType.Mexican},
            };
        }

        public IEnumerable<Restaurant> GetRestaurantByName(string name)
        {
            return this.Restaurants.Where(res => string.IsNullOrEmpty(name) || res.Name.StartsWith(name)).OrderBy(restaurant => restaurant.Name);        }

        public Restaurant GetRestaurantById(int id)
        {
            return this.Restaurants.FirstOrDefault(res => res.Id == id);
        }

        public Restaurant UpdateRestaurant(Restaurant updatedRestaurant)
        {
            var restaurant = this.Restaurants.FirstOrDefault(res => res.Id == updatedRestaurant.Id);
            if(restaurant != null)
            {
                restaurant.Name = updatedRestaurant.Name;
                restaurant.Location = updatedRestaurant.Location;
                restaurant.Cuisine = updatedRestaurant.Cuisine;
            }

            return restaurant;
        }

        public int Commit()
        {
            return 0;
        }

        public Restaurant CreateRestaurant(Restaurant newRestaurant)
        {
            this.Restaurants.Add(newRestaurant);
            newRestaurant.Id = this.Restaurants.Max(res => res.Id) + 1;
            return newRestaurant;
        }
    }
}
