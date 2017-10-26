using System.ComponentModel.DataAnnotations;

namespace TheCollection.Web.Models.AccountViewModels {

    public class ExternalLoginConfirmationViewModel {

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
