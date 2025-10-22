using System;
using System.Collections.Generic;

namespace HR.Core.BaseEntity
{
    /// <summary>
    /// Paged list
    /// </summary>
    /// <typeparam name="T">T</typeparam>
    [Serializable]
    public class PagedList<T>
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="totalCount">Total count</param>
        public PagedList(IList<T> source, int pageIndex, int pageSize, long? totalCount = null)
        {
            //min allowed page size is 1
            pageSize = Math.Max(pageSize, 1);

            TotalCount = totalCount ?? (source == null ? 0 : source.Count);
            TotalPages = TotalCount / pageSize;

            if (TotalCount % pageSize > 0)
                TotalPages++;

            PageSize = pageSize;
            PageIndex = pageIndex;
            // Items = source;
            Items = source is null || source.Count == 0 ? [] : source;

        }

        /// <summary>
        /// Page index
        /// </summary>
        public int PageIndex { get; }

        /// <summary>
        /// Page size
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// Total count
        /// </summary>
        public long TotalCount { get; }

        /// <summary>
        /// Total pages
        /// </summary>
        public long TotalPages { get; }

        /// <summary>
        /// Item list
        /// </summary>
        public IList<T> Items { get; set; }
    }
}