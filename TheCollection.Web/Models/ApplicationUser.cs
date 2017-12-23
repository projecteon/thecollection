namespace TheCollection.Web.Models {
    using System.Collections.Generic;
    using AspNetCore.Identity.DocumentDb;
    using TheCollection.Web.Contracts;

    public class ApplicationUser : DocumentDbIdentityUser<DocumentDbIdentityRole>, IApplicationUser {
        public ApplicationUser() {
            Contacts = new List<Domain.RefValue>();
        }
        public ApplicationUser(ICollection<Domain.RefValue> contacts) {
            Contacts = contacts;
        }

        public ICollection<Domain.RefValue> Contacts { get; }
    }
}
