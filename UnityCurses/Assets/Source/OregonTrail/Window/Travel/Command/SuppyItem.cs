// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

namespace Assets.Source.OregonTrail.Window.Travel.Command
{
    public class SuppyItem
    {
        private readonly string _amountPretty;
        private readonly string _name;

        public SuppyItem(string name, string amountPretty)
        {
            _name = name;
            _amountPretty = amountPretty;
        }

        public string Name
        {
            get { return _name; }
        }

        public string AmountPretty
        {
            get { return _amountPretty; }
        }
    }
}