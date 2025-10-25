using System;

namespace QLThuvien.Models
{
    public class ViPhamUser
    {
        public int Id { get; set; }

        public string? UserId { get; set; }

        public ApplicationUser? User { get; set; }

        public int CountNoShow { get; set; }
        public int CountTreHan { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}