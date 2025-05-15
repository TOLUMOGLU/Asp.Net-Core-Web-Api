using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    //readonly
    //immuatable
    //LINQ
    //Ref Type
    //Ctor (DTO)
    //public record BookDtoForUpdate //değer tipli ama farklı struct,record DTO readonly değişmeyen değer olmalı
    
        //public int Id { get; init; }  //değişmez demek,initilaze da değerleri almak
        //public String Title { get; init; }
        //public decimal Price { get; init; } bu yapı zor geliyorsa aşağıdaki gibi

        public record BookDtoForUpdate(int Id, String Title, decimal Price);

    
}
