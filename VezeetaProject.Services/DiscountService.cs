using AutoMapper;
using VezeetaProject.Core;
using VezeetaProject.Core.Dtos.DiscountDtos;
using VezeetaProject.Core.Models;

namespace VezeetaProject.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DiscountService(IUnitOfWork unitOfWork ,IMapper mappe)
        {
            _unitOfWork = unitOfWork;
            _mapper = mappe;
        }
        /// <summary>
        /// Add new Discount code in the system
        /// </summary>
        /// <param name="discountDto"></param>
        /// <returns>Discount model</returns>
        public async Task<Discount> AddDiscountAsync(DiscountDto discountDto)
        {
            if (! _unitOfWork.Discounts.FindAny(x=> x.DiscountCode == discountDto.DiscountCode))
            {
                var discount = _mapper.Map<Discount>(discountDto);

                var result = await _unitOfWork.Discounts.AddAsync(discount);
                _unitOfWork.Commit();
                _unitOfWork.Dispose();
                return result;
            }
            return null;
        }
        /// <summary>
        /// Deactivate Discount code 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>Discount type</returns>
        public async Task<Discount> DeactivateDiscountAsync(int Id)
        {
            var result = await _unitOfWork.Discounts.GetbyIdAsync(Id);
            try
            {
                if (result is null) return null;

                result.IsActivate = false;

                result = await _unitOfWork.Discounts.UpdateAsync(result);
                _unitOfWork.Commit();

            }
            finally
            {
                _unitOfWork.Dispose();
            }

          
           
            return result;
        }
        /// <summary>
        /// Delete DeleteDiscount from the system  
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>void</returns>
        public async Task<Discount> DeleteDiscountAsync(int Id)
        {
            var discount = await _unitOfWork.Discounts.GetbyIdAsync(Id);
            try
            {
                var check = _unitOfWork.Bookings.FindAny(x => x.DiscountId == Id);
                if (check) return null;

                if (discount is null) return null;

                _unitOfWork.Discounts.Delete(discount);
                _unitOfWork.Commit();
            }
            finally
            {
                _unitOfWork.Dispose();
            }
            return discount;
        }
        public async Task<Discount> UpdateDiscountAsync(UpdateDiscountDto discountUpdateDto)
        {
            var discount = _mapper.Map<Discount>(discountUpdateDto);
            if (discount is null) return null;

          
            var check =  _unitOfWork.Bookings.FindAny(x => x.DiscountId == discountUpdateDto.Id);
            if (check) return null;
           

            var result= await _unitOfWork.Discounts.UpdateAsync(discount);
            _unitOfWork.Commit();
            _unitOfWork.Dispose();
            return result;
        }
    }
}
