using Flock.Classes.SmartFocus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Flock.ViewModels
{
    public class SmartFocusTestViewModelBuilder
    {
        private SmartFocusManager _manager;

        public SmartFocusTestViewModelBuilder(SmartFocusManager Manager)
        {
            _manager = Manager;
        }

        public async Task<SmartFocusTestViewModel> Build()
        {
            SmartFocusTestViewModel Model = new SmartFocusTestViewModel();
            string connectionToken = await _manager.OpenConnection().ConfigureAwait(false);
            Model.TestGroupCollection = new TestGroupCollection();
            Model.TestGroupCollection.TestGroupIDs = await _manager.FetchTestGroupIDs(connectionToken).ConfigureAwait(false);
            Model.TestGroupCollection.Groups = new List<TestGroup>();

            Model.TestMembers = await _manager.GetAllTestMembers(connectionToken).ConfigureAwait(false);

            foreach (long s in Model.TestGroupCollection.TestGroupIDs.Entities)
            {
                Model.TestGroupCollection.Groups.Add(await _manager.FetchTestGroup(s, connectionToken).ConfigureAwait(false));
            }

            Model.Segments = await _manager.GetSegmentList(connectionToken).ConfigureAwait(false);

            await _manager.CloseConnection(connectionToken).ConfigureAwait(false);

            return Model;
        }
    }

    public class SmartFocusTestViewModel
    {
        public TestGroupCollection TestGroupCollection { get; set; }

        public List<TestMember> TestMembers { get; set; }

        public List<Segment> Segments { get; set; }
    }
}