using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared
{
    public class ProductQueryParams
    {
        public int? brandId { get; set; }

        public int? typeId { get; set; }

        public string? search { get; set; }

        public ProductSortingOptions Sort { get; set; }


        private const int _defaultPageIndex = 1;
        private const int _maxPageSize = 10;
        private const int _defaultPageSize = 5;


        private int _pageIndex = _defaultPageIndex;
        public int PageIndex
        {
            get { return _pageIndex; }
            set 
            { 
                _pageIndex = (value <= 0? 1 : value);
            }
        }

        private int _pageSize = _defaultPageSize;
        public int PageSize
        {
            get { return _pageSize; }
            set 
            {
                _pageSize = (value > 10 ? _maxPageSize : value);
            }
        }


    }
}
    