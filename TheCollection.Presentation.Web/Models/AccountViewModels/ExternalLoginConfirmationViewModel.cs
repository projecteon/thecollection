using System.ComponentModel.DataAnnotations;

namespace TheCollection.Presentation.Web.Models.AccountViewModels {

    public class ExternalLoginConfirmationViewModel {

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
