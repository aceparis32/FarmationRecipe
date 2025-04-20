using System.Text.Json;

namespace FarmationRecipe.Endpoints.Recipe.Responses
{
    public class GetRecipeByIdResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public List<RecipeIngredientsResponse>? RecipeIngredients { get; set; }
        public List<RecipeStepResponse>? RecipeSteps { get; set; }
    }

    public class RecipeIngredientsResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public double Amount { get; set; }  
    }

    public class RecipeStepResponse
    {
        public Guid Id { get; set; }
        public Guid? RecipeStepParentId { get; set; }
        public int Order { get; set; }
        public Dictionary<string, object>? Parameters { get; set; }
        public List<RecipeStepResponse>? RecipeStepChild { get; set; }
    }
}
