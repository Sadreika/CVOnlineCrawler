namespace CvOnlineCrawler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Text.RegularExpressions;
    using RestSharp;
    
    public class Crawler
    {
        public string Salary { get; set; }
        public string WorkArea { get; set; }
        public string Location { get; set; }
        public string UploadPeriod { get; set; }

        private RestClient Client = new RestClient();

        public Crawler()
        {
            Client.AddDefaultHeader("Host", "www.cvonline.lt");
            Client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:85.0) Gecko/20100101 Firefox/85.0";
            Client.AddDefaultHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            Client.AddDefaultHeader("Accept-Language", "en-GB,en;q=0.5");
            Client.AddDefaultHeader("DNT", "1");
            Client.AddDefaultHeader("Connection", "keep-alive");
            Client.AddDefaultHeader("Upgrade-Insecure-Requests", "1");
        }

        public bool StartCrawler(string searchWords)
        {
            string[] wordsToSearch = searchWords.Split('|');

            if(TryExtractJobsUrls(out List<string> jobsUrlList))
            {
                Console.WriteLine("Jobs urls extracted");

                if (TryExtractPossibleJobs(wordsToSearch, jobsUrlList, out List<string> possibleJobsList))
                {
                    Console.WriteLine("Extracted possible jobs offers");

                    SendList(possibleJobsList);
                }
            }

            return true;
        }

        private string LoadPage(string pageNumber)
        {
            Client.BaseUrl = new Uri(FormUrl(pageNumber), UriKind.Absolute);
            RestRequest request = new RestRequest("", Method.GET);
            IRestResponse response = Client.Execute(request);

            Console.WriteLine(Client.BaseUrl + " " + response.StatusCode);

            return response.Content;
        }

        private string LoadJob(string url)
        {
            Client.BaseUrl = new Uri("https:" + url, UriKind.Absolute);
            RestRequest request = new RestRequest("", Method.GET);
            IRestResponse response = Client.Execute(request);

            Console.WriteLine(Client.BaseUrl + " " + response.StatusCode);

            return response.Content;
        }

        private string FormUrl(string pageNumber)
        {
            return Urls.PageUrl + $"{UploadPeriod}d/{Dictionaries.GetWorkArea(WorkArea)}/ms{Salary}/{Dictionaries.Location[Location]}?page={pageNumber}";
        }

        private bool TryExtractUrls(string pageBody, out List<string> jobUrlList)
        {
            jobUrlList = RegexFunctions.RegexToStringArray(pageBody, Regexes.url).ToList();

            return jobUrlList.Count != 0;
        }

        private bool TryExtractJobsUrls(out List<string> jobsUrlList)
        {
            jobsUrlList = new List<string>();

            int pageNumber = 0;

            do
            {
                string pageBody = pageNumber == 0
                    ? LoadPage(pageNumber: "")
                    : LoadPage(pageNumber: pageNumber.ToString());

                if (TryExtractUrls(pageBody, out List<string> jobUrlList))
                {
                    jobsUrlList.AddRange(jobUrlList);
                }
                else
                {
                    break;
                }

                pageNumber += 1;
            }
            while (true);

            if (jobsUrlList.Count == 0)
            {
                return false;
            }

            jobsUrlList = jobsUrlList.Distinct().ToList();

            return true;
        }

        private bool TryExtractPossibleJobs(string[] wordsToSearch, List<string> jobsUrlList, out List<string> possibleJobsList)
        {
            possibleJobsList = new List<string>();

            foreach (string url in jobsUrlList)
            {
                string jobContent = LoadJob(url);

                foreach (string word in wordsToSearch)
                {
                    if (Regex.IsMatch(jobContent, word))
                    {
                        possibleJobsList.Add(url);
                        break;
                    }
                }
            }

            if(possibleJobsList.Count == 0)
            {
                return false;
            }

            return true;
        }
  
        private bool SendList(List<string> possibleJobsList)
        {
            string messageBody = string.Empty;

            foreach(string possibleJob in possibleJobsList)
            {
                messageBody += $"{possibleJob}\n";
            }

            try
            {
                var fromAddress = new MailAddress("", "");
                var toAddress = new MailAddress("", "");
                const string fromPassword = "";
                const string subject = "";
                string body = messageBody;

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                    Console.WriteLine("Sent");
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
