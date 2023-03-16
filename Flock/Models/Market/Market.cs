using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.Models
{
    public class MarketModel
    {
        public int ID { get; set; }

        public string Name { get; set; }


        public static IEnumerable<Market> GetMarkets()
        {
            Flock.Models.FlockDataContext db = new Flock.Models.FlockDataContext();

            return from market in db.Markets select market;
        }
    }
}