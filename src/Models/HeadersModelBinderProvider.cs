using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Validation;

namespace ParkingAbilityServer.Models
{
    public class HeadersModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            Requires.NotNull(context, nameof(context));

            if (context.Metadata.ModelType == typeof(MetadataRequest))
            {
                return new BinderTypeModelBinder(typeof(HeadersModelBinder));
            }

            return null;
        }
    }
}
