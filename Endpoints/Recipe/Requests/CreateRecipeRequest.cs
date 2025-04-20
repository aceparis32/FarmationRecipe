namespace FarmationRecipe.Endpoints.Recipe.Requests
{
    public class CreateRecipeRequest
    {
        public string Name { get; set; } = null!;
        public List<RecipeIngredientsRequest>? RecipeIngredients { get; set; }
        public List<RecipeStepRequest>? RecipeSteps { get; set; }
    }

    public class RecipeIngredientsRequest
    {
        public string Name { get; set; } = null!;
        public double Amount { get; set; }
    }

    public class RecipeStepRequest
    {
        public int Order { get; set; }
        public Dictionary<string, object>? Parameters { get; set; }
        public List<RecipeStepRequest>? RecipeStepChild { get; set; }
    }
}
