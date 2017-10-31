namespace TheCollection.Import.Console.Repositories {
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OleDb;
    using System.Threading.Tasks;
    using TheCollection.Business;
    using TheCollection.Import.Console.Extensions;
    using TheCollection.Import.Console.Models;

    public class MerkRepository : ILinqSearchRepository<Merk> {
        public MerkRepository(string dbPath) {
            DbPath = dbPath;
        }

        public string DbPath { get; }

        public async Task<IEnumerable<Merk>> SearchItemsAsync(System.Linq.Expressions.Expression<System.Func<Merk, bool>> predicate = null, int pageSize = 0, int page = 0) {
            return await Task.Run(() => { return GetMeerken(DbPath); });
        }

        private List<Merk> GetMeerken(string dbPath) {
            var meerkens = new List<Merk>();
            using (var connection = new OleDbConnection($"Provider=Microsoft.JET.OlEDB.4.0;Data Source={dbPath}")) {
                connection.Open();
                var ds = new DataSet();
                var da = new OleDbDataAdapter("Select * from tblTheeMerken", connection);
                da.Fill(ds);
                var dt = ds.Tables[0];
                meerkens = dt.ToList<Merk>();
            }

            return meerkens;
        }
    }
}
