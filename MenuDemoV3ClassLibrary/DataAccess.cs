using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace MenuDemoV3ClassLibrary
{
    public class DataAccess
    {
        public static string currentDBname = "MenuDemoDB";

        public static string CnnVal(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
