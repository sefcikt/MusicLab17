﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MusicApp2017.Models
{
    public class Album
    {
        public int AlbumID { get; set; }
        [Required(ErrorMessage ="Title is required")]
        public string Title { get; set; }

        // Foreign key
        public int ArtistID { get; set; }
        // Navigation property
        public Artist Artist { get; set; }
        // Foreign key
        [Display(Name ="Genre")]
        public int GenreID { get; set; }
        // Navigation property
        public Genre Genre { get; set; }
        [Display(Name = "Rating")]
        public double RatingNumber { get; set; }
        [Display(Name = "AverageRating")]
        public double AverageRating { get; set; }
    }
}
