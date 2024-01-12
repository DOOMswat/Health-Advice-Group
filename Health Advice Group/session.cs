using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Advice_Group
{
    internal class session
    {
        //API KEY = daf5fcbcd2384c6eb9791539232311
        public static string request = "http://api.weatherapi.com/v1/current.json?key=daf5fcbcd2384c6eb9791539232311&q=Leeds&aqi=no";
        public static string connStr = "server=ND-COMPSCI;" + "user=TL_S2101550;" + "database=tl_s2101550_hag;" + "port=3306;" + "password=Notre260605";
        public static string userID = "";
        public static string postCode = "";
        public static string fistName = "";
        public static string LastName = "";
        public static string Email = "";
        

    }
}
