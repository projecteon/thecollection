namespace TheCollection.Import.Console.Repositories {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using TheCollection.Application.Services.Contracts.Repository;
    using TheCollection.Import.Console.Models;

    public class MerkJsonRepository : ILinqSearchRepository<Merk> {
        public MerkJsonRepository(string filepath) {
            Filepath = filepath;
        }

        public string Filepath { get; }

        public async Task<IEnumerable<Merk>> SearchItemsAsync(Expression<Func<Merk, bool>> predicate = null, int pageSize = 0, int page = 0) {
            return await Task.Run(() => { return GetMeerken(); });
        }

        List<Merk> GetMeerken() {
            var jsonContent2 = ReadFile(Filepath);
            return JsonConvert.DeserializeObject<Merkens>(jsonContent2).tblTheeMerken;
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
