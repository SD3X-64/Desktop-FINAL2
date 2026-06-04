using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
            public string Name { get; set; } = string.Empty;

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
            public int? LastWatchedEpisode { get; set; }
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
    }
}
