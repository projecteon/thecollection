﻿using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using TheCollection.Import.Console.Models;

namespace TheCollection.Import.Console
{
    public class AccessExport
    {
        public static List<Merk> GetMeerken(string dbPath)
        {
            var meerkens = new List<Merk>();
            using (OleDbConnection connection = new OleDbConnection($"Provider=Microsoft.JET.OlEDB.4.0;Data Source={dbPath}"))
            {
                connection.Open();
                DataSet ds = new DataSet();
                OleDbDataAdapter da = new OleDbDataAdapter("Select * from tblTheeMerken", connection);
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                meerkens = dt.ToList<Merk>();
            }
            
            return meerkens;
        }

        public static List<Thee> GetThees(string dbPath)
        {
            var thees = new List<Thee>();
            using (OleDbConnection connection = new OleDbConnection($"Provider=Microsoft.JET.OlEDB.4.0;Data Source={dbPath}"))
            {
                connection.Open();
                DataSet ds = new DataSet();
                OleDbDataAdapter da = new OleDbDataAdapter("Select * from TheeTotaallijst", connection);
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                thees = dt.ToList<Thee>();
            }

            return thees;
        }
    }
}