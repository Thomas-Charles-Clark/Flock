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
    [HubName("VisitorHub")]
    public class VisitorHub : Hub
    {
        private readonly VisitorPusher _visitorPusher;

        public VisitorHub() : this(VisitorPusher.Instance) { }

        public VisitorHub(VisitorPusher vPusher)
        {
            _visitorPusher = vPusher;
        }
    }

    public class VisitorPusher
    {
        // Singleton instance
        private readonly static Lazy<VisitorPusher> _instance = new Lazy<VisitorPusher>(() => new VisitorPusher(GlobalHost.ConnectionManager.GetHubContext<VisitorHub>().Clients));

        private VisitorPusher(IHubConnectionContext clients)
        {
            Clients = clients;
        }

        public static VisitorPusher Instance
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

        public void BroadcastVisitor(int memberID)
        {
            Clients.All.updateVisitor(memberID);
        }
    }
}