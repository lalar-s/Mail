using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using RSSreader.Models;
using RSSreader.Services;

namespace RSSreader.Controllers
{
    public class HomeController : Controller
    {
        RepositoryService repositoryService;
        RSSService rssService;
        public HomeController()
        {
            repositoryService = new RepositoryService();
            rssService = new RSSService();
        }

        public IActionResult Index(ManagerModel model)
        {
            return View(model);
        }

        [HttpPost]
        public ActionResult SaveRSS(ManagerModel model)
        {
            Subscription subscription = new Subscription
            {
                Email = model.Email,
                RSSlink = model.RSSLink
            };

            
            repositoryService.SaveSubscription(subscription);
            return View("Index", model);
        }
        [HttpPost]
        public ActionResult DeleteRSSLink(ManagerModel model)
        {
            repositoryService.RemoveSubscription(model.SelectedSubscriptions);
            string userEmail = model.Email;
            List<Subscription> subscriptions = repositoryService.GetSubscriptions(userEmail);
            model.Subscriptions = new SelectList(subscriptions, "Id", "RSSlink");
            return View("Index", model);
        }

        [HttpPost]
        public async Task<ActionResult> DownloadRSS(ManagerModel model)
        {
            var subscriptions = repositoryService.GetSubscriptions(model.Email);
            model.Subscriptions = new SelectList(subscriptions, "Id", "RSSlink");
            var feedItems = await rssService.SubscriptionsToFeedItems(subscriptions);
            model.EmailContent = rssService.GetEmailContentFromFeedItems(feedItems);
            return View("Index", model);
        }

        [HttpPost]
        public async Task<ActionResult> SendEmail(ManagerModel model)
        {
            var subscriptions = repositoryService.GetSubscriptions(model.Email);
            var feedItems = await rssService.SubscriptionsToFeedItems(subscriptions);
            model.EmailContent = rssService.GetEmailContentFromFeedItems(feedItems);
            var sender = new SendGridSenderService();
            var result = await sender.SendEmail(model.Email, "RSS", model.EmailContent);
            return View("Index", model);
        }
    }
}
