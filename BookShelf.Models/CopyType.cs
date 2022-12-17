using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookShelf.Models
{
    public class CopyType
    {
        [Key]
        public int Id { get; set; }
        [Display(Name="Cover Type")]
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
