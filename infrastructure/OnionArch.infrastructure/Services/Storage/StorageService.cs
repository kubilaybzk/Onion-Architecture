using System;
using Microsoft.AspNetCore.Http;
using OnionArch.Application.Abstractions.Storage;

namespace OnionArch.infrastructure.Services.Storage
{
    public class StorageService : IStorageService
    {

        //  Dependency Injection kullandık Bu sayede gelecek olan storage yöntemi
        //  ne olursa ona göre bir çalıştırma işlemi yapacağız.


        readonly IStorage _ıstorage;

        public StorageService(IStorage ıstorage)
        {
            _ıstorage = ıstorage;
        }

        public string StorageType { get => _ıstorage.GetType().Name; }

        public async Task DeleteFileAsync(string FileName, string PathOrContainerName)

          => await _ıstorage.DeleteFileAsync(FileName, PathOrContainerName);


        public List<string> GetAllFiles(string PathOrContainerName)

          => _ıstorage.GetAllFiles(PathOrContainerName);


        public bool HasFile(string FileName, string PathOrContainerName)

         => _ıstorage.HasFile(FileName, PathOrContainerName);


        public Task<List<(string fileName, string PathOrContainerName)>> UploadAsync(string PathOrContainerName, IFormFileCollection files)
        => _ıstorage.UploadAsync(PathOrContainerName, files);
    }
}

