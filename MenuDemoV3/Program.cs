using MenuDemoV3ClassLibrary;
using System;
using System.Collections.Generic;

namespace MenuDemoV3
{
    class Program
    {
        static void Main(string[] args)
        {
            DataManager dm = new DataManager();
            MenuManager mm = new MenuManager(dm);
            //var luku = DataManager.InsertDishDB(dish);
            //var r = DataManager.GetAllRestaurantDataWithID(1);
            //MenuManager.PrintMenu(r.Menus[0]);
            // Dish dish = new Dish() { Name = "Testiannes", Description = "Herkullista testiä", Price = 20.5F };
            // dish.AddAllergen(Allergen.AllergenType.Kala);
            // dish.AddAllergen(Allergen.AllergenType.Laktoosi);
            //Dish d = DataManager.GetDishWithID(1002);
            mm.StartMainMenu();
        }
    }
}
