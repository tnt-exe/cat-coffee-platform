using DTO.Common.ModelBinderExample;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace DTO.Common;

public class ModelBinderProvider : IModelBinderProvider
{
    private readonly IList<IInputFormatter> _formatters;
    private readonly IHttpRequestStreamReaderFactory _readerFactory;
    private BodyModelBinderProvider _defaultProvider;

    public ModelBinderProvider(IList<IInputFormatter> formatters, IHttpRequestStreamReaderFactory readerFactory)
    {
        _formatters = formatters;
        _readerFactory = readerFactory;
        _defaultProvider = new BodyModelBinderProvider(formatters, readerFactory);
    }

    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        // Get model binder for a specific type of model
        // If the model is type of IExampleEntity, get the custom model binder named ExampleModelBinder
        if (context.Metadata.ModelType == typeof(IExampleEntity))
        {
            IModelBinder modelBinder = _defaultProvider.GetBinder(context);
            if (modelBinder != null)
            {
                // Get a specific model binder here
                modelBinder = new ExampleModelBinder(_formatters, _readerFactory);
            }
            return modelBinder;
        }

        // Deafult model binder is null
        return null;
    }
}
