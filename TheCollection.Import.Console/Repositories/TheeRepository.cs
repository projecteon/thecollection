namespace TheCollection.Import.Console.Repositories {
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OleDb;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using TheCollection.Application.Services.Contracts.Repository;
    using TheCollection.Import.Console.Extensions;
    using TheCollection.Import.Console.Models;

    public class TheeRepository : ILinqSearchRepository<Thee> {
        public TheeRepository(string dbPath) {
            DbPath = dbPath;
        }

        public string DbPath { get; }

        private List<Thee> GetThees(string dbPath) {
            var thees = new List<Thee>();
            using (var connection = new OleDbConnection($"Provider=Microsoft.JET.OlEDB.4.0;Data Source={dbPath}")) {
                connection.Open();
                var ds = new DataSet();
                var da = new OleDbDataAdapter("Select * from TheeTotaallijst", connection);
                da.Fill(ds);
                var dt = ds.Tables[0];
                thees = dt.ToList<Thee>();
            }

            return thees;
        }

        public async Task<IEnumerable<Thee>> SearchItemsAsync(Expression<Func<Thee, bool>> predicate = null, int pageSize = 0, int page = 0) {
            return await Task.Run(() => { return GetThees(DbPath); });
        }
    }
}
