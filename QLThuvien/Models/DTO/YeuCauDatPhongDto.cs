using System;
using System.ComponentModel.DataAnnotations;

namespace QLThuvien.Models.Dto
{
    public class YeuCauDatPhongDto
    {
        [Required]
        public int PhongId { get; set; }

        [Required]
        public DateTime GioBatDau { get; set; }

        [Required]
        public DateTime GioKetThuc { get; set; }
    }
}
