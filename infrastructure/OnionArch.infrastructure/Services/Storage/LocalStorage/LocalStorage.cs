using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using OnionArch.Application.Abstractions.Storage.LocalStorage;

namespace OnionArch.infrastructure.Services.Storage.LocalStorage
{
    public class LocalStorage : ILocalStorage
    {

        //Dependency Injection
        readonly IWebHostEnvironment _webHostEnvironment;

        public LocalStorage(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
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

        public  List<string> GetAllFiles(string PathOrContainerName)
        {
            DirectoryInfo target = new DirectoryInfo(PathOrContainerName);
            return  target.GetFiles().Select(p=>p.Name).ToList();
        }

        public bool HasFile(string FileName, string PathOrContainerName)
        {
            return File.Exists($"{PathOrContainerName}\\{FileName}");
        }




        public async Task<List<(string fileName, string PathOrContainerName)>> UploadAsync(string PathOrContainerName, IFormFileCollection files)
        {

            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, PathOrContainerName);
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            List<(string fileName, string path)> datas = new();

            foreach (IFormFile file in files)
            {
                var Name = file.FileName;
                string CopyFilePath = $"{uploadPath}\\{file.FileName}";

                bool result = await CopyFileAsync(CopyFilePath, file);

                datas.Add((file.FileName, $"{uploadPath}\\{file.FileName}"));

            }
            return datas;
        }

      

    }
}





