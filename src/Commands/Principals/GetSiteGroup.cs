﻿using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.Model;
using PnP.PowerShell.Commands.Base.PipeBinds;
using Microsoft.Online.SharePoint.TenantAdministration;
using PnP.PowerShell.Commands.Base;

namespace PnP.PowerShell.Commands.Principals
{
    [Cmdlet(VerbsCommon.Get, "SiteGroup", DefaultParameterSetName = "All")]
    public class GetSiteGroup : PnPAdminCmdlet
    {
        [Parameter(Mandatory = false)]
        public string Site;

        [Parameter(Mandatory = false)]
        public string Group;


        protected override void ExecuteCmdlet()
        {
            var url = PnPConnection.CurrentConnection.Url;
            if (ParameterSpecified(nameof(Site)))
            {
                url = Site;
            }
            var site = this.Tenant.GetSiteByUrl(url);
            if (!ParameterSpecified(nameof(Group)))
            {
                var groups = ClientContext.LoadQuery(site.RootWeb.SiteGroups.IncludeWithDefaultProperties(g => g.Users, g => g.Title, g => g.OwnerTitle, g => g.Owner.LoginName, g => g.LoginName));
                ClientContext.ExecuteQueryRetry();
                foreach (var group in groups)
                {
                    var siteGroup = new SiteGroup(ClientContext, Tenant, group, site.RootWeb);
                    WriteObject(siteGroup);
                }
            }
            else
            {
                var group = site.RootWeb.SiteGroups.GetByName(Group);
                ClientContext.Load(group, g => g.Users, g => g.Title, g => g.OwnerTitle, g => g.Owner.LoginName, g => g.LoginName);
                ClientContext.ExecuteQueryRetry();
                var siteGroup = new SiteGroup(ClientContext, Tenant, group, site.RootWeb);
                WriteObject(siteGroup);
            }
        }
    }



}
