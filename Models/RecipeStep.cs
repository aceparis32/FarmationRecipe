using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace FarmationRecipe.Models
{
    public class RecipeStep : BaseEntity
    {
        [ForeignKey(nameof(Recipe))]
        public Guid RecipeId { get; set; }
        [ForeignKey(nameof(RecipeStepParent))]
        public Guid? RecipeStepParentId { get; set; }
        public int Order { get; set; }
        public string Description { get; set; } = null!;
        public int Duration { get; set; }
        public float Temperature { get; set; }
        public float Pressure { get; set; }
        public JsonDocument? Parameters { get; set; } = JsonDocument.Parse("{}");

        public virtual Recipe Recipe { get; set; } = null!;
        public virtual RecipeStep? RecipeStepParent { get; set; }
    }
}
