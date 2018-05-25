namespace TheCollection.Presentation.Web.Models {
    using System.Collections.Generic;
    using System.Linq;
    using AspNetCore.Identity.DocumentDb;
    using TheCollection.Application.Services.Contracts;

    public class WebUser : DocumentDbIdentityUser<WebRole>, IApplicationUser {
        public WebUser() {
            Contacts = new List<Domain.RefValue>();
        }

        public WebUser(ICollection<Domain.RefValue> contacts) {
            Contacts = contacts;
        }

        public ICollection<Domain.RefValue> Contacts { get; }

        IEnumerable<IRole> IApplicationUser.Roles { get { return Roles.Cast<IRole>(); } }

        //IEnumerable<IRole> IApplicationUser.Roles { get { return Roles.Select(x => new WebRole(x.Id, x.Name)); } }
    }
    public class WebRole : DocumentDbIdentityRole, IRole {
    }
}
