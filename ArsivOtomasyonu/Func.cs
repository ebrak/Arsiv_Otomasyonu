using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsivOtomasyonu
{
    class Func
    {
        public static string connectionString = "Server=localhost;Database=arsiv;Uid=root;Pwd=;Encrypt=false;";

        //public static string connectionString = "Server=localhost;Database=arsiv_odev;Uid=root;Pwd=;Encrypt=false;";
        public bool yetki;
        
        public static void CikisYap()
        {
            Properties.Settings.Default.nick = "";
            Properties.Settings.Default.Save();
        }


    }
}
