using System;
using System.Runtime.Serialization;

namespace OnionArch.Application.CustomExceptions
{
    public class CreateUserExceptions : Exception
    {
        //Default olarak karşılaşacağımız hata mesajı. 
        public CreateUserExceptions():base("Kullanıcı oluşturulurken bir hata oldu")
        {

        }

        //Dışarıdan mesaj değerini yollayarak oluşturacağımız exception alanı. 
        public CreateUserExceptions(string? message) : base(message)
        {
        }
        //Dışarıdan mesaj ve Exception  değerlerini yollayarak oluşturacağımız exception alanı. 
        public CreateUserExceptions(string? message, Exception? innerException) : base(message, innerException)
        {
        }

       
    }
}

