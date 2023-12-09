namespace VezeetaProject.Core.Dtos.DiscountDtos
{
    public class DiscountDto
    {
        [Required, MaxLength(50)]
        public string DiscountCode { get; set; }
        [Required]
        public int requsetCompleted { get; set; }
        [Required]
        public DiscountType DiscountType { get; set; }
        [Required]
        public decimal Value { get; set; }

    }
}
