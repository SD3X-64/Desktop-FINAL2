using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.SqlServer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop_FINAL2
{
    internal class DatabaseInitializer
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
                Status NVARCHAR,
                WatchStatus NVARCHAR,
                LastWatched INT,

                CONSTRAINT FK_Studio FOREIGN KEY (Studio) REFERENCES Studios(Name),
                CONSTRAINT FK_Genre FOREIGN KEY (PrimaryGenre) REFERENCES Genres(Name)
                )";

            string sqlQuery1 = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Studios')
            CREATE TABLE Studios (
                Id INT PRIMARY KEY IDENTITY(1,1),
                Name NVARCHAR(200) NOT NULL,
                )";

            string sqlQuery2 = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Genres')
            CREATE TABLE Genres (
                Id INT PRIMARY KEY IDENTITY(1,1),
                Name NVARCHAR(200) NOT NULL,
                )";

            using var command = new SqlCommand(sqlQuery, connection);
            command.ExecuteNonQuery();

            using var command1 = new SqlCommand(sqlQuery1, connection);
            command1.ExecuteNonQuery();

            using var command2 = new SqlCommand(sqlQuery2, connection);
            command2.ExecuteNonQuery();
        }

        public void TestData(string ConnectionString)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            string sqlQuery2 = @"INSERT INTO Titles (Title, Studio, PrimaryGenre, YearStarted, Episodes, Status, WatchStatus, LastWatched)
                VALUES (@title, @sn, @gn, @ys, @ep, @status, @ws, @lw)";

            string sqlQuery = @"INSERT INTO Studios (Name)
                VALUES (@name)";

            string sqlQuery1 = @"INSERT INTO Genres (Name)
                VALUES (@name)";

            using var command = new SqlCommand(sqlQuery, connection);
            command.Parameters.AddWithValue("@name", "MAPPA");
            command.ExecuteNonQuery();

            using var command1 = new SqlCommand(sqlQuery, connection);
            command.Parameters.AddWithValue("@name", "Studio Drive");
            command.ExecuteNonQuery();

            using var command2 = new SqlCommand(sqlQuery, connection);
            command.Parameters.AddWithValue("@name", "Kyoto Animation");
            command.ExecuteNonQuery();

            using var command3 = new SqlCommand(sqlQuery, connection);
            command.Parameters.AddWithValue("@name", "CloverWorks");
            command.ExecuteNonQuery();

            using var command4 = new SqlCommand(sqlQuery, connection);
            command.Parameters.AddWithValue("@name", "Wit Studio");
            command.ExecuteNonQuery();

            using var command5 = new SqlCommand(sqlQuery, connection);
            command.Parameters.AddWithValue("@name", "CygamesPictures");
            command.ExecuteNonQuery();

            using var command6 = new SqlCommand(sqlQuery1, connection);
            command.Parameters.AddWithValue("@name", "Shonen");
            command1.ExecuteNonQuery();

            using var command7 = new SqlCommand(sqlQuery1, connection);
            command.Parameters.AddWithValue("@name", "Romance");
            command1.ExecuteNonQuery();

            using var command8 = new SqlCommand(sqlQuery1, connection);
            command.Parameters.AddWithValue("@name", "Gag Humor");
            command1.ExecuteNonQuery();

            using var command9 = new SqlCommand(sqlQuery1, connection);
            command.Parameters.AddWithValue("@name", "cgdct");
            command1.ExecuteNonQuery();

            using var command10 = new SqlCommand(sqlQuery1, connection);
            command.Parameters.AddWithValue("@name", "Isekai");
            command1.ExecuteNonQuery();

            using var command11 = new SqlCommand(sqlQuery1, connection);
            command.Parameters.AddWithValue("@name", "Music");
            command1.ExecuteNonQuery();

            using var command12 = new SqlCommand(sqlQuery2, connection);
            command.Parameters.AddWithValue("@title", "Lucky Star");
            command.Parameters.AddWithValue("@sn", "Kyoto Animation");
            command.Parameters.AddWithValue("@gn", "cgdct");
            command.Parameters.AddWithValue("@ys", "2007");
            command.Parameters.AddWithValue("@ep", 24);
            command.Parameters.AddWithValue("@status", "Released");
            command.Parameters.AddWithValue("@ws", "Viewed");
            command.Parameters.AddWithValue("@lw", 24);
            command2.ExecuteNonQuery();

            using var command13 = new SqlCommand(sqlQuery2, connection);
            command.Parameters.AddWithValue("@title", "Shikanoko Nokonoko Koshitantan");
            command.Parameters.AddWithValue("@sn", "Wit Studio");
            command.Parameters.AddWithValue("@gn", "Gag Humor");
            command.Parameters.AddWithValue("@ys", "2024");
            command.Parameters.AddWithValue("@ep", 12);
            command.Parameters.AddWithValue("@status", "Released");
            command.Parameters.AddWithValue("@ws", "Viewed");
            command.Parameters.AddWithValue("@lw", 12);
            command2.ExecuteNonQuery();

            using var command14 = new SqlCommand(sqlQuery2, connection);
            command.Parameters.AddWithValue("@title", "Nichijou");
            command.Parameters.AddWithValue("@sn", "Kyoto Animation");
            command.Parameters.AddWithValue("@gn", "Gag Humor");
            command.Parameters.AddWithValue("@ys", "2011");
            command.Parameters.AddWithValue("@ep", 26);
            command.Parameters.AddWithValue("@status", "Released");
            command.Parameters.AddWithValue("@ws", "Viewed");
            command.Parameters.AddWithValue("@lw", 26);
            command2.ExecuteNonQuery();

            using var command15 = new SqlCommand(sqlQuery2, connection);
            command.Parameters.AddWithValue("@title", "Konosuba 4");
            command.Parameters.AddWithValue("@sn", "Studio Drive");
            command.Parameters.AddWithValue("@gn", "Isekai");
            command.Parameters.AddWithValue("@ys", null);
            command.Parameters.AddWithValue("@ep", null);
            command.Parameters.AddWithValue("@status", "Announced");
            command.Parameters.AddWithValue("@ws", "Planned");
            command.Parameters.AddWithValue("@lw", null);
            command2.ExecuteNonQuery();

            using var command16 = new SqlCommand(sqlQuery2, connection);
            command.Parameters.AddWithValue("@title", "Bocchi The Rock 2");
            command.Parameters.AddWithValue("@sn", "CloverWorks");
            command.Parameters.AddWithValue("@gn", "Music");
            command.Parameters.AddWithValue("@ys", null);
            command.Parameters.AddWithValue("@ep", null);
            command.Parameters.AddWithValue("@status", "Announced");
            command.Parameters.AddWithValue("@ws", "Planned");
            command.Parameters.AddWithValue("@lw", null);
            command2.ExecuteNonQuery();

            using var command17 = new SqlCommand(sqlQuery2, connection);
            command.Parameters.AddWithValue("@title", "Umamusume: Pretty Derby - Shin Jidai no Tobira");
            command.Parameters.AddWithValue("@sn", "CygamesPictures");
            command.Parameters.AddWithValue("@gn", "Sport");
            command.Parameters.AddWithValue("@ys", "2024");
            command.Parameters.AddWithValue("@ep", 1);
            command.Parameters.AddWithValue("@status", "Released");
            command.Parameters.AddWithValue("@ws", "Watching");
            command.Parameters.AddWithValue("@lw", 1);
            command2.ExecuteNonQuery();

            using var command18 = new SqlCommand(sqlQuery2, connection);
            command.Parameters.AddWithValue("@title", "Spy x Family");
            command.Parameters.AddWithValue("@sn", "cloverWorks");
            command.Parameters.AddWithValue("@gn", "Shonen");
            command.Parameters.AddWithValue("@ys", "2022");
            command.Parameters.AddWithValue("@ep", 12);
            command.Parameters.AddWithValue("@status", "Released");
            command.Parameters.AddWithValue("@ws", "Viewed");
            command.Parameters.AddWithValue("@lw", 12);
            command2.ExecuteNonQuery();
        }
    }
}
