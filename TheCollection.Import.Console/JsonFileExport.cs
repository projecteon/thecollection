using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using TheCollection.Import.Console.Models;

namespace TheCollection.Import.Console
{
    public class JsonFileExport
    {
        public static List<Merk> GetMeerken(string filename)
        {
            var jsonContent2 = ReadFile(filename);
            return JsonConvert.DeserializeObject<Merkens>(jsonContent2).tblTheeMerken;
        }

        public static List<Thee> GetThees(string filename)
        {
            var jsonContent2 = ReadFile(filename);
            return JsonConvert.DeserializeObject<Thees>(jsonContent2).TheeTotaallijst;
        }

        public static string ReadFile(string filename)
        {
            try
            {
                using (StreamReader reader = File.OpenText(filename))
                {
                    string fileContent = reader.ReadToEnd();
                    if (fileContent != null && fileContent != "")
                    {
                        return fileContent;
                    }
                }
            }
            catch (Exception ex)
            {
                //Log
                throw ex;
            }
            return "";
        }
    }
}
