namespace Artillery.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using Artillery.Utilities;

    public class Shell
    {
        public Shell()
        {
            Guns = new HashSet<Gun>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(GlobalConstance.MaxShellWeightLenght)]
        public double ShellWeight  { get; set; }


        [Required]
        [MaxLength(GlobalConstance.MaxCaliberSize)]
        public string Caliber { get; set; } = null!;

        public ICollection<Gun> Guns { get; set; }

    }
}
