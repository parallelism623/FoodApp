using FoodShop.Application.Common.DataTransferObjects.Request.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Common.FileUpload
{
    public interface IFileServices
    {
        public Task UpLoadImageAsync<T>(UploadImageRequest request);
    }
}
