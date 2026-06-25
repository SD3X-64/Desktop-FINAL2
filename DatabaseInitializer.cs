using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.SqlServer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop_FINAL2
{
    public class DatabaseInitializer
    {
        public class Anime
        {
            [Key]
            public int Id { get; set; }

            [Required]
            [MaxLength(200)]
            public string Title { get; set; } = string.Empty;

            [Required]
            [MaxLength(200)]
            public string StudioName { get; set; } = string.Empty;

            [Required]
            [MaxLength(200)]
            public string GenreName { get; set; } = string.Empty;
            public DateTime? YearStarted { get; set; }
            public int? Episodes { get; set; }
            public string? Status { get; set; }
            public string? WatchStatus { get; set; }
            public int? LastWatched { get; set; }
        }

        public class Studio
        {
            [Key]
            public int Id { get; set; }

            [Required]
            [MaxLength(200)]
            public string Name { get; set; } = string.Empty;
        }

        public class Genre
        {
            [Key]
            public int Id { get; set; }

            [Required]
            [MaxLength(200)]
            public string Name { get; set; } = string.Empty;
        }

        public void InitTables(string ConnectionString)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            string sqlQuery = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Titles')
            CREATE TABLE Titles (
                Id INT PRIMARY KEY IDENTITY(1,1),
                Title NVARCHAR(200) NOT NULL,
                Studio NVARCHAR(200) NOT NULL,
                PrimaryGenre NVARCHAR(200) NOT NULL,
                YearStarted DATE,
                Episodes INT,
                Status NVARCHAR (50),
                WatchStatus NVARCHAR (50),
                LastWatched INT,

                CONSTRAINT FK_Studio FOREIGN KEY (Studio) REFERENCES Studios(Name),
                CONSTRAINT FK_Genre FOREIGN KEY (PrimaryGenre) REFERENCES Genres(Name)
                )";

            string sqlQuery1 = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Studios')
            CREATE TABLE Studios (
                Id INT PRIMARY KEY IDENTITY(1,1),
                Name NVARCHAR(200) NOT NULL UNIQUE
                )";

            string sqlQuery2 = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Genres')
            CREATE TABLE Genres (
                Id INT PRIMARY KEY IDENTITY(1,1),
                Name NVARCHAR(200) NOT NULL UNIQUE
                )";

            using var command1 = new SqlCommand(sqlQuery1, connection);
            command1.ExecuteNonQuery();

            using var command2 = new SqlCommand(sqlQuery2, connection);
            command2.ExecuteNonQuery();
            
            using var command = new SqlCommand(sqlQuery, connection);
            command.ExecuteNonQuery();
        }

        public void TestData(string ConnectionString)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            string[] studios = { "MAPPA", "Studio Drive", "Kyoto Animation", "CloverWorks", "Wit Studio", "CygamesPictures" };
            string insertStudio = "INSERT INTO Studios (Name) VALUES (@name)";
            foreach (var studio in studios)
            {
                using var cmd = new SqlCommand(insertStudio, connection);
                cmd.Parameters.AddWithValue("@name", studio);
                cmd.ExecuteNonQuery();
            }

            string[] genres = { "Shonen", "Romance", "Gag Humor", "cgdct", "Isekai", "Music", "Sport" };
            string insertGenre = "INSERT INTO Genres (Name) VALUES (@name)";
            foreach (var genre in genres)
            {
                using var cmd = new SqlCommand(insertGenre, connection);
                cmd.Parameters.AddWithValue("@name", genre);
                cmd.ExecuteNonQuery();
            }

            var animes = new List<(string Title, string Studio, string Genre, string? Year, int? Ep, string Status, string Watch, int? Last)>
            {
                ("Lucky Star", "Kyoto Animation", "cgdct", "2007", 24, "Released", "Viewed", 24),
                ("Shikanoko Nokonoko Koshitantan", "Wit Studio", "Gag Humor", "2024", 12, "Released", "Viewed", 12),
                ("Nichijou", "Kyoto Animation", "Gag Humor", "2011", 26, "Released", "Viewed", 26),
                ("Konosuba 4", "Studio Drive", "Isekai", null, null, "Announced", "Planned", null),
                ("Bocchi The Rock! 2", "CloverWorks", "Music", null, null, "Announced", "Planned", null),
                ("Umamusume: Pretty Derby - Start of New Era", "CygamesPictures", "Sport", "2024", 1, "Released", "Watching", 1),
                ("Spy x Family", "CloverWorks", "Shonen", "2022", 12, "Released", "Viewed", 12),
            };

            string insertAnime = @"
            INSERT INTO Titles (Title, Studio, PrimaryGenre, YearStarted, Episodes, Status, WatchStatus, LastWatched)
            VALUES (@title, @sn, @gn, @ys, @ep, @status, @ws, @lw)";

            foreach (var anime in animes)
            {
                using var cmd = new SqlCommand(insertAnime, connection);
                cmd.Parameters.AddWithValue("@title", anime.Title);
                cmd.Parameters.AddWithValue("@sn", anime.Studio);
                cmd.Parameters.AddWithValue("@gn", anime.Genre);
                if (string.IsNullOrEmpty(anime.Year))
                    cmd.Parameters.AddWithValue("@ys", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@ys", DateTime.ParseExact(anime.Year, "yyyy", null));
                cmd.Parameters.AddWithValue("@ep", anime.Ep.HasValue ? (object)anime.Ep : DBNull.Value);
                cmd.Parameters.AddWithValue("@status", anime.Status);
                cmd.Parameters.AddWithValue("@ws", anime.Watch);
                cmd.Parameters.AddWithValue("@lw", anime.Last.HasValue ? (object)anime.Last : DBNull.Value);
                cmd.ExecuteNonQuery();
            }
        }

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
    }
}
