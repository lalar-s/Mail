using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using RSSreader.Models;

namespace RSSreader.Services
{
    public class RSSService
    {
        public async Task<List<FeedItem>> GetRSSFromUrl(string feedUrl)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(feedUrl);
                    var responseMessage = await client.GetAsync(feedUrl);
                    var responseString = await responseMessage.Content.ReadAsStringAsync();

                    XDocument doc = XDocument.Parse(responseString);
                    var feedItems = from item in doc.Root.Descendants().First(i => i.Name.LocalName == "channel").Elements().Where(i => i.Name.LocalName == "item")
                                    select new FeedItem
                                    {
                                        Content = item.Elements().First(i => i.Name.LocalName == "description").Value,
                                        Link = item.Elements().First(i => i.Name.LocalName == "link").Value,
                                        PublishDate = ParseDate(item.Elements().First(i => i.Name.LocalName == "pubDate").Value),
                                        Title = item.Elements().First(i => i.Name.LocalName == "title").Value
                                    };
                    return feedItems.ToList();
                }
            }
            catch
            {
                return null;
            }
        }
        private DateTime ParseDate(string date)
        {
            DateTime result;
            if (DateTime.TryParse(date, out result))
                return result;
            else
                return DateTime.MinValue;
        }
        public async Task<List<FeedItem>> SubscriptionsToFeedItems(List<Subscription> subscriptionsList)
        {
            var feedItems = new List<FeedItem>();
            foreach (var subscription in subscriptionsList)
            {
                var rssLink = subscription.RSSlink;
                var xd = await GetRSSFromUrl(rssLink);
                if (xd != null)
                {
                    feedItems.AddRange(xd);
                }

            }
            return feedItems;
        }

        public string GetEmailContentFromFeedItems(List<FeedItem> feedItems)
        {
            string rssContent = "";
            foreach (var feedItem in feedItems)
            {
                rssContent += feedItem.Title + "\n" + feedItem.PublishDate + "\n" + "\n" + feedItem.Content + "\n" + "\n" + "\n";
            }
            return rssContent;
        }
    }
}
