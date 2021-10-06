using System;
using System.Collections.Generic;
using OdeToFood.Core;

namespace OdeToFood.Data 
{
    public interface IRestaurantData
    {
        IEnumerable<Restaurant> GetRestaurantByName(string name);
        Restaurant GetRestaurantById(int id);
        Restaurant UpdateRestaurant(Restaurant updatedRestaurant);
        Restaurant CreateRestaurant(Restaurant newRestaurant);
        Restaurant DeleteRestaurant(int id);
        int GetCount();
        int Commit();
    }

}