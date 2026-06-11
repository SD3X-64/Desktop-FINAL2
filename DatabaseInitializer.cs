using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
            public int? Seasons { get; set; }
            public int? Episodes { get; set; }
            public string? AnimeStatus { get; set; }
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
                StudioName NVARCHAR(200) NOT NULL,
                GenreName NVARCHAR(200) NOT NULL,
                YearStarted DATE,
                Seasons INT,
                Episodes INT,
                AnimeStatus NVARCHAR,
                WatchStatus NVARCHAR,
                LastWatched INT,
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
    }
}
