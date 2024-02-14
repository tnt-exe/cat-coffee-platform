using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace DTO.Common.ModelBinderExample;

public class ExampleModelBinder : IModelBinder
{
    private readonly BodyModelBinder defaultBinder;
    public ExampleModelBinder(IList<IInputFormatter> formatters, IHttpRequestStreamReaderFactory readerFactory)
    {
        defaultBinder = new BodyModelBinder(formatters, readerFactory);
    }

    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        // Body model binding
        await defaultBinder.BindModelAsync(bindingContext);

        // Cutom binding here
        if (bindingContext.Result.IsModelSet && bindingContext.Result.Model is IExampleEntity)
        {
            var inputModel = (IExampleEntity)bindingContext.Result.Model;

            // all of your property updates with be listed here
            //
            //
            //

            bindingContext.Result = ModelBindingResult.Success(inputModel);
        }
    }
}
