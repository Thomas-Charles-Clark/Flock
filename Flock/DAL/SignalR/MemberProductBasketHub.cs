using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Collections.Concurrent;
using Flock.Models;
using System.Timers;

namespace Flock.DAL.SignalR
{
    [HubName("MemberBasketHub")]
    public class MemberBasketHub : Hub
    {
        private readonly MemberBasketPusher _memberBasketPusher;

        public MemberBasketHub() : this(MemberBasketPusher.Instance) { }

        public MemberBasketHub(MemberBasketPusher maPusher)
        {
            _memberBasketPusher = maPusher;
        }
    }

    public class MemberBasketPusher
    {
        // Singleton instance
        private readonly static Lazy<MemberBasketPusher> _instance = new Lazy<MemberBasketPusher>(() => new MemberBasketPusher(GlobalHost.ConnectionManager.GetHubContext<MemberBasketHub>().Clients));

        private MemberBasketPusher(IHubConnectionContext clients)
        {
            Clients = clients;
        }

        public static MemberBasketPusher Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private IHubConnectionContext Clients
        {
            get;
            set;
        }

        public void BroadcastMemberBasket(int memberID)
        {
            Clients.All.updateMemberBasket(memberID);
        }
    }
}