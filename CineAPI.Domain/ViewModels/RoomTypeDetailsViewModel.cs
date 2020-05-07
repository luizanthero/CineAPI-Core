using System;
using System.Collections.Generic;

namespace CineAPI.ViewModels
{
    public class RoomTypeDetailsViewModel
    {
        public int id { get; set; }

        public string Description { get; set; }

        public ICollection<RoomDetailsViewModel> Rooms { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }
    }
}
