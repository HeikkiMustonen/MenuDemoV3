using System;
using System.Collections.Generic;
using System.Text;

namespace MenuDemoV3ClassLibrary
{
    public class Drink:Dish
    {
        private float _sizeInSentiliters;
        private bool _IsAlcoholic;
        private float _vol;

        public Drink()
        {

        }

        public Drink(string name, float size = 0F, bool isAlcoholic=false,float vol=0):base(name)
        {
            this.IsAlcoholic = isAlcoholic;
            this.Vol = vol;
        }
        /// <summary>
        /// How much there is alcohol in this drink
        /// </summary>
        public float Vol
        {
            get { return _vol; }
            set { _vol = value; }
        }


        public bool IsAlcoholic
        {
            get { 
                return _vol > 0; }
            set { _IsAlcoholic = value; }
        }


        public float SizeInSentiliters
        {
            get { return _sizeInSentiliters; }
            set { _sizeInSentiliters = value; }
        }

    }
}
