﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CORE2
{
    /// <summary>
    /// Paged results with continuation token
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedResults<T>
    {
        public PagedResults()
        {
            Results = new List<T>();
        }
        /// <summary>
        /// Continuation Token for DocumentDB
        /// </summary>
        public string ContinuationToken { get; set; }

        /// <summary>
        /// Results
        /// </summary>
        public List<T> Results { get; set; }
    }
}
