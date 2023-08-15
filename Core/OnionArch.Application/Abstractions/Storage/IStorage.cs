using System;
using Microsoft.AspNetCore.Http;

namespace OnionArch.Application.Abstractions.Storage
{
	public interface IStorage
	{
        //Burası tüm storage işlemlerinde ortak olarak kullanılacak olan İmzaları içerecek olan interface.

        Task<List<(string fileName, string PathOrContainerName)>> UploadAsync(string PathOrContainerName, IFormFileCollection files);
        //Biz bir path vereceğiz onun içine bu verileri oluşturacağız fakat bu
        //Container'da olabilir AWS gibi yada Azure gibi cloud servislerinde bunlar
        //Container olarak geçer bundan dolayı PathOrContainer olarak isimlendirdik.

        Task DeleteFileAsync(string FileName, string PathOrContainerName);
        //Dosya ismine sahip olan tüm resimleri silelim.
        //Nereden "PathOrContainer" burada verdiğimiz dizinden .
        //Nereden "PathOrContainer" burada verdiğimiz dizinden .

        List<string> GetAllFiles(string PathOrContainerName);
        //verdiğimiz dizinde olan bütün resimlerin listesi.

        bool HasFile(string FileName, string PathOrContainerName);
        //verdiğimiz isme sahip bir dosya var mı yok mu ? 


    }
}

