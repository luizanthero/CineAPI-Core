using System;
using System.Collections.Generic;

namespace CineAPI.ViewModels
{
    public class FilmDetailsViewModel
    {
        public int id { get; set; }

        public string Name { get; set; }

        public string ApiCode { get; set; }

        public string Poster { get; set; }

        public string Plot { get; set; }

        public string Genre { get; set; }

        public string Actors { get; set; }

        public string Writer { get; set; }

        public string Director { get; set; }

        public string Year { get; set; }

        public string Runtime { get; set; }

        public string Awards { get; set; }

        public string Production { get; set; }

        public string Website { get; set; }

        public ICollection<ExhibitionDetailsViewModel> Exhibitions { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }
    }
}
