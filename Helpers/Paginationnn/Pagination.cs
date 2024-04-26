using System;
using System.Collections.Generic;
using System.Linq;

public class PaginationHelper<T>
{
    public IEnumerable<T> GetPaginatedData(IEnumerable<T> data, int page, int pageSize, string sortField, string sortOrder, string searchString=null, Func<T, bool> filterFunc = null)
    {

        if (filterFunc != null)
        {
            data = data.Where(filterFunc);
        }

        if (!string.IsNullOrEmpty(sortField))
        {
            data = ApplySorting(data, sortField, sortOrder);
        }

        int skipCount = (page - 1) * pageSize;

        data = data.Skip(skipCount).Take(pageSize);

        return data;
    }

    private IEnumerable<T> ApplySorting(IEnumerable<T> data, string sortField, string sortOrder)
    {
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
