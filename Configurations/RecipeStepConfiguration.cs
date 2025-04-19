using FarmationRecipe.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FarmationRecipe.Configurations
{
    public class RecipeStepConfiguration : IEntityTypeConfiguration<RecipeStep>
    {
        public void Configure(EntityTypeBuilder<RecipeStep> builder)
        {
            builder.ToTable("RecipeStep");
            builder.HasKey(rs => rs.Id);
            builder.Property(rs => rs.Description).IsRequired().HasMaxLength(500);
            builder.Property(rs => rs.Duration).IsRequired();
            builder.HasIndex(rs => rs.Description);
            builder.HasIndex(rs => rs.Order);
            builder.HasOne(rs => rs.Recipe)
                .WithMany(r => r.RecipeSteps)
                .HasForeignKey(rs => rs.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
