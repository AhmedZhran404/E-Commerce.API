using System.ComponentModel.DataAnnotations;

namespace ECommerce.Shared.BasketDTO
{
    public record BasketItemsDTO
    (
         int Id,
         string ProductName ,
         string PictureUrl ,
        [Range(0 , double.MaxValue)]
         decimal Price,
        [Range(0 , 100)]
        int Quantity 
    );
}