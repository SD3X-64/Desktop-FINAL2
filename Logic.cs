using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Desktop_FINAL2.DatabaseInitializer;

namespace Desktop_FINAL2
{
    public class Logic
    {
        public static List<Anime> LoadTitles(string ConnectionString)
        {
            var list = new List<Anime>();
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            string sql = @"
                SELECT Id, Title, Studio, PrimaryGenre, YearStarted, Episodes, Status, WatchStatus, LastWatched
                FROM Titles
                ORDER BY Title";
            using var cmd = new SqlCommand(sql, connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Anime
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    StudioName = reader.GetString(2),
                    GenreName = reader.GetString(3),
                    YearStarted = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
                    Episodes = reader.IsDBNull(5) ? null : reader.GetInt32(5),
                    Status = reader.IsDBNull(6) ? null : reader.GetString(6),
                    WatchStatus = reader.IsDBNull(7) ? null : reader.GetString(7),
                    LastWatched = reader.IsDBNull(8) ? null : reader.GetInt32(8)
                });
            }
            return list;
        }

        public static List<Studio> LoadStudios(string ConnectionString)
        {
            var list = new List<Studio>();
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            string sql = "SELECT Id, Name FROM Studios ORDER BY Name";
            using var cmd = new SqlCommand(sql, connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Studio { Id = reader.GetInt32(0), Name = reader.GetString(1) });
            }
            return list;
        }

        public static List<Genre> LoadGenres(string ConnectionString)
        {
            var list = new List<Genre>();
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            string sql = "SELECT Id, Name FROM Genres ORDER BY Name";
            using var cmd = new SqlCommand(sql, connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Genre { Id = reader.GetInt32(0), Name = reader.GetString(1) });
            }
            return list;
        }
    }
}
