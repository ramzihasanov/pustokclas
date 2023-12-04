using Microsoft.EntityFrameworkCore;
using WebApplication6.CustomExceptions.BookException;
using WebApplication6.Helpers;
using WebApplication6.Repositories;

namespace WebApplication6.Services.IImplementations
{
    public class BookService : IBookService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IBookRepository _bookRepository;
        public BookService(IWebHostEnvironment env, IBookRepository bookRepository)
        {
            _env = env;
            _bookRepository = bookRepository;

        }
        public async Task CreateAsync(Book book)
        {

            List<Author> Authors = await _bookRepository.GetAllAuthorAsync();
            List<Genre> Genres = await _bookRepository.GetAllGenreAsync();
            List<Tag> Tags = await _bookRepository.GetAllTagAsync();
            List<BookTag> BookTags = await _bookRepository.GetAllBookTagAsync();
            List<BookImage> BookImages = await _bookRepository.GetAllBookImagesAsync();

            if (!Authors.Any(a => a.Id == book.AuthorId))
            {
                throw new InvalidAuthorid("AuthorId", "author is not found!!!");
            }

            if (!Genres.Any(g => g.Id == book.GenreId))
            {
                throw new invalidGenreId("GenreId", "genre is not found!!!");
            }

            bool check = false;

            if (book.TagIds != null)
            {
                foreach (var item in book.TagIds)
                {
                    if (!Tags.Any(t => t.Id == item))
                    {
                        check = true;
                        break;
                    }

                }
            }

            if (check)
            {
                throw new InvalidTagId("TagId", "tag is not found!!!");
            }
            else
            {
                if (book.TagIds != null)
                {
                    foreach (var item in book.TagIds)
                    {
                        BookTag bookTag = new BookTag()
                        {
                            Book = book,
                            TagId = item,
                        };

                        await _bookRepository.CreateBookTagAsync(bookTag);
                    }
                }
            }


            if (book.FaceImage != null)
            {

                if (book.FaceImage.ContentType != "image/png" && book.FaceImage.ContentType != "image/jpeg")
                {
                    throw new InvalidContenttype("FaceImage", "ancaq sekil yukle");
                    
                }

                if (book.FaceImage.Length > 1048576)
                {
                    throw new InvalidImgSize("FaceImage", "1 mb dan az yukle pul yazir ");
                    
                }
                
                string newFileName = Helper.GetFileName(_env.WebRootPath, "upload", book.FaceImage);
                BookImage bookImage = new BookImage
                {
                    book = book,
                    ImageUrl = newFileName,
                    IsPoster = true,
                };

                await _bookRepository.CreateBookImageAsync(bookImage);
            };

            if (book.BackViewImg != null)
            {

                if (book.BackViewImg.ContentType != "image/png" && book.BackViewImg.ContentType != "image/jpeg")
                {
                    throw new InvalidContenttype("FaceImage", "ancaq sekil yukle");
                }

                if (book.BackViewImg.Length > 1048576)
                {
                    throw new InvalidImgSize("FaceImage", "1 mb dan az yukle pul yazir ");
                }

                string newFileName = Helper.GetFileName(_env.WebRootPath, "upload", book.BackViewImg);
                BookImage bookImage = new BookImage
                {
                    book = book,
                    ImageUrl = newFileName,
                    IsPoster = false,
                };
               await _bookRepository.CreateBookImageAsync( bookImage);
            };

            if (book.BookDetailImgs != null)
            {
                foreach (var img in book.BookDetailImgs)
                {
                    string fileName = img.FileName;
                    if (img.ContentType != "image/png" && img.ContentType != "image/jpeg")
                    {
                        throw new InvalidContenttype("FaceImage", "ancaq sekil yukle");
                    }

                    if (img.Length > 1048576)
                    {
                        throw new InvalidImgSize("FaceImage", "1 mb dan az yukle pul yazir ");
                    }

                    string newFileName = Helper.GetFileName(_env.WebRootPath, "upload", img);
                    BookImage bookImage = new BookImage
                    {
                        book = book,
                        ImageUrl = newFileName,
                        IsPoster = null,
                    };
                    await _bookRepository.CreateBookImageAsync(bookImage);
                }
            }

            await _bookRepository.CreateAsync(book);
            await _bookRepository.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            if (id == null) throw new InvalidNullreferance();

            Book wantedBook = await _bookRepository.GetAsync(id);

            if (wantedBook == null) throw new InvalidNullreferance();

            if (wantedBook.BookImages != null)
            {
                foreach (var item in wantedBook.BookImages)
                {
                    string path = Path.Combine(_env.WebRootPath, "upload", item.ImageUrl);

                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
            }

            _bookRepository.Delete(wantedBook);
            await _bookRepository.CommitAsync();
        }

        public async Task<List<Book>> GetAllAsync()
        {
            return await _bookRepository.GetAllAsync();
        }

        public async Task<List<Author>> GetAllAuthorAsync()
        {
            return await _bookRepository.GetAllAuthorAsync();
        }

        public async Task<List<Genre>> GetAllGenreAsync()
        {
            return await _bookRepository.GetAllGenreAsync();
        }

        public async Task<List<Tag>> GetAllTagAsync()
        {
            return await _bookRepository.GetAllTagAsync();
        }

        public async Task<Book> GetAsync(int id)
        {
            return await _bookRepository.GetAsync(id);
        }

        public async Task UpdateAsync(Book book)
        {
            List<Book> Books = await _bookRepository.GetAllAsync();
            List<Author> Authors = await _bookRepository.GetAllAuthorAsync();
            List<Genre> Genres = await _bookRepository.GetAllGenreAsync();
            List<Tag> Tags = await _bookRepository.GetAllTagAsync();
            List<BookTag> BookTags = await _bookRepository.GetAllBookTagAsync();
            List<BookImage> BookImages = await _bookRepository.GetAllBookImagesAsync();


            Book existBook = await _bookRepository.GetAsync(book.Id);

            if (existBook == null) throw new InvalidNullreferance();

            if (!Authors.Any(a => a.Id == book.AuthorId))
            {
                throw new InvalidAuthorid("AuthorId", "author is not found!!!");
            }

            if (!Genres.Any(g => g.Id == book.GenreId))
            {
                throw new invalidGenreId("GenreId", "genre is not found!!!");
            }

            existBook.BookTags.RemoveAll(bt => !book.TagIds.Any(tId => tId == bt.TagId));

            foreach (var id in book.TagIds.Where(bt => !existBook.BookTags.Any(tId => bt == tId.TagId)))
            {
                BookTag bookTag = new BookTag()
                {
                    TagId = id,
                };

                existBook.BookTags.Add(bookTag);

            }


            if (book.FaceImage != null)
            {
                string folderPath = "upload";
                string path = Path.Combine(_env.WebRootPath, folderPath, existBook.BookImages.FirstOrDefault(x => x.IsPoster == true).ImageUrl);

                existBook.BookImages.RemoveAll(bi => !book.BookImagesIds.Contains(bi.Id) && bi.IsPoster == true);


                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                if (book.FaceImage.ContentType != "image/png" && book.FaceImage.ContentType != "image/jpeg")
                {

                    throw new InvalidContenttype("FaceImage", "sekil yukle a bala");
                }

                if (book.FaceImage.Length > 1048576)
                {
                    throw new InvalidImgSize("FaceImage", "1 mb dan az yukle");
                }

                string newFileName = Helper.GetFileName(_env.WebRootPath, "upload", book.FaceImage);
                BookImage bookImage = new BookImage
                {
                    book = book,
                    ImageUrl = newFileName,
                    IsPoster = true,
                };
                existBook.BookImages.Add(bookImage);
            };

            if (book.BackViewImg != null)
            {
                string folderPath = "upload";
                string path = Path.Combine(_env.WebRootPath, folderPath, existBook.BookImages.Where(x => x.IsPoster == false).FirstOrDefault().ImageUrl);
                existBook.BookImages.RemoveAll(x => !book.BookImagesIds.Contains(x.Id) && x.IsPoster == false);


                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                if (book.BackViewImg.ContentType != "image/png" && book.BackViewImg.ContentType != "image/jpeg")
                {
                    throw new InvalidContenttype("BackViewImg", "sekil tap gel");
                }

                if (book.BackViewImg.Length > 1048576)
                {
                    throw new InvalidImgSize("BackViewImg", "1 mbdan az yukle");
                }

                string newFileName = Helper.GetFileName(_env.WebRootPath, "upload", book.BackViewImg);
                BookImage bookImage = new BookImage
                {
                    book = book,
                    ImageUrl = newFileName,
                    IsPoster = false,
                };
                existBook.BookImages.Add(bookImage);
            };



            foreach (var item in existBook.BookImages.Where(x => !book.BookImagesIds.Contains(x.Id) && x.IsPoster == null))
            {
                string path = Path.Combine(_env.WebRootPath, "upload", item.ImageUrl);

                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }

            existBook.BookImages.RemoveAll(x => !book.BookImagesIds.Contains(x.Id) && x.IsPoster == null);

            if (book.BookDetailImgs != null)
            {
                foreach (var img in book.BookDetailImgs)
                {

                    if (img.ContentType != "images/png" && img.ContentType != "image/jpeg")
                    {
                        throw new InvalidContenttype("BookDetailImgs", "get sekil yukle ");
                    }

                    if (img.Length > 1048576)
                    {
                        throw new InvalidImgSize("BookDetailImgs", "1 mbdan az yukle");
                    }

                    string newFileName = Helper.GetFileName(_env.WebRootPath, "upload", img);
                    BookImage bookImage = new BookImage
                    {
                        book = book,
                        ImageUrl = newFileName,
                        IsPoster = null,
                    };
                    existBook.BookImages.Add(bookImage);
                }
            }


            existBook.Name = book.Name;
            existBook.Description = book.Description;
            existBook.Tax = book.Tax;
            existBook.Code = book.Code;
            existBook.SalePrice = book.SalePrice;
            existBook.CostPrice = book.CostPrice;
            existBook.IsAvailable = book.IsAvailable;
            existBook.isBestseller = book.isBestseller;
            existBook.isNew = book.isNew;
            existBook.isFeatured = book.isFeatured;
            existBook.DisPrice = book.DisPrice;
            existBook.AuthorId = book.AuthorId;
            existBook.GenreId = book.GenreId;


            await _bookRepository.CommitAsync();
        }
    }
}
