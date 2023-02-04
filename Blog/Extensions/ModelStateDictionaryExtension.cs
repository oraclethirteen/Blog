using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Blog.Extensions
{
    public static class ModelStateDictionaryExtension
    {
        public static string GetAllError(this ModelStateDictionary modelState)
        {
            List<string> errorList = new List<string>();

            errorList.AddRange(modelState.Root.Errors
                .Select(e => e.ErrorMessage)
                .ToList());


            errorList.AddRange(modelState.Values.SelectMany(m => m.Errors)
                .Select(e => e.ErrorMessage)
                .ToList());

            return string.Join(";\\r\\n", errorList);
        }
    }
}
