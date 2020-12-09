using System;
using System.Collections.Generic;
using System.Text;

namespace MenuDemoV3ClassLibrary
{
    public class Dish
    {
        private List<Allergen.AllergenType> _allergenTypes = new List<Allergen.AllergenType>();

        private string _name;
        private float _price;
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private int _dishType;

        public int DishType
        {
            get { return _dishType; }
            set { _dishType = value; }
        }


        public Dish() {}

        public Dish(string name)
        {
            this.Name = name;
        }

        public Dish(string name, float price) : this(name)
        {
            this.Price = price;
        }
        public Dish(string name, float price, List<Allergen.AllergenType> allergens):this(name,price)
        {
            this.AddAllergen(allergens);
        }

        public void AddAllergen(Allergen.AllergenType type)
        {
            if (_allergenTypes.Contains(type)) return;
            _allergenTypes.Add(type);
        }

        public void AddAllergen(List<Allergen.AllergenType> list)
        {
            foreach (Allergen.AllergenType item in list)
            {
                AddAllergen(item);
            }
        }

        public List<Allergen.AllergenType> GetAllergens()
        {
            return _allergenTypes;
        }

        public float Price
        {
            get { return _price; }
            set { _price = value; }
        }


        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public int DishTypeInDB
        {
            get{ return GetDishTypeInDB(this); }
        }


        public override string ToString()
        {
            return $"{Name} - {Price}€";
        }

        private static int GetDishTypeInDB(Dish dish)
        {
            //must correlate to the types in DB
            if(typeof(Dish) == dish.GetType())
            {
                return 1;
            }
            if (typeof(Drink) == dish.GetType())
            {
                return 2;
            }
            return 0;
        }

        public static Dish GetDistClassWithDishType(int type, Dish template = null)
        {
            Dish dish = null;
            
            if(type == 1)
            {
                dish = new Dish();
            }
            if(type == 2)
            {
                dish = new Drink();
            }

            if(template != null)
            {
                dish.Name = template.Name;
                dish.Description = template.Description;
                dish.Price = template.Price;
                dish.Id = template.Id;
            }

            return dish;
        }
    }
}
