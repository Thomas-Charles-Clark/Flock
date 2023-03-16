using Flock.Classes.SmartFocus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Flock.ViewModels
{
    public class SFTransactionalTemplateOverviewViewModelBuilder
    {
        private SmartFocusManager _manager;

        public SFTransactionalTemplateOverviewViewModelBuilder(SmartFocusManager Manager)
        {
            _manager = Manager;
        }

        public async Task<SFTransactionalTemplateOverviewViewModel> Build()
        {
            SFTransactionalTemplateOverviewViewModel Model = new SFTransactionalTemplateOverviewViewModel();

            string connectionToken = await _manager.OpenConnection(_manager.TransactionalTemplateServer).ConfigureAwait(false);

            Model.Templates = await _manager.GetLastTransactionalTemplates(connectionToken);
            Model.Banners = await _manager.GetLastDynamicContentBlocks(connectionToken);

            await _manager.CloseConnection(connectionToken, _manager.TransactionalTemplateServer).ConfigureAwait(false);

            return Model;
        }

    }

    public class SFTransactionalTemplateOverviewViewModel : BaseViewModel
    {
        public List<APITemplate> Templates { get; set; }
        public List<DynamicContentBlock> Banners { get; set; }
    }
}