using OnionArch.Domain.Entities.Common;

namespace OnionArch.Domain.Entities
{
    public class BasketItem:BaseEntity
    {
       //Bu Entity bir sepete bağlı olan ürünleri listeliyor
            //Bunun için bir sepete ve sepetin sahip olduğu ürünleri tutması gerekmekte.
        public Basket Basket { get; set; }
        public Product Product { get; set; }
        public Guid BasketId {  get; set; }
        public Guid ProductId { get; set;}
        public int Quantity { get; set; } // Miktar sepette kaç adet var.

       
    }
}
