using System;
using System.Collections.Generic;

namespace CineAPI.ViewModels
{
    public class RoomDetailsViewModel
    {
        public int id { get; set; }

        public string Name { get; set; }

        public string RoomType { get; set; }

        public string Screen { get; set; }

        public ICollection<ExhibitionDetailsViewModel> Exhibitions { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }
    }
}
