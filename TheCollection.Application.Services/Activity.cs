namespace TheCollection.Application.Services {
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Newtonsoft.Json;
    using TheCollection.Application.Services.Contracts;

    public class Activity : IActivity {
        [Key]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "validroles")]
        public IEnumerable<IRole> ValidRoles { get; set; }

        public Activity AddRole(IRole newRole) {
            var newRoles = new List<IRole> { newRole };
            newRoles.AddRange(ValidRoles);
            return new Activity { Id = Id, Name = Name, ValidRoles = newRoles };
        }

        public Activity RemoveRole(IRole removeRole) {
            var newRoles = ValidRoles.Where(x => x.Id != removeRole.Id);
            return new Activity { Id = Id, Name = Name, ValidRoles = newRoles };
        }
    }
}
