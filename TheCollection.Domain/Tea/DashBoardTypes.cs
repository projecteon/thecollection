namespace TheCollection.Domain.Tea {
    using System;
    using Constant;
    using Constant.JsonConverter;
    using Newtonsoft.Json;

    [JsonConverter(typeof(ConstantConverter<Guid, DashBoardTypes>))]
    public class DashBoardTypes : Constant<Guid, DashBoardTypes> {
        public static DashBoardTypes TotalBagsCountByPeriod = new DashBoardTypes(Guid.Parse("f08db9e3-2675-4973-a1fe-f5cd62f3dc20"));
        public static DashBoardTypes BagsCountByPeriod = new DashBoardTypes(Guid.Parse("10250ca5-cff3-4149-8613-c9c2068b81ac"));
        public static DashBoardTypes BagsCountByBagTypes = new DashBoardTypes(Guid.Parse("37a3592f-e614-45e8-9903-45135aea9dd4"));
        public static DashBoardTypes BagsCountByBrands = new DashBoardTypes(Guid.Parse("00ac062d-68f4-4c6e-9a60-044bf8c9077c"));

        protected DashBoardTypes(Guid key) : base(key) {
        }
    }
}
