namespace CvOnlineCrawler
{
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            Crawler crawler = new Crawler();

            crawler.Salary = "500";
            crawler.Location = "Vilnius";
            crawler.UploadPeriod = "7";
            crawler.WorkArea = "medicina-socialine-rupyba";

            if(crawler.StartCrawler("psicholog"))
            {
                Console.WriteLine("Finished succesfuly");
            }
        }
    }
}
