using System.Text.Json.Serialization;

namespace Entities.DataTransferObjects
{
    //[Serializable]
    //public record BookDto(int Id, String Title, decimal Price);  aşağıdaki gibi yapınca serialize a gerek yok ve daha derli toplu
    
    public record BookDto
    {
        public int Id { get; init; }
        public String Title { get; init; }
        public decimal Price { get; init; }

    }
}
