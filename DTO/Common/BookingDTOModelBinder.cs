using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text;
using System.Text.Json;

namespace DTO.Common;

public class BookingDTOModelBinder : IModelBinder
{
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        // ValueProvider is not designed to fetch the body of an HTTP POST request
        // ValueProvider is used to get values from certain parts of the request such as query string, route data and form data.

        // To Access the body of a POST request, we can use StreamReader to read the stream of bytes from the HTTP request body
        string requestBodyAsJson;
        using (var reader = new StreamReader(bindingContext.HttpContext.Request.Body, Encoding.UTF8))
        {
            requestBodyAsJson = await reader.ReadToEndAsync();
        }

        if (requestBodyAsJson.Length == 0)
        {
            bindingContext.ModelState.TryAddModelError(nameof(BookingDTO.BookingDTO), "Invalid input");
            bindingContext.Result = ModelBindingResult.Failed();
            return;
        }

        BookingDTO.BookingDTO? bookingDto = null;

        try
        {
            bookingDto = JsonSerializer.Deserialize<BookingDTO.BookingDTO>(requestBodyAsJson);
        }
        catch (Exception ex)
        {
            bindingContext.ModelState.TryAddModelError(nameof(BookingDTO.BookingDTO), ex.Message);
            bindingContext.Result = ModelBindingResult.Failed();
            return;
        }

        if (bookingDto is null)
        {
            bindingContext.ModelState.TryAddModelError(nameof(BookingDTO.BookingDTO), "Invalid input");
            bindingContext.Result = ModelBindingResult.Failed();
            return;
        }

        bindingContext.Result = ModelBindingResult.Success(bookingDto);
    }
}


/*
 var type = bindingContext.ModelType;
        var modelName = bindingContext.ModelName;
        var model = bindingContext.Model;
        var metadata = bindingContext.ModelMetadata;
        var properties = metadata.Properties.Select(p => p.Name);
        var dateValue = bindingContext.ValueProvider.GetValue("BookingDTO.Date").Values;
        var valueList1 = new List<StringValues>();
        var valueList2 = new List<StringValues>();
        properties.ToList().ForEach(p =>
        {
            valueList1.Add(bindingContext.ValueProvider.GetValue($"{modelName}.{p}").Values);
        });
        properties.ToList().ForEach(p =>
        {
            valueList2.Add(bindingContext.ValueProvider.GetValue($"BookingDTO.{p}").Values);
        });
        var modelValue1 = bindingContext.ValueProvider.GetValue(modelName);
        var modelValue2 = bindingContext.ValueProvider.GetValue("BookingDTO");

        // Custom binding for DateOnly type
        if (type is not null && type == typeof(DateOnly))
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult != ValueProviderResult.None &&
                DateOnly.TryParse(valueProviderResult.FirstValue, out var date))
            {
                bindingContext.Result = ModelBindingResult.Success(date);
            }
            else
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Invalid DateOnly format");
                bindingContext.Result = ModelBindingResult.Failed();
            }
        }
 */
