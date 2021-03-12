namespace CvOnlineCrawler
{
    using System.Collections.Generic;
    public static class Dictionaries
    {
        private static string[] workAreaArray =
        {
            "administravimas-sekretoriavimas",
            "apsauga-gelbejimo-paslaugos",
            "bankai-draudimas",
            "elektronika-telekomunikacijos",
            "energetika",
            "farmacija",
            "finansai-apskaita",
            "gamyba-pramone",
            "informacines-technologijos",
            "inzinerija",
            "klientu-aptarnavimas-paslaugos",
            "kokybes-vadyba-kokybes-kontrole",
            "kultura-menas-pramogos-sportas",
            "medicina-socialine-rupyba",
            "miskininkyste-medienos-apdirbimas",
            "organizavimas-valdymas",
            "pardavimai",
            "prekyba-pirkimai-tiekimas",
            "rinkodara-reklama",
            "savanoriskas-darbas",
            "statyba-nekilnojamas-turtas",
            "svietimas-mokslas-mokymai",
            "teise",
            "transportas-logistika",
            "trumpalaikis-sezoninis-darbas",
            "turizmas-viesbuciai-viesasis-maitinimas",
            "valstybinis-ir-viesasis-administravimas",
            "zemes-ukis-aplinkos-inzinerija",
            "ziniasklaida-viesieji-rysiai",
            "zmogiskieji-istekliai",
        };
       
        public static string GetWorkArea(string workAreaWord)
        {
            string workArea = null;
            foreach(string work in workAreaArray)
            {
                if(work.Contains(workAreaWord))
                {
                    workArea = work;
                    break;
                }
            }

            return workArea;
        }
      
        public static Dictionary<string, string> Location = new Dictionary<string, string>
        {
            ["Vilnius"] = "vilniaus",
            ["Kaunas"] = "kauno",
            ["Klaipėda"] = "klaipedos" 
        };
    }
}
