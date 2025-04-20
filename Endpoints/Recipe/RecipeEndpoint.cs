using FarmationRecipe.Data;
using FarmationRecipe.Endpoints.Recipe.Requests;
using Microsoft.EntityFrameworkCore;

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
            
            //routes.MapGet("/recipes/{id}", async (Guid id, AppDbContext db) =>
            //{
            //    return await db.Recipes.FindAsync(id)
            //        is Recipe recipe
            //            ? Results.Ok(recipe)
            //            : Results.NotFound();
            //})
            //.WithName("GetRecipeById")
            //.Produces<Recipe>(StatusCodes.Status200OK)
            //.Produces(StatusCodes.Status404NotFound);
            
            routes.MapPost("/recipes", async (CreateRecipeRequest request, AppDbContext db, CancellationToken cancellationToken) =>
            {
                var isExist = await db.Recipes.AnyAsync(x => x.Name == request.Name, cancellationToken);
                if (isExist)
                    return Results.BadRequest("Recipe already exists");

                async Task AddSubStepsAsync(Guid recipeId, Guid recipeStepParentId, int order, string description, int duration, float temperature, float pressure, List<RecipeStepRequest>? recipeStepChild)
                {
                    var newStep = new Models.RecipeStep
                    {
                        RecipeId = recipeId,
                        RecipeStepParentId = recipeStepParentId,
                        Order = order,
                        Description = description,
                        Duration = duration,
                        Temperature = temperature,
                        Pressure = pressure
                    };

                    await db.RecipeStep.AddAsync(newStep, cancellationToken);

                    if (recipeStepChild != null)
                    {
                        foreach (var stepChild in recipeStepChild)
                        {
                            await AddSubStepsAsync(recipeId, newStep.Id, stepChild.Order, stepChild.Description, stepChild.Duration, stepChild.Temperature, stepChild.Pressure, stepChild.RecipeStepChild);
                        }
                    }
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
                        var newStep = new Models.RecipeStep
                        {
                            RecipeId = newRecipe.Id,
                            Order = step.Order,
                            Description = step.Description,
                            Duration = step.Duration,
                            Temperature = step.Temperature,
                            Pressure = step.Pressure
                        };

                        await db.RecipeStep.AddAsync(newStep, cancellationToken);

                        if (step.RecipeStepChild != null)
                        {
                            foreach (var stepChild in step.RecipeStepChild)
                            {
                                await AddSubStepsAsync(newRecipe.Id, newStep.Id, stepChild.Order, stepChild.Description, stepChild.Duration, stepChild.Temperature, stepChild.Pressure, stepChild.RecipeStepChild);
                            }
                        }
                    }
                }

                await db.SaveChangesAsync(cancellationToken);
                return Results.NoContent();
            })
            .WithName("CreateRecipe")
            .WithTags([ "Recipe" ])
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
    }
}
