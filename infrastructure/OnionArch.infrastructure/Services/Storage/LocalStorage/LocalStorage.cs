using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OnionArch.Application.Abstractions.Storage.LocalStorage;

namespace OnionArch.infrastructure.Services.Storage.LocalStorage
{
    public class LocalStorage : Storage, ILocalStorage
    {

        //Dependency Injection
        readonly IWebHostEnvironment _webHostEnvironment;
        readonly ILogger<LocalStorage> _logger;

        public LocalStorage(IWebHostEnvironment webHostEnvironment, ILogger<LocalStorage> logger)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
          
        }

        //Sadece buraya özel bir method bundan dolayı private yapabilriiz.
        private async Task<bool> CopyFileAsync(string path, IFormFile file)
        {
            try
            {
                //Gelen dosya için bir filestream işlemi ger
                await using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);
                //Gelen dosyayı kopyalama işlemini gerçekleştiriyoruz.
                await file.CopyToAsync(fileStream);
                //Kopyalama işemi sonrası temizleme işlemi yapıyoruzç
                await fileStream.FlushAsync();
                return true;
            }
            catch (Exception ex)
            {
                //todo log!
                throw ex;
            }
        }


        public async Task DeleteFileAsync(string FileName, string PathOrContainerName)
        {
            File.Delete($"{PathOrContainerName}\\{FileName}");
        }

        public List<string> GetAllFiles(string PathOrContainerName)
        {
            DirectoryInfo target = new DirectoryInfo(PathOrContainerName);
            return target.GetFiles().Select(p => p.Name).ToList();
        }

        public bool HasFile(string FileName, string PathOrContainerName)
        {
            string uploadImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "resource", PathOrContainerName, FileName);

            return File.Exists(uploadImagePath);
        }

        public async Task<List<(string fileName, string PathOrContainerName)>> UploadAsync(string PathOrContainerName, IFormFileCollection files)
        {
            if (files[0].Length == 0)
            {
                _logger.LogError("Görsel eklemeden ekleme işlemi çalıştırılmaya çalışılıyor.");
                throw new Exception("Görsel eklenmeden ürün ekleyemezsiniz.! ");
                
            }
            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "resource", PathOrContainerName);
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            List<(string fileName, string path)> datas = new();


            foreach (IFormFile file in files)
            {
                string uploadImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "resource", PathOrContainerName, file.FileName);


                string fileNewName = await FileRenameAsync(PathOrContainerName, file.FileName, HasFile);
                string uploadImagePathNew = Path.Combine(_webHostEnvironment.WebRootPath, "resource", PathOrContainerName, fileNewName);
                string databasePath = Path.Combine("resource", PathOrContainerName, fileNewName);
                await CopyFileAsync(uploadImagePathNew, file);
                datas.Add((fileNewName, databasePath));
            }
            return datas;
        }



    }
}





