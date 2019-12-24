using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;
using Validation;

namespace ParkingAbilityServer.Models
{
    public class HeadersModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            Requires.NotNull(bindingContext, nameof(bindingContext));
            var headerDictionary = bindingContext.HttpContext.Request.Headers;
            var model = Activator.CreateInstance(bindingContext.ModelType, headerDictionary);
            bindingContext.Result = ModelBindingResult.Success(model);
            return Task.CompletedTask;
        }
    }
}
