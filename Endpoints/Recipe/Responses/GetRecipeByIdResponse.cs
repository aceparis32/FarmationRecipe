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
        public string Description { get; set; } = null!;
        public int Duration { get; set; }
        public float Temperature { get; set; }
        public float Pressure { get; set; }
        public List<RecipeStepResponse>? RecipeStepChild { get; set; }
    }
}
