using FarmationRecipe.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FarmationRecipe.Configurations
{
    public class RecipeIngredientsConfiguration : IEntityTypeConfiguration<RecipeIngredients>
    {
        public void Configure(EntityTypeBuilder<RecipeIngredients> builder)
        {
            builder.ToTable("RecipeIngredients");
            builder.HasKey(ri => ri.Id);
            builder.Property(ri => ri.Name).IsRequired().HasMaxLength(100);
            builder.Property(ri => ri.Amount).IsRequired();
            builder.HasIndex(ri => ri.Name);
            builder.HasOne(ri => ri.Recipe)
                .WithMany(r => r.RecipeIngredients)
                .HasForeignKey(ri => ri.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
