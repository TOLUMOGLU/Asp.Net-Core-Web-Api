using System.Text.Json.Serialization;

namespace Entities.DataTransferObjects
{
    //[Serializable]
    //public record BookDto(int Id, String Title, decimal Price);  aşağıdaki gibi yapınca serialize a gerek yok ve daha derli toplu
    
    public record BookDto
    {
        public int Id { get; set; }
        public String Title { get; set; }
        public decimal Price { get; set; }

    }
}
