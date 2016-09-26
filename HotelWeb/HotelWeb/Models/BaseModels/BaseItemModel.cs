using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelWeb.Models.BaseModels
{
    public class BaseItemModel<TModel> where TModel : new()
    {
        public BaseItemModel()
        {
            this.Item = new TModel();
        }

        public TModel Item { get; set; }
    }
}