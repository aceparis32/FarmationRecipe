using System.ComponentModel.DataAnnotations.Schema;

namespace FarmationRecipe.Models
{
    public class RecipeIngredients : BaseEntity
    {
        [ForeignKey(nameof(Recipe))]
        public Guid RecipeId { get; set; }
        public string Name { get; set; } = null!;
        public double Amount { get; set; }

        public virtual Recipe Recipe { get; set; } = null!;
    }
}
