namespace Medicines.DataProcessor.ImportDtos
{
    using System.ComponentModel.DataAnnotations;

    public class ImportPatients
    {
        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string FullName { get; set; } = null!;

        [Required]
        [Range(0, 2)]
        public int AgeGroup { get; set; }

        [Required]
        [Range(0, 1)]
        public int Gender { get; set; }

        public int[] Medicines { get; set; }
    }
}
