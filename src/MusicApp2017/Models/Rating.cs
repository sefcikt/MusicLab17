using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicApp2017.Models
{
    public class Rating
    {
        public int RatingID { get; set; }

        public int AlbumID { get; set; }

        public double RatingNumber { get; set; }

        public string UserID { get; set; }
    }
}
