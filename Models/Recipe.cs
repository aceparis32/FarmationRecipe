namespace FarmationRecipe.Models
{
    public class Recipe : BaseEntity
    {
        public string Name { get; set; } = null!;
        public virtual ICollection<RecipeIngredients> RecipeIngredients { get; set; } = [];
        public virtual ICollection<RecipeStep> RecipeSteps { get; set; } = [];
    }
}
