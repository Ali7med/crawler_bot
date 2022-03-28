using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TelegramBolt.classes
{
      class WebsiteClass
    { 
        public string? Title { get; set; }
        public string? Auther { get; set; }
        public string? Keywords { get; set; }
        public string? Description { get; set; }
        public string? URL { get; set; }
        public WebsiteClass()
        {
        }
        public WebsiteClass(string url)
        {
            URL = url;
        }
       public void FetchMetaTags_Async(string address="")
        {
            if(address!="") URL = address;
            Title = "none";
            Auther = "none";
            Keywords = "none";
            Description = "none";
            if (address.Contains("https://") == false) URL = "https://" + URL;
            String strUrl = URL;
            try
            {
                //To Get Title
                WebClient x = new WebClient();
                string pageSource = x.DownloadString(strUrl);
                Title = Regex.Match(pageSource, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase).Groups["Title"].Value;
                //Creating a method to get Meta Tags like description and keyword meta tags
                GetMetaTagsDetailsAsync(strUrl);
            }
            catch (Exception ex)
            {
               
            }
        }
        void GetMetaTagsDetailsAsync(string strUrl)
        {
            //Get Meta Tags
            var webGet = new HtmlWeb();
            var document = webGet.Load(strUrl);
            var metaTags = document.DocumentNode.SelectNodes("//meta");
            
            if (metaTags != null)
            {
                foreach (var tag in metaTags)
                {
                    if (tag.Attributes["name"] != null && tag.Attributes["content"] != null && tag.Attributes["name"].Value == "description")
                    {
                        Description = tag.Attributes["content"].Value;
                        byte[] bytes = Encoding.Default.GetBytes(Description);
                        Description = Encoding.UTF8.GetString(bytes);
                    }
                    if (tag.Attributes["name"] != null && tag.Attributes["content"] != null && tag.Attributes["name"].Value == "auther")
                    {
                        Auther = tag.Attributes["content"].Value;
                        byte[] bytes = Encoding.Default.GetBytes(Auther);
                        Auther = Encoding.UTF8.GetString(bytes);
                    }
                    if (tag.Attributes["name"] != null && tag.Attributes["content"] != null && tag.Attributes["name"].Value == "keywords")
                    {
                        Keywords = tag.Attributes["content"].Value;
                        byte[] bytes = Encoding.Default.GetBytes(Keywords);
                        Keywords = Encoding.UTF8.GetString(bytes);
                    }
                }
            }
        }
            public bool RemoteFileExists(string url)
        {
           
                bool br = false;
                try
                {
                    IPHostEntry ipHost = Dns.Resolve(url);
                    br = true;
                }
                catch (SocketException se)
                {
                    br = false;
                }
                return br;
            
        }
        
        
        public List<TempData> getDataWether(string city)
        { 
            var html = city;

            HtmlWeb web = new HtmlWeb();

            var htmlDoc = web.Load(html);
            List<TempData> temps = new List<TempData>();
            var htmlBody = htmlDoc.DocumentNode.SelectSingleNode(".");
            if(htmlBody.Name != "#document")
            foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes("//div[@class='forecast-hour']"))
            {
                string value = node.InnerText;

                foreach (HtmlNode node1 in node.SelectNodes("//div[@class='row']"))
                {
                
                    var _tempData = new TempData();
                    _tempData.timecell = node1.ChildNodes[1].ChildNodes[1].InnerHtml + " " + node1.ChildNodes[1].ChildNodes[3].InnerHtml;
                    _tempData.iconPath = node1.ChildNodes[3].InnerHtml;
                    _tempData.temperature = node1.ChildNodes[5].ChildNodes[1].InnerHtml + " " + node1.ChildNodes[5].ChildNodes[3].InnerHtml;
                    _tempData.wind = node1.ChildNodes[7].ChildNodes[1].InnerHtml + " " + node1.ChildNodes[7].ChildNodes[3].InnerHtml;
                    _tempData.precipitation = node1.ChildNodes[9].ChildNodes[1].InnerHtml + " " + node1.ChildNodes[9].ChildNodes[3].InnerHtml;
                    _tempData.pressure = node1.ChildNodes[11].ChildNodes[1].InnerHtml + " " + node1.ChildNodes[11].ChildNodes[3].InnerHtml;
                    temps.Add(_tempData);
                } 
            } 
            return temps; 

        }


    } public class TempData
    {
        public string timecell { get; set; }
        public string iconPath { get; set; }
        public string temperature { get; set; }
        public string wind { get; set; }
        public string precipitation { get; set; }
        public string pressure { get; set; } 
        public int Time
        {
            get
            {
                return Convert.ToInt32(this.timecell.Substring(0, 4));
            }
        }
        public int Date
        {
            get
            {
                return Convert.ToInt32(this.timecell.Substring(4, 2));
            }
        } 
    }

}
