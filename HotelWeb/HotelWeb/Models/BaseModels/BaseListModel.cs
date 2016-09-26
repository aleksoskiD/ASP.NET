using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelWeb.Models.BaseModels
{
    public class BaseListModel<TModel>
    {
        public IEnumerable<TModel> Items { get; set; }
    }
}