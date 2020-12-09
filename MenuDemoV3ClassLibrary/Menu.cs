using System;
using System.Collections.Generic;
using System.Text;

namespace MenuDemoV3ClassLibrary
{
    public class Menu
    {
        private string _name;
        private int _Id;

        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }


        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private List<Category> _categories = new List<Category>();

        public List<Category> Categories
        {
            get { return _categories; }
            set { _categories = value; }
        }


    }
}
