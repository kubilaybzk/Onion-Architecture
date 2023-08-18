using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using OnionArch.infrastructure.Operations;

namespace OnionArch.infrastructure.Services
{
    public class FileService
    {
        readonly IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        private async Task<string> FileRenameAsync(string yol, string dosyaAdi, bool ilk = true)
        {
            string yeniDosyaAdi = await Task.Run<string>(async () =>
            {
                string uzanti = Path.GetExtension(dosyaAdi);
                string yeniDosyaAdi = string.Empty;

                if (ilk)
                {
                    // İlk adımda, dosya adını belirli kurallara göre değiştir.
                    string eskiAd = Path.GetFileNameWithoutExtension(dosyaAdi);
                    yeniDosyaAdi = $"{NameOperation.CharacterRegulatory(eskiAd)}{uzanti}";
                    Console.WriteLine("İlk dosya adı düzenlemesi.");
                }
                else
                {
                    // Sonraki adımlarda, mevcut dosya adlarına göre dosya adı değişiklikleri yap.
                    yeniDosyaAdi = dosyaAdi;
                    int indexNo1 = yeniDosyaAdi.IndexOf("-");

                    if (indexNo1 == -1)
                    {
                        // Eğer tire bulunamazsa, dosya adına "-2" eklenir.
                        yeniDosyaAdi = $"{Path.GetFileNameWithoutExtension(yeniDosyaAdi)}-2{uzanti}";
                    }
                    else
                    {
                        int sonIndex = 0;
                        while (true)
                        {
                            sonIndex = indexNo1;
                            indexNo1 = yeniDosyaAdi.IndexOf("-", indexNo1 + 1);

                            if (indexNo1 == -1)
                            {
                                indexNo1 = sonIndex;
                                break;
                            }
                        }

                        int indexNo2 = yeniDosyaAdi.IndexOf(".");
                        string dosyaNo = yeniDosyaAdi.Substring(indexNo1 + 1, indexNo2 - indexNo1 - 1);

                        if (int.TryParse(dosyaNo, out int _dosyaNo))
                        {
                            // Dosya adındaki sayıyı artır (örneğin, -2'yi -3 yap).
                            _dosyaNo++;
                            yeniDosyaAdi = yeniDosyaAdi.Remove(indexNo1 + 1, indexNo2 - indexNo1 - 1)
                                                .Insert(indexNo1 + 1, _dosyaNo.ToString());
                        }
                        else
                        {
                            // Sayı çıkarılamazsa, dosya adına "-2" eklenir.
                            yeniDosyaAdi = $"{Path.GetFileNameWithoutExtension(yeniDosyaAdi)}-2{uzanti}";
                        }
                    }
                }

                string uploadImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "resource", yol, yeniDosyaAdi);

                if (File.Exists(uploadImagePath))
                {
                    // Yeni adıyla bir dosya zaten varsa, işlemi değiştirilmiş bir ad ile tekrar çağır.
                    return await FileRenameAsync(yol, yeniDosyaAdi, false);
                }
                else
                {
                    // Yeni ad uygunsa, son adı döndür.
                    return yeniDosyaAdi;
                }
            });

            return yeniDosyaAdi;
        }
        
    }
}
