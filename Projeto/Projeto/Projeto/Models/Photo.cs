﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto.Models
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }


        [ForeignKey(nameof(Establishment))]
        public int EstablishmentFK { get; set; }
        public Establishment Establishment { get; set; }

    }
}
