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
    [HubName("MemberAddressHub")]
    public class MemberAddressHub : Hub
    {
        private readonly MemberAddressPusher _memberAddressTicker;

        public MemberAddressHub() : this(MemberAddressPusher.Instance) { }

        public MemberAddressHub(MemberAddressPusher maTicker)
        {
            _memberAddressTicker = maTicker;
        }
    }

    public class MemberAddressPusher
    {
        // Singleton instance
        private readonly static Lazy<MemberAddressPusher> _instance = new Lazy<MemberAddressPusher>(() => new MemberAddressPusher(GlobalHost.ConnectionManager.GetHubContext<MemberAddressHub>().Clients));

        private MemberAddressPusher(IHubConnectionContext clients)
        {
            Clients = clients;
        }

        public static MemberAddressPusher Instance
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

        public void BroadcastMemberAddress(MemberAddress address)
        {
            Clients.All.updateMemberAddress(address);
        }
    }
}