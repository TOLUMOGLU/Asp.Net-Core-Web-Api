namespace Entities.Exceptions
{
    public sealed class BookNotFoundException : NotFoundException //hiçbir şekilde bir kalıtım olmaz, kalıtıma kapalı
    {
        public BookNotFoundException(int id) : base($"The book with id : {id} could not found.")
        {

        }
    }
}
