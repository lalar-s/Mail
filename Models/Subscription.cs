using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSSreader.Models
{
    public class Subscription
    {
        public string Email { get; set; }

        public string RSSlink { get; set; }

        public int Id { get; set; }
    }
}
