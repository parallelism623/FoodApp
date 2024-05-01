using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Common.DataTransferObjects.Request.V1
{
    public class UploadImageRequest
    {
        public Guid Id { get; set; }
        public string ImageLink { get; set; }
        public string[] ImageList {get;set;}
    }
}
