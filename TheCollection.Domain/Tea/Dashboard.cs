namespace TheCollection.Domain.Tea {
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    public class Dashboard<T> {
        public Dashboard(string id, string userId, DashBoardTypes dashBoardType, T data) {
            Id = id;
            UserId = userId;
            DashboardType = dashBoardType;
            Data = data;
        }

        [Key]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; }

        [JsonProperty(PropertyName = "userid")]
        public string UserId { get; }

        [JsonProperty(PropertyName = "dashboardtype")]
        public DashBoardTypes DashboardType { get; }

        [JsonProperty(PropertyName = "data")]
        public T Data { get; }
    }
}
