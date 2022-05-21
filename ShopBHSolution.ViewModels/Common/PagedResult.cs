using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBHSolution.ViewModels.Common
{
    public class PagedResult<T> : PagingRequestBase
    {
        public List<T> Items { set; get; }
    }
}
