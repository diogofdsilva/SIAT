using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIAT.BL
{
    public class CountriesRoadLimits : Dictionary<string, CountryRoadLimits>
    {

        private CountriesRoadLimits()
        {
        }

        private static CountriesRoadLimits _current;

        public static CountriesRoadLimits Limits
        {
            get
            {
                if (_current == null)
                {
                    _current = new CountriesRoadLimits();
                    Load();
                }
                return _current;
            }
        }



        private static void Load()
        {
            _current.Add("PT",new CountryRoadLimits("PT", "Portugal"));
        }

    }

    public class CountryRoadLimits : Dictionary<string , byte>
    {
        private string _code;
        private string _name;

        public CountryRoadLimits(string code, string name) : base()
        {
            _code = code;
            _name = name;
        }

        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public byte this[string code]
        { 
            get
            {
                byte b = 40;

                try
                {
                    b = base[code];
                }
                catch (KeyNotFoundException exception)
                {
                }

                return b;
            }
        }
    }
}
