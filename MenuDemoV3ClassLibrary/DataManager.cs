using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace MenuDemoV3ClassLibrary
{
    public class DataManager:DataAccess
    {
        private List<Dish> _dishes = new List<Dish>();
        private List<Restaurant> _restaurants = new List<Restaurant>();

        public static Dish GetDishWithID(int dishID)
        {
            string str = DataAccess.CnnVal(DataAccess.currentDBname);
            using (IDbConnection connection = new SqlConnection(str))
            {
                //Haetaan "perus" dish
                var toReturn = connection.QueryFirst<Dish>("SELECT * FROM Dishes WHERE id=@id", new { id = dishID });
                
                //Tarkastetaan, mitä luokkaa objektin tulee olla, käyttäen DistType valueta.
                Dish dish = Dish.GetDistClassWithDishType(toReturn.DishType, toReturn);

                if(dish.GetType() == typeof(Drink))
                {
                    dynamic drinkData = connection.QuerySingle("SELECT * FROM Drinks WHERE DishId=@dishId", new { dishId = dish.Id });
                    (dish as Drink).Vol = (float)drinkData.Vol;
                    (dish as Drink).SizeInSentiliters = drinkData.SizeInSentiliters;
                }


                //Haetaan annokseen kuuluvat allergeenit.
                var allergens = connection.Query<Allergen.AllergenType>("SELECT AllergenId FROM AllergensInDishes WHERE DishID = @id", new { id = dishID }).ToList();
                //lisätään allergeenit objektiin
                dish.AddAllergen(allergens);
                
                

                return dish;
            } 
        }

        public static List<Dish> GetAllDishesDB()
        {
            string str = DataAccess.CnnVal(DataAccess.currentDBname);
            using (IDbConnection connection = new SqlConnection(str))
            {
                var toReturn = connection.Query<Dish>("SELECT * FROM Dishes").ToList();
                return toReturn;
            }
        }

        public static int InsertDish(Dish dish)
        {
            string str = DataAccess.CnnVal(DataAccess.currentDBname);
            int dishId;
            using (IDbConnection connection = new SqlConnection(str))
            {
                dishId = connection.QuerySingle<int>("INSERT Dishes (name, description, price, dishtype) OUTPUT inserted.id " +
                    " VALUES (@name, @description, @price, @dishtype)",new {name=dish.Name, description=dish.Description, price=dish.Price, dishtype=dish.DishTypeInDB });
            }

            if(dishId>0 && dish.GetAllergens().Count > 0)
            {
                using (IDbConnection connection = new SqlConnection(str))
                {
                    foreach (var allergen in dish.GetAllergens())
                    {
                        connection.Execute("INSERT AllergensInDishes(AllergenId,DishId)" +
                            " VALUES(@AllergenId, @DishId) ", new { AllergenId = (int)allergen, DishId = dishId });
                    }
                }
            }

            return dishId;
        }

        public static List<Menu> GetAllMenusDB()
        {
            string str = DataAccess.CnnVal(DataAccess.currentDBname);
            using (IDbConnection connection = new SqlConnection(str))
            {
                var toReturn = connection.Query<Menu>("SELECT * FROM Menus").ToList();
                return toReturn;
            }
        }

        public static int[] GetDishIDsFromCategory(int categoryID)
        {
            string str = DataAccess.CnnVal(DataAccess.currentDBname);
            using (IDbConnection connection = new SqlConnection(str))
            {
                var toReturn = connection.Query<int>("SELECT DishId as Id FROM CategoryDishes WHERE CategoryId=@id", new { id=categoryID } ).ToArray();
                return toReturn;
            }
        } 

        public static List<Dish> GetAllDishesInCategoty(int categoryID)
        {
            int[] ids = GetDishIDsFromCategory(categoryID);
            List<Dish> dishes = new List<Dish>();
            foreach (int id in ids)
            {
                dishes.Add(GetDishWithID(id));
            }
            return dishes;
        }

        public static void AddDishToCategory(int dishID, int categoryId)
        {
            string str = DataAccess.CnnVal(DataAccess.currentDBname);
            using (IDbConnection connection = new SqlConnection(str))
            {
                connection.Execute("INSERT CategoryDishes(DishId,CategoryId) VALUES (@dishId, @categoryId)", new { dishID = dishID, categoryId = categoryId });
            }
            
        }

        public static List<Restaurant> GetRestaurants()
        {
            string str = DataAccess.CnnVal(DataAccess.currentDBname);
            using (IDbConnection connection = new SqlConnection(str))
            {
                var toReturn = connection.Query<Restaurant>("SELECT * FROM Restaurants").ToList();
                return toReturn;
            }
        }

        public static Restaurant GetAllRestaurantDataWithID(int restaurantId)
        {
            Restaurant restaurant = GetRestaurant(restaurantId);
            restaurant.Menus = GetMenusWithRestaurantID(restaurant.Id);
            
            foreach (Menu menu in restaurant.Menus)
            {
                menu.Categories = GetCategoriesWithMenuID(menu.Id);

                foreach (Category category in menu.Categories)
                {
                    category.Dishes = GetAllDishesInCategoty(category.Id);
                }
            }

            return restaurant;
        }

        public static Restaurant GetRestaurant(int id)
        {
            string str = DataAccess.CnnVal(DataAccess.currentDBname);
            using (IDbConnection connection = new SqlConnection(str))
            {
                var toReturn = connection.QueryFirst<Restaurant>("SELECT * FROM Restaurants WHERE ID=@ID", new {id=id});
                return toReturn;
            }
        }

        public static List<Menu> GetMenusWithRestaurantID(int id)
        {
            string str = DataAccess.CnnVal(DataAccess.currentDBname);
            using (IDbConnection connection = new SqlConnection(str))
            {
                string query =
                "SELECT menus.* FROM RestaurantMenus " +
                "INNER JOIN Menus ON menus.Id = RestaurantMenus.Id " +
                "WHERE RestaurantId =@id ";
                var toReturn = connection.Query<Menu>(query, new { id = id }).ToList();
                return toReturn;
            }
        }

        public static List<Category> GetCategoriesWithMenuID(int id)
        {
            string str = DataAccess.CnnVal(DataAccess.currentDBname);
            using (IDbConnection connection = new SqlConnection(str))
            {
                string query =
                "SELECT c.* FROM MenuCategories as mc INNER JOIN Categories as c ON c.Id = mc.CategoryId WHERE mc.MenuId=@id";
                var toReturn = connection.Query<Category>(query, new { id = id }).ToList();
                return toReturn;
            }
        }




        public void InsertRestaurant(Restaurant restaurant)
        {
            _restaurants.Add(restaurant);
        }

        public List<Dish> GetAllDishes()
        {
            return GetAllDishesDB(); 
            //return _dishes;      
        }

        public DataManager()
        {
           // AddTestData();
        }

        private void AddTestData()
        {

            //this.InsertDish(new Dish { Name = "Hernekeitto", Price = 5.3F });
            //this.InsertDish(new Dish { Name = "Lihakeitto", Price = 7.5F });
            //this.InsertDish(new Dish { Name = "Kaalikääryle", Price = 10.25F });
            //this.InsertDish(new Dish { Name = "Pihvi ja pottu", Price = 16F });
            //this.InsertDish(new Dish { Name = "Kuhaa naavapedillä", Price = 19.20F });
            //this.InsertDish(new Dish { Name = "Janssoninkiusaus", Price = 10F });

            //this.InsertDish(new Drink { Name = "JeekeriMaisteri", Vol = 40, Price = 10, SizeInDesiliters = 0.4F });
            //this.InsertDish(new Drink { Name = "Maito", Vol = 0, Price = 3, SizeInDesiliters = 5F });
            //this.InsertDish(new Drink { Name = "Vichy", Vol = 0, Price = 5, SizeInDesiliters = 3.3F });
            //this.InsertDish(new Drink { Name = "Olut", Vol = 4, Price = 7, SizeInDesiliters = 3.3F });
            //this.InsertDish(new Drink { Name = "Kahvi / Tee", Vol = 0, Price = 2.5F, SizeInDesiliters = 2F });

            Restaurant restaurant = new Restaurant() { Name = "Topin Baari" };
            Category category = new Category { Name = "Pääruuat" };
            Menu menu = new Menu { Name = "Topin Tarjonta" };
            menu.Categories.Add(category);
            restaurant.Menus.Add(menu);
            category.Dishes.Add(this.GetAllDishes()[0]);
            category.Dishes.Add(this.GetAllDishes()[1]);
            category.Dishes.Add(this.GetAllDishes()[2]);

            Restaurant restaurant1 = new Restaurant() { Name = "Fiini pulju" };
            Menu menu1 = new Menu() { Name = "Lounaslista" };
            Category lounaslista = new Category() { Name = "Lounaslista", Description = "Herkullisia lounaista" };
            lounaslista.Dishes.Add(GetAllDishes()[0]);
            lounaslista.Dishes.Add(GetAllDishes()[2]);
            lounaslista.Dishes.Add(GetAllDishes()[4]);
            lounaslista.Dishes.Add(GetAllDishes()[5]);
            menu1.Categories.Add(lounaslista);
            restaurant1.Menus.Add(menu1);


            this.InsertRestaurant(restaurant);
            this.InsertRestaurant(restaurant1);

        }
    }
}
