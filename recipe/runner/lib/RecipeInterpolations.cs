using System.Text;
using CliWrap;

namespace Tell;

public static class RecipeInterpolations
{
    extension (Recipe recipe)
    {
        public string InterpolateWith(Dictionary<string, string> variables)
        {
            var result = new StringBuilder();

            var firstElement = recipe.Elements.FirstOrDefault();
            if (firstElement is null)
            {
                return string.Empty;
            }

            result.Append(firstElement.InterpolateWith(variables));
            
            foreach (var element in recipe.Elements.Skip(1))
            {
                var interpolated = element.InterpolateWith(variables);
                if (element.Word.HasValue)
                {
                    //result.Append(' ');
                }

                result.Append(interpolated);
            }

            return result.ToString();
        }
    }

    extension (RecipeElement element)
    {
        public string InterpolateWith(IReadOnlyDictionary<string, string> variables)
        {
            if (element.Word is not null && element.Word.HasValue)
            {
                return element.Word.Value.ToStringValue();
            }
            
            var variableName = element.VariableUse!.Name.ToStringValue();
            return variables.GetValueOrDefault(variableName, defaultValue: String.Empty);
        }
    }
}
