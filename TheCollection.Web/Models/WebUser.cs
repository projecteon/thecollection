namespace TheCollection.Web.Models {
    using System.Collections.Generic;
    using System.Linq;
    using AspNetCore.Identity.DocumentDb;
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Web.Contracts;

    public class WebUser : DocumentDbIdentityUser<DocumentDbIdentityRole>, IWebUser {
        public WebUser() {
            Contacts = new List<Domain.RefValue>();
        }

        public WebUser(ICollection<Domain.RefValue> contacts) {
            Contacts = contacts;
        }

        public ICollection<Domain.RefValue> Contacts { get; }

        IEnumerable<IRole> IApplicationUser.Roles { get { return Roles.Cast<IRole>(); } }
    }
}
