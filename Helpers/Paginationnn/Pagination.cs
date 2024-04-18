using System;
using System.Collections.Generic;
using System.Linq;

public class PaginationHelper<T>
{
    public IEnumerable<T> GetPaginatedData(IEnumerable<T> data, int page, int pageSize, string sortField, string sortOrder, string searchString=null, Func<T, bool> filterFunc = null)
    {

        // Apply additional filter if filterFunc is provided
        if (filterFunc != null)
        {
            data = data.Where(filterFunc);
        }

        // Apply sorting
        if (!string.IsNullOrEmpty(sortField))
        {
            data = ApplySorting(data, sortField, sortOrder);
        }

        // Calculate skip count based on pagination parameters
        int skipCount = (page - 1) * pageSize;

        // Apply pagination
        data = data.Skip(skipCount).Take(pageSize);

        return data;
    }

    private IEnumerable<T> ApplySorting(IEnumerable<T> data, string sortField, string sortOrder)
    {
        // Assuming T has properties, you can dynamically sort based on property name
        if (sortOrder.ToLower() == "asc")
        {
            return data.OrderBy(item => item.GetType().GetProperty(sortField).GetValue(item, null));
        }
        else
        {
            return data.OrderByDescending(item => item.GetType().GetProperty(sortField).GetValue(item, null));
        }
    }
}
