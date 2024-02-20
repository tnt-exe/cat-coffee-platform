using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.OData.Edm;
using DTO.BookingDTO;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.OData.Batch;

namespace CatCoffeePlatformAPI.Common.Odata
{
    public class BookingEnableQueryAttribute : EnableQueryAttribute
    {
        public override IQueryable ApplyQuery(IQueryable queryable, ODataQueryOptions queryOptions)
        {
            return queryable;

            /*IQueryable? result = default(IQueryable);

            // Get the original request
            HttpRequest originalRequest = queryOptions.Request;

            var settings = new ODataQuerySettings();
            var orderBy = queryOptions.OrderBy;
            var skip = queryOptions.Skip;
            var top = queryOptions.Top;
            var selectExpand = queryOptions.SelectExpand;

            if (queryOptions.Filter is not null)
            {
                if (queryOptions.Filter.RawValue.ToLower().Contains("date eq"))
                {
                    var filterStrings = queryOptions.Filter.RawValue.Split(" and ");
                    var acceptFilter = filterStrings?.Where(f => !f.ToLower().Contains("date eq")) ?? Enumerable.Empty<string>();
                    string newFilterString;
                    if (acceptFilter.Count() == 1)
                    {
                        newFilterString = acceptFilter.First();
                    }
                    else
                    {
                        newFilterString = String.Join(" and ", acceptFilter);
                    }
                    var newFilter = new FilterQueryOption(newFilterString, queryOptions.Filter.Context,
                        new Microsoft.OData.UriParser.ODataQueryOptionParser(
                            model: queryOptions.Filter.Context.Model,
                            targetEdmType: queryOptions.Filter.Context.NavigationSource.EntityType(),
                            targetNavigationSource: queryOptions.Filter.Context.NavigationSource,
                            queryOptions: new Dictionary<string, string> { { "newFilter", newFilterString } },
                            container: queryOptions.Filter.Context.RequestContainer
                            ));

                    result = newFilter.ApplyTo(queryable, settings);
                }
                else
                {
                    result = queryOptions.Filter.ApplyTo(queryable, settings);
                }
            }
            if (queryOptions.Count?.Value == true)
            {
                queryOptions.Request.ODataFeature().TotalCount = ((IQueryable<BookingResponseDTO>)queryable).LongCount();
            }
            if (orderBy != null)
                result = orderBy.ApplyTo(queryable, settings);
            if (skip != null)
                result = skip.ApplyTo(queryable, settings);
            if (top != null)
                result = top.ApplyTo(queryable, settings);
            if (selectExpand != null)
                result = selectExpand.ApplyTo(queryable, settings);


            // This is used to change the existed values
            // add the NextLink if one exists
            *//*if (queryOptions.Request.ODataFeature().NextLink != null)
            {
                originalRequest.ODataFeature().NextLink = queryOptions.Request.ODataFeature().NextLink;
            }
            // add the TotalCount if one exists
            if (queryOptions.Request.ODataFeature().TotalCount != null)
            {
                originalRequest.ODataFeature().TotalCount = queryOptions.Request.ODataFeature().TotalCount;
            }*//*

            return result ?? Enumerable.Empty<BookingResponseDTO>().AsQueryable();*/
        }
    }
}
