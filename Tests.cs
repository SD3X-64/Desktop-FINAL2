using Avalonia.Interactivity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Desktop_FINAL2.DatabaseInitializer;

namespace Desktop_FINAL2
{
    internal class Tests
    {
        public static bool IsDatabaseReady(string ConnectionString)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            string checkTableSql = "SELECT COUNT(*) FROM sys.tables WHERE name = 'Titles'";
            using var checkCmd = new SqlCommand(checkTableSql, connection);
            int tableExists = (int)checkCmd.ExecuteScalar();
            if (tableExists == 0) return false;

            string sql = "SELECT COUNT(*) FROM Titles";
            using var command = new SqlCommand(sql, connection);
            int count = (int)command.ExecuteScalar();
            return count > 0;
        }
    }
}
