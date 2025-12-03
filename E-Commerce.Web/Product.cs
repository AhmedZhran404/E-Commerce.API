using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Web
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name Is Required")]
        [MaxLength(7 , ErrorMessage = "Max Length Is 7 char")]
        public string Name { get; set; } = null!;
    }
}
