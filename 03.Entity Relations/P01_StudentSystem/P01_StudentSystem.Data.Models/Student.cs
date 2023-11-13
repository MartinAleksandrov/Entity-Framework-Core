using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace P01_StudentSystem.Data.Models
{
    public class Student
    {
        public Student()
        {
            StudentCourses = new HashSet<StudentCourses>();
            Homeworks = new HashSet<Homework>();
        }

        [Key]
        public int StudentId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        public int PhoneNumber { get; set; }

        [Required]
        public DateTime RegisteredOn { get; set; }

        public DateTime? Birthday  { get; set; }

        public ICollection<StudentCourses> StudentCourses { get; set; }

        public ICollection<Homework> Homeworks { get; set; }

    }
}