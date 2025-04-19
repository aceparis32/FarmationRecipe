using FarmationRecipe.Configurations;
using FarmationRecipe.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmationRecipe.Data
{
    public class AppDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeIngredients> RecipeIngredients { get; set; }
        public DbSet<RecipeStep> RecipeStep { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RecipeConfiguration());
            modelBuilder.ApplyConfiguration(new RecipeIngredientsConfiguration());
            modelBuilder.ApplyConfiguration(new RecipeStepConfiguration());
        }
    }
}
