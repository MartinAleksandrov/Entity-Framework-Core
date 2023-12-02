using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicines.Data.Models
{
    public class PatientMedicine
    {
        [Required]
        [ForeignKey(nameof(Patient))]
        public int PatientId  { get; set; }

        [Required]
        public Patient Patient { get; set; } = null!;


        [Required]
        [ForeignKey(nameof(Medicine))]
        public int MedicineId  { get; set; }

        [Required]
        public Medicine Medicine  { get; set; } = null!;

    }
}
