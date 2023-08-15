using System;
using System.ComponentModel.DataAnnotations.Schema;
using OnionArch.Domain.Entities.Common;

namespace OnionArch.Domain.Entities
{
	public class File:BaseEntity
	{
        //Burada bunun tablo içinde olmamasını istediğimizi belirliyoruz.
        [NotMapped]
        //Override işlemine basit bir örnek fakat bunu NotMapped yaptığımız için
        //Bu alan tablomuzda çıkmayacak.
        public override DateTime UpdateTime { get => base.UpdateTime; set => base.UpdateTime = value; }

        public string FileName { get; set; }

        public string Path { get; set; }
        //Storage'ın bilgisini tutan alan AWS Local Azure vs vs 
        public string Storage { get; set; }
    }
}

