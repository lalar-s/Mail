using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RSSreader.Models
{
    public class ManagerModel
    {
        public ManagerModel()
        {
            SelectedSubscriptions = new List<int>();
            Subscriptions = new SelectList(new List<Subscription>(), "Id", "RSSlink");
        }
        [Display(Name = "RSS")]
        public string RSSLink { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string EmailContent { get; set; }

        public SelectList Subscriptions { get; set; }

        public IEnumerable <int> SelectedSubscriptions { get; set; }
    }
}
