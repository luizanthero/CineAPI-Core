using CineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CineAPI.ViewModels
{
    public class ExhibitionDetailsViewModel
    {
        public int id { get; set; }

        public string Filme { get; set; }

        public string ApiCode { get; set; }

        public string Room { get; set; }

        public string Schedule { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }

        public static ICollection<ExhibitionDetailsViewModel> ConvertTo(ICollection<Exhibition> exhibitions)
        {
            List<ExhibitionDetailsViewModel> result = new List<ExhibitionDetailsViewModel>();

            exhibitions.ToList().ForEach(item =>
            {
                result.Add(new ExhibitionDetailsViewModel()
                {
                    id = item.id,
                    Filme = item.Film.Name,
                    ApiCode = item.Film.ApiCode,
                    Room = item.Room.Name,
                    Schedule = item.Schedule.Description,
                    created_at = item.created_at,
                    updated_at = item.updated_at
                });
            });

            return result;
        }

        public static ExhibitionDetailsViewModel ConvertTo(Exhibition exhibition)
        {
            return new ExhibitionDetailsViewModel()
            {
                id = exhibition.id,
                Filme = exhibition.Film.Name,
                ApiCode = exhibition.Film.ApiCode,
                Room = exhibition.Room.Name,
                Schedule = exhibition.Schedule.Description,
                created_at = exhibition.created_at,
                updated_at = exhibition.updated_at
            };
        }
    }
}
