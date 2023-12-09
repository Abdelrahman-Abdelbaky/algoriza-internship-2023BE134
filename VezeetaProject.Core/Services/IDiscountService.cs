using VezeetaProject.Core.Dtos.DiscountDtos;

namespace VezeetaProject.Core.Services
{
    public interface IDiscountService
    {
        Task<Discount> AddDiscountAsync(DiscountDto discountDto); 
        Task<Discount> DeactivateDiscountAsync(int Id); 
        Task<Discount> DeleteDiscountAsync(int Id); 
        Task<Discount> UpdateDiscountAsync(UpdateDiscountDto discountUpdateDto); 
      

    }
}
