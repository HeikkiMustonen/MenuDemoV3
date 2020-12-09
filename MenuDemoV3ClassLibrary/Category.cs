using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MenuDemoV3ClassLibrary
{
    [DebuggerDisplay("{DebuggerString}")]
    public class Category
    {
        private string DebuggerString
        {
            get
            {
                return $"{this.Name} dishes: {Dishes.Count}";
            }
        }
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private int _Id;

        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }


        private string _description;

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private List<Dish> dishes = new List<Dish>();

        public List<Dish> Dishes
        {
            get { return dishes; }
            set { dishes = value; }
        }


    }
}
