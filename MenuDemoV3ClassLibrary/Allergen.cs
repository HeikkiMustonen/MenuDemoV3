using System;
using System.Collections.Generic;
using System.Text;

namespace MenuDemoV3ClassLibrary
{
    public class Allergen
    {
        public enum AllergenType {Pähkinä=1,Laktoosi,Gluteeni,Kala};
        static public Dictionary<AllergenType, string> allergenChars = new Dictionary<AllergenType, string>() 
        { { AllergenType.Gluteeni, "G" },{AllergenType.Laktoosi,"L" },{AllergenType.Pähkinä,"P" },{AllergenType.Kala, "K" } };
   
        public static string GetAllergenString(AllergenType type)
        {
            switch (type)
            {
                case AllergenType.Laktoosi:
                    return "laktoosia";
                case AllergenType.Pähkinä:
                    return "pähkinää";
                case AllergenType.Gluteeni:
                    return "Gluteenia";
                case AllergenType.Kala:
                    return "Kalaisa";
                default:
                    return "";
            }

        }
    }
}
