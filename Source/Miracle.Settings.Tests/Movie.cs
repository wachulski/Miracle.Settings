﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Miracle.Settings.Tests
{
    /// <summary>
    /// Sample class more or less taken from Microsoft documentation.
    /// </summary>
    public class Movie
    {
        public int ID { get; set; }

        [StringLength(60, MinimumLength = 3)]
        public string Title { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
        [Required]
        [StringLength(30)]
        public string Genre { get; set; }

        [Range(1, 100)]
        public decimal Price { get; set; }

        [RegularExpression(@"^[A-Z0-9]*$")]
        [StringLength(5)]
        public string Rating { get; set; }
    }
}