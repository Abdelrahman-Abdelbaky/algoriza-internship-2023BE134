namespace VezeetaProject.Core.EntitysConfiguration
{
    public class DiscountEntityConfiguration : IEntityTypeConfiguration<Discount>
    {
        public void Configure(EntityTypeBuilder<Discount> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.DiscountCode)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(p => p.requsetCompleted)
                   .IsRequired();

            builder.Property(p => p.DiscountType)
                   .IsRequired();

            builder.Property(p => p.Value)
                   .IsRequired();

            builder.Property(p => p.IsActivate)
                   .HasDefaultValue(true);

            builder.HasMany(p => p.bookings)
                   .WithOne(p => p.Discount)
                   .HasForeignKey(p => p.DiscountId);

            
        }
    }
}
