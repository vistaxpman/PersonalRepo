using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public abstract class Person
    {
        [Key]
        public int PersonID { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [Display(Name = "Last Name")]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [Column("FirstName")]
        [Display(Name = "First Name")]
        [MaxLength(50)]
        public string FirstName { get; set; }

        public string FullName
        {
            get
            {
                return LastName + ", " + FirstName;
            }
        }
    }
}