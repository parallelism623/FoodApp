using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FoodShop.Application.Common.DataTransferObjects.Request.V1;
using FoodShop.Application.Common.Exceptions;
using FoodShop.Application.Common.FileUpload;
using FoodShop.Domain.Exceptions;
using FoodShop.Infrastructure.Common.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Infrastructure.FileUpload
{
    public class FileServices : IFileServices
    {
        private readonly CloudinarySettings _cloudSettings;
        public FileServices(IOptionsMonitor<CloudinarySettings> cloudSettings)
        {
            _cloudSettings = cloudSettings.CurrentValue;
        }
        public async Task UpLoadImageAsync<T>(UploadImageRequest uploadImage)
        {
            UpLoadImageAsync<T>(uploadImage.Id, uploadImage.ImageLink);
            foreach(var image in uploadImage.ImageList)
            {
                UpLoadImageAsync<T>(uploadImage.Id, image);
            }

        }
        private async Task UpLoadImageAsync<T>(Guid Id, string filePath)
        {
            Account account = new Account(
                _cloudSettings.CloudName,
                _cloudSettings.APIKey,
                _cloudSettings.APISecret
                );
            Cloudinary cloudinary = new Cloudinary(account);

            var imageParams = new ImageUploadParams
            {
                DisplayName = CreateDisplayName(filePath),
                File = new FileDescription($"{filePath}"),
                PublicId = $"FoodShopAPI/{nameof(T)}s/{Id}",
                Overwrite = true,
            };
            var upLoadResult = await cloudinary.UploadAsync(imageParams);
            if (!(upLoadResult.Error.Message.Contains("OK") || upLoadResult.Error.Message.Contains("Success")))
            {
                throw new InternalServerException("An Error Ocurred");
            }
        }
        private string CreateDisplayName(string filePath)
        {
            var newFilePath = filePath.Split(new string[] { "./\\" }, StringSplitOptions.None);
            return newFilePath[newFilePath.Length - 2];
        }


    }
}
