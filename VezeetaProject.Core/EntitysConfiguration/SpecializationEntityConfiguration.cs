namespace VezeetaProject.Core.EntitysConfiguration
{
    public class SpecializationEntityConfiguration : IEntityTypeConfiguration<Specialization>
    {
        public void Configure(EntityTypeBuilder<Specialization> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.SpecializaEn)
                   .IsRequired();
            builder.Property(p => p.SpecializaAr)
                  .IsRequired();
            builder.HasMany(p => p.Dotors)
                   .WithOne(p => p.Specialization)
                   .HasForeignKey(p => p.SpecializationId);

            builder.HasData(new[]{
                            new Specialization { Id = 1 ,SpecializaEn = "Cardiology / cardiac surgery" ,SpecializaAr ="أمراض القلب / جراحة القلب" },
                            new Specialization { Id = 2 ,SpecializaEn = "Dentistry" ,SpecializaAr ="طب الأسنان" },
                            new Specialization { Id = 3 ,SpecializaEn = "Dermatology" ,SpecializaAr ="الأمراض الجلدية"  },
                            new Specialization { Id = 4 ,SpecializaEn = "Ear, nose and throat (ENT)" ,SpecializaAr ="الأذن والأنف والحنجرة" },
                            new Specialization { Id = 5 ,SpecializaEn = "Endocrinology",SpecializaAr ="الغدد الصماء"  },
                            new Specialization { Id = 6 ,SpecializaEn = "Gastroenterology",SpecializaAr ="أمراض الجهاز الهضمي"  } 
                             }
                         );
        }
    }
}
