using FarmationRecipe.Data;
using FarmationRecipe.Endpoints.Recipe.Requests;
using FarmationRecipe.Endpoints.Recipe.Responses;
using FarmationRecipe.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace FarmationRecipe.Endpoints.Recipe
{
    public static class RecipeEndpoint
    {
        public static void MapRecipeEndpoints(this IEndpointRouteBuilder routes)
        {
            //routes.MapGet("/recipes", async (AppDbContext db) =>
            //{
            //    return await db.Recipes.ToListAsync();
            //})
            //.WithName("GetAllRecipes")
            //.Produces<List<Recipe>>(StatusCodes.Status200OK);

            routes.MapGet("/recipes/{id}", async (Guid id, AppDbContext db, CancellationToken cancellationToken) =>
            {
                var recipe = await db.Recipes
                    .Include(x => x.RecipeIngredients)
                    .Include(x => x.RecipeSteps)
                    .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

                if (recipe == null)
                    return Results.NotFound("Recipe data not found!");

                var recipeResponse = new GetRecipeByIdResponse
                {
                    Id = recipe.Id,
                    Name = recipe.Name,
                    RecipeIngredients = recipe.RecipeIngredients
                        .Select(x => new RecipeIngredientsResponse
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Amount = x.Amount
                        }).ToList(),
                };

                var mainRecipeSteps = recipe.RecipeSteps
                    .Where(x => x.RecipeStepParentId == null)
                    .Select(x => new RecipeStepResponse
                    {
                        Id = x.Id,
                        RecipeStepParentId = x.RecipeStepParentId,
                        Order = x.Order,
                        Parameters = x.Parameters != null ?
                            x.Parameters.Deserialize<Dictionary<string, object>>()
                            : null
                    })
                    .OrderBy(x => x.Order)
                    .ToList();

                foreach (var mainRecipeStep in mainRecipeSteps)
                {
                    mainRecipeStep.RecipeStepChild = [];
                    mainRecipeStep.RecipeStepChild.Add(LoadSubRecipeSteps(mainRecipeStep, recipe.RecipeSteps));
                }

                recipeResponse.RecipeSteps = mainRecipeSteps;

                return Results.Ok(recipeResponse);
            })
            .WithName("GetRecipeById")
            .WithTags(["Recipe"])
            .Produces<GetRecipeByIdResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            routes.MapPost("/recipes", async (CreateRecipeRequest request, AppDbContext db, CancellationToken cancellationToken) =>
            {
                var isExist = await db.Recipes.AnyAsync(x => x.Name == request.Name, cancellationToken);
                if (isExist)
                    return Results.BadRequest("Recipe already exists");

                async Task AddSubStepsAsync(Guid recipeId, Guid recipeStepParentId, int order, Dictionary<string, object>? parameters, List<RecipeStepRequest>? recipeStepChild)
                {
                    var jsonParameters = parameters != null ?
                        JsonSerializer.Serialize(parameters)
                        : null;

                    var newStep = new Models.RecipeStep
                    {
                        RecipeId = recipeId,
                        RecipeStepParentId = recipeStepParentId,
                        Order = order,
                        Parameters = jsonParameters == null ? null : JsonDocument.Parse(jsonParameters)
                    };

                    await db.RecipeStep.AddAsync(newStep, cancellationToken);

                    if (recipeStepChild != null)
                        foreach (var stepChild in recipeStepChild)
                            await AddSubStepsAsync(recipeId, newStep.Id, stepChild.Order, stepChild.Parameters, stepChild.RecipeStepChild);
                }

                var newRecipe = new Models.Recipe
                {
                    Name = request.Name
                };

                await db.Recipes.AddAsync(newRecipe, cancellationToken);

                if (request.RecipeIngredients != null)
                {
                    foreach (var ingredients in request.RecipeIngredients)
                    {
                        var newIngredients = new Models.RecipeIngredients
                        {
                            RecipeId = newRecipe.Id,
                            Name = ingredients.Name,
                            Amount = ingredients.Amount
                        };

                        await db.RecipeIngredients.AddAsync(newIngredients, cancellationToken);
                    }
                }

                if (request.RecipeSteps != null)
                {
                    foreach (var step in request.RecipeSteps)
                    {
                        var jsonParameters = step.Parameters != null ?
                        JsonSerializer.Serialize(step.Parameters)
                        : null;

                        var newStep = new Models.RecipeStep
                        {
                            RecipeId = newRecipe.Id,
                            Order = step.Order,
                            Parameters = jsonParameters == null ? null : JsonDocument.Parse(jsonParameters)
                        };

                        await db.RecipeStep.AddAsync(newStep, cancellationToken);

                        if (step.RecipeStepChild != null)
                            foreach (var stepChild in step.RecipeStepChild)
                                await AddSubStepsAsync(newRecipe.Id, newStep.Id, stepChild.Order, stepChild.Parameters, stepChild.RecipeStepChild);
                    }
                }

                await db.SaveChangesAsync(cancellationToken);
                return Results.NoContent();
            })
            .WithName("CreateRecipe")
            .WithTags(["Recipe"])
            .Produces(StatusCodes.Status204NoContent);

            //routes.MapPut("/recipes/{id}", async (Guid id, Recipe inputRecipe, AppDbContext db) =>
            //{
            //    var recipe = await db.Recipes.FindAsync(id);
            //    if (recipe is null) return Results.NotFound();
            //    recipe.Name = inputRecipe.Name;
            //    await db.SaveChangesAsync();
            //    return Results.NoContent();
            //})
            //.WithName("UpdateRecipe")
            //.Produces(StatusCodes.Status204NoContent)
            //.Produces(StatusCodes.Status404NotFound);

            //routes.MapDelete("/recipes/{id}", async (Guid id, AppDbContext db) =>
            //{
            //    if (await db.Recipes.FindAsync(id) is Recipe recipe)
            //    {
            //        db.Recipes.Remove(recipe);
            //        await db.SaveChangesAsync();
            //        return Results.Ok(recipe);
            //    }
            //    return Results.NotFound();
            //})
            //.WithName("DeleteRecipe")
            //.Produces<Recipe>(StatusCodes.Status200OK)
            //.Produces(StatusCodes.Status404NotFound);
        }

        private static RecipeStepResponse LoadSubRecipeSteps(RecipeStepResponse mainRecipeStep, ICollection<RecipeStep> recipeSteps)
        {
            var subSteps = recipeSteps
                .Where(x => x.RecipeStepParentId == mainRecipeStep.Id)
                .Select(x => new RecipeStepResponse
                {
                    Id = x.Id,
                    RecipeStepParentId = x.RecipeStepParentId,
                    Order = x.Order,
                    Parameters = x.Parameters != null ?
                            x.Parameters.Deserialize<Dictionary<string, object>>()
                            : null
                })
                .OrderBy(x => x.Order)
                .ToList();

            mainRecipeStep.RecipeStepChild = subSteps;

            foreach (var subStep in subSteps)
                LoadSubRecipeSteps(subStep, recipeSteps);

            return mainRecipeStep;
        }
    }
}
