using System;
using Microsoft.AspNetCore.Http;

namespace OnionArch.Application.Services
{
	public interface IFileService
	{
        //Burada dosya yüklerken nelere ihtiyacımız düşünelim ve bu dosyanın içinde bu methodların imzalarını oluşturalım.

        Task<List<(string fileName, string path)>> UploadAsync(string path, IFormFileCollection files);
        //Biz bir path vereceğiz onun içine bu verileri oluşturacağız.
        //Bir yada birden fazla  dosya vereceğiz bu gelen dosyayı kayıt edecek bunlar request tarafından gelecek. .

        Task<bool> CopyFileAsync(string path,IFormFile file);
        //Burada dosyaları kayıt edeceğimiz işlemi gerçekleştiriyoruz.Yani Fiziksel olarak dizine ekleyeceğiz.


    }
}

