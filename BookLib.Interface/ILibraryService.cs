using BookLib.Entity;
using System.Collections.Generic;

namespace BookLib.Interface
{
    public interface ILibraryService
    {
        Library GetLibrary(int id);
        IEnumerable<Library> GetLibraries();
        void Create(Library library);
        void Update(Library library);
    }
}