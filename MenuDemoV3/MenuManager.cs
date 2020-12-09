using MenuDemoV3ClassLibrary;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MenuDemoV3
{
    public class MenuManager
    {
        private DataManager dataManager;
        private Restaurant _selectedRestaurant;
        
        public MenuManager(DataManager dataManager)
        {
            this.dataManager = dataManager;
            var restaurants = DataManager.GetRestaurants();
            Restaurant restaurant = null;

            if (restaurants.Count > 0)
            {
                restaurant = DataManager.GetAllRestaurantDataWithID(restaurants[0].Id);
            }
            else
            {
                restaurant = new Restaurant();
                restaurant.Name = "Default restaurant";
            }

            if (restaurant != null)
            {
                this.SelectedRestaurant = restaurant;
            }
            Init();
        }
        public Restaurant SelectedRestaurant
        {
            get { return _selectedRestaurant; }
            set { _selectedRestaurant = value; }
        }


        public void Init()
        {
            Console.Title = "MenuDemo";
            Console.OutputEncoding = System.Text.Encoding.UTF8;
        }

        public void StartMainMenu()
        {
            bool showMenu = true;
            while (showMenu)
            {
                //Päävalikko
                Console.Clear();
                string str = "\n-------- MenuDemo ---------";
                str += $"\n Valittu ravintola : {SelectedRestaurant.Name}";
                str += "\n1. Näytä annokset";
                str += "\n2. Lisää Annos";
                str += "\n3. Näytä ruokalista";
                str += "\n4. Lisää Uusi ravintoa";
                str += "\n5. Lisää kategoria ruokalistaan";
                str += "\n6. Lisää annos ruokalistaan";

                str += "\n";
                str += "\n9. Vaihda editoitava ravintola";
                str += "\n0. Exit";

                str += "\n\nValitse toiminto:";
                Console.WriteLine(str);
                ConsoleKey selected = Console.ReadKey(true).Key;

                switch (selected)
                {
                    case ConsoleKey.D1:
                        Console.Clear();
                        PrintListNamesWithNumbers(dataManager.GetAllDishes());
                        Console.ReadKey();
                        break;
                    case ConsoleKey.D2:
                        Console.Clear();
                        Console.WriteLine("Luo lisättävä annos.");
                        Dish dish = CreateDish();
                        //dataManager.InsertDish(dish);
                        Console.WriteLine("Annos lisätty. Paina mitä tahansa.");
                        Console.ReadKey();
                        break;
                    case ConsoleKey.D3:
                        Console.Clear();
                        //var restaurant = SelectFromList(DataManager.GetRestaurants());
                        var menu = SelectFromList(SelectedRestaurant.Menus);
                        PrintMenu(menu);
                        Console.ReadKey();
                        break;
                    case ConsoleKey.D5:
                        //var r = SelectFromList(DataManager.GetRestaurants());
                        var m = SelectFromList(SelectedRestaurant.Menus);
                        m.Categories.Add(AddCategory());
                        Console.WriteLine("Kategoria lisätty.");
                        Console.ReadKey();
                        break;
                    case ConsoleKey.D6:
                        Console.Clear();
                        m = SelectFromList(SelectedRestaurant.Menus, "Valitse ruokalista:\n");
                        var ca = SelectFromList(m.Categories, "Valitse kategoria johon lisätään:\n");
                        Console.Clear();
                        Console.WriteLine($"Lisää annos ruokalistaan {m.Name} / {ca.Name}");
                        var di = SelectFromList(dataManager.GetAllDishes());
                        DataManager.AddDishToCategory(di.Id, ca.Id );
                        UpdateSelectedRestaurant();
                        
                        Console.WriteLine("Lisätty...");
                        Console.ReadKey();
                        break;
                    case ConsoleKey.D9:
                        SelectedRestaurant = SelectFromList(DataManager.GetRestaurants());
                        break;
                    case ConsoleKey.D0:
                        showMenu = false;
                        break;
                }
            }   
        }

        private void UpdateSelectedRestaurant()
        {
            SelectedRestaurant = DataManager.GetAllRestaurantDataWithID(SelectedRestaurant.Id);
        }

        private Category AddCategory()
        {
            Console.WriteLine("Anna kategorian nimi:");
            Category category = new Category();
            category.Name = Console.ReadLine();
            Console.WriteLine("Anna kategorian kuvaus:");
            category.Description = Console.ReadLine();
            return category;
        }

        private void ShowMenuOptions(List<Menu> menus)
        {
            bool show = true;
            while (show)
            {
                Console.Clear();
                PrintListNamesWithNumbers(menus);
                Console.WriteLine("(P)oista (L)isää (0)Exit");
                ConsoleKey selected = Console.ReadKey().Key;
                switch (selected)
                {
                    case ConsoleKey.L:
                        Dish d = CreateDish();
                        
                        break;
                    default:
                        break;
                }

            }
        }

        private void GetDeleteInsertEditFromUser(Action add=null)
        {
            bool show = true;
            while (show)
            {
                Console.WriteLine("(L)isää  -  (P)oista  -   (0)Takaisin");
                ConsoleKey selected = Console.ReadKey().Key;

                switch (selected)
                {
                    case ConsoleKey.D0:
                        show = false;
                        break;
                    case ConsoleKey.L:
                        add();
                        break;
                }
            }
        }

        public void AddNewDishToDB()
        {
            Dish dish = CreateDish();
           // dataManager.InsertDish(dish);
        }

        public Dish CreateDish()
        {
            Dish dish = new Dish();
            
            //Name
            string name = null;
            while (name == null)
            {
                Console.WriteLine("Syötä annoksen nimi:");
                name = StringChekker(Console.ReadLine());
            }
            
            //Type
            Console.WriteLine("Valitse annoksen tyyppi:");
            Console.WriteLine("1. Ruoka");
            Console.WriteLine("2. Juoma");

            int selected = GetIntFromUser();

            if(selected == 2)
            {
                dish = new Drink();
            }

            if(dish.GetType() == typeof(Drink))
            {
                dish = GetDrinkAttributesFromUser(dish as Drink);
            }
            
            dish.Name = name;
            dish.Price = GetFloatFromUser("Anna annoksen hinta:");

            GetAllergensFromUser(dish);

            return dish;
        }

        static public Dish GetAllergensFromUser(Dish dish)
        {

            Console.WriteLine("Sisältääkö annos seuraavaa ruoka-ainetta?");
            Console.WriteLine("Vastaa (K/E)");

            //Get an array from enums
            Allergen.AllergenType[] allerList = (Allergen.AllergenType[])Enum.GetValues(typeof(Allergen.AllergenType));
            
            foreach (Allergen.AllergenType alty in allerList)
            {
                Console.WriteLine(alty);
                bool answer = GetYesOrNoFromUser();
                if (answer)
                {
                    dish.AddAllergen(alty);
                }
            }
            return dish;
        }

        public Drink GetDrinkAttributesFromUser(Drink drink)
        {
            drink.SizeInSentiliters = GetFloatFromUser("Anna koko desilitroina:");
            drink.Vol = GetIntFromUser("Anna alkoholiprosentti");
            return drink;
        }

        static public bool GetYesOrNoFromUser(string question=null)
        {
            //Merkin näyttö pois päältä...?
            bool success = false;
            while (!success)
            {
                if (question != null) Console.WriteLine(question);
                ConsoleKey consoleKey = Console.ReadKey().Key;
                if(consoleKey == ConsoleKey.Y || consoleKey == ConsoleKey.K)
                {
                    return true;
                }
                if(consoleKey == ConsoleKey.N || consoleKey == ConsoleKey.E) 
                {
                    return false;
                }
            }
            return false;
        }

        public float GetFloatFromUser(string question=null)
        {
            bool success = false;
            while (success == false)
            {
                //if empty - return 0.0F
                if (question != null) Console.WriteLine(question);
                string fromUser = Console.ReadLine();
                if (String.IsNullOrEmpty(fromUser))
                {
                    return 0.0F;
                }

                float toReturn;
                success = float.TryParse(fromUser, out toReturn);
                if (success)
                {
                    return toReturn;
                }
            }
            return 0.0F;
        }

        public int GetIntFromUser(string question = null)
        {
            //Check if parsing was OK!
            bool success = false;
            while (success == false)
            {
                if (question != null) Console.WriteLine(question);
                string fromUser = Console.ReadLine();
                
                //user just presses enter...
                if (String.IsNullOrEmpty(fromUser))
                {
                    return 0;
                }

                //try to parse..
                success = int.TryParse(fromUser, out int toReturn);
                if (success)
                {
                    return toReturn;
                }
            }
            return 0;
        }

        public string StringChekker(string str)
        {
            
            if (String.IsNullOrWhiteSpace(str))
            {
                return null;
            }
            return str.Trim();
        }

        public static void PrintDish(Dish dish)
        {
            Console.Write($"{dish.Name} - {dish.Price} €");
            Type type = dish.GetType();
            if(type == typeof(Drink))
            {
                //Cast new object of a type
                Drink drink = (Drink)dish;
                Console.Write($" size:{drink.SizeInSentiliters}dl");
                
                //use 'as' kayword 
                Console.Write($" alcohol:{(dish as Drink).IsAlcoholic}");
            }

            var allergenlist = dish.GetAllergens();
            if(allergenlist.Count > 0)
            {
                Console.Write("\n\tSisältää:");
                string list = "";
                foreach (Allergen.AllergenType allergen in allergenlist)
                {
                   list += (Allergen.GetAllergenString(allergen)+"("+Allergen.allergenChars[allergen]+"),");
                }
                list = list.Remove(list.Length - 1);
                Console.Write(list);
            }
            Console.WriteLine();
        }

        public static void PrintCategory(Category category)
        {
            Console.WriteLine($"{category.Name}");
            Console.WriteLine($"{category.Description}");
            foreach (Dish dish in category.Dishes)
            {
                Console.Write("\t");
                PrintDish(dish);
            }
        }

        public static void PrintListNamesWithNumbers<T>(List<T> list)
        {
            Type type = typeof(T);
            if (type.GetProperty("Name") != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    string valu = list[i].GetType().GetProperty("Name").GetValue(list[i]).ToString();
                    Console.WriteLine($"{i+1}. {valu}");
                }
            }
        }

        public static T SelectFromList<T>(List<T> list, string question="Valitse Listasta\n")
        {
            Console.WriteLine(question);
            PrintListNamesWithNumbers(list);
            Console.WriteLine("\nValintasi?");
            int selection = int.Parse(Console.ReadLine());
            return list[selection-1];
        }

        public Restaurant SelectRestaurant()
        {
            PrintListNamesWithNumbers(DataManager.GetRestaurants());
            Console.WriteLine("\nValintasi?");
            int selection = int.Parse(Console.ReadLine());
            return DataManager.GetRestaurants()[selection-1];

        }

        public static void PrintMenu(Menu menu)
        {
            Console.WriteLine($"{menu.Name}");
            foreach (Category category in menu.Categories)
            {
                PrintCategory(category);
            }
        }

        public void Test()
        {
            Restaurant r = SelectFromList(DataManager.GetRestaurants());
            Menu menu = SelectFromList(r.Menus);
            Category c = SelectFromList(menu.Categories);
            Dish d = SelectFromList(c.Dishes);
            PrintDish(d);


        }
    }
}
