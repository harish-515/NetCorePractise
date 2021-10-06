using System;
using System.Collections.Generic;
using System.Linq;
using OdeToFood.Core;
using OdeToFood.OdeToFood.Data;

namespace OdeToFood.Data
{
    public class SqlRestaurantData : IRestaurantData
    {
        private readonly OdeToFoodDbContext db;
        public SqlRestaurantData(OdeToFoodDbContext dbcontext)
        {
            this.db = dbcontext;
        }

        public int Commit()
        {
            return this.db.SaveChanges();
        }

        public Restaurant CreateRestaurant(Restaurant newRestaurant)
        {
            this.db.Restaurants.Add(newRestaurant);
            return newRestaurant;
        }

        public Restaurant DeleteRestaurant(int id)
        {
            var restaurant = GetRestaurantById(id);
            this.db.Restaurants.Remove(restaurant);
            return restaurant;
        }

        public int GetCount()
        {
            return this.db.Restaurants.Count();
        }

        public Restaurant GetRestaurantById(int id)
        {
            return this.db.Restaurants.Find(id);
        }

        public IEnumerable<Restaurant> GetRestaurantByName(string name)
        {
            return this.db.Restaurants.Where(res => String.IsNullOrEmpty(name) || res.Name.Contains(name));
        }

        public Restaurant UpdateRestaurant(Restaurant updatedRestaurant)
        {
            var entity = this.db.Restaurants.Attach(updatedRestaurant);
            entity.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return updatedRestaurant;
        }
    }
}
