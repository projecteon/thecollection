namespace TheCollection.Import.Console.Repositories {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using TheCollection.Application.Services.Contracts.Repository;
    using TheCollection.Import.Console.Models;

    public class TheeJsonRepository : ILinqSearchRepository<Thee> {
        public TheeJsonRepository(string filepath) {
            Filepath = filepath;
        }

        public string Filepath { get; }

        public async Task<IEnumerable<Thee>> SearchItemsAsync(Expression<Func<Thee, bool>> predicate = null, int pageSize = 0, int page = 0) {
            return await Task.Run(() => { return GetThees(); });
        }

        List<Thee> GetThees() {
            var jsonContent2 = ReadFile(Filepath);
            return JsonConvert.DeserializeObject<Thees>(jsonContent2).TheeTotaallijst;
        }

        string ReadFile(string filename) {
            try {
                using (var reader = File.OpenText(filename)) {
                    var fileContent = reader.ReadToEnd();
                    if (fileContent != null && fileContent != "") {
                        return fileContent;
                    }
                }
            }
            catch (Exception ex) {
                //Log
                throw ex;
            }
            return "";
        }
    }
}
