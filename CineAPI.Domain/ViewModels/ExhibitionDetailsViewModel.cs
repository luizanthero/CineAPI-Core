﻿using System;

namespace CineAPI.ViewModels
{
    public class ExhibitionDetailsViewModel
    {
        public int id { get; set; }

        public string Film { get; set; }

        public string Poster { get; set; }

        public string Year { get; set; }

        public string ApiCode { get; set; }

        public string Room { get; set; }

        public string Schedule { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }
    }
}
