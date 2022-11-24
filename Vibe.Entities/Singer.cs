using System.ComponentModel.DataAnnotations;

namespace Vibe.Entities
{
    public class Singer
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Nickname { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        public string Photo { get; set; } = string.Empty;
    }
}