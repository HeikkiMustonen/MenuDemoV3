using System;
using System.Collections.Generic;
using System.Text;

namespace MenuDemoV3ClassLibrary
{
    public class Restaurant
    {
        private string _name;
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }


        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private List<Menu> _menus = new List<Menu>();

        public List<Menu> Menus
        {
            get { return _menus; }
            set { _menus = value; }
        }
    }
}
