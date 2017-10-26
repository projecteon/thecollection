using AspNetCore.Identity.DocumentDb;

namespace TheCollection.Web.Models {

    public class ApplicationUser : DocumentDbIdentityUser<DocumentDbIdentityRole> {
    }
}
