using BookLib.Data;
using BookLib.Entity;
using BookLib.Interface;
using System.Collections.Generic;
using System.Linq;

namespace BookLib.Service
{
    public class LibraryService : ILibraryService
    {
        private readonly BookLibContext _bookLibContext;

        public LibraryService(BookLibContext bookLibContext)
        {
            _bookLibContext = bookLibContext;
        }
        public void Create(Library library)
        {
            _bookLibContext.Libraries.Add(library);
            _bookLibContext.SaveChanges();
        }

        public IEnumerable<Library> GetLibraries()
            => _bookLibContext.Libraries.ToList();

        public Library GetLibrary(int id)
            => _bookLibContext.Libraries.FirstOrDefault(x => x.Id == id);

        public void Update(Library library)
        {
            _bookLibContext.Libraries.Update(library);
            _bookLibContext.SaveChanges();
        }
    }
}
