﻿using Library.DataAccess.Entites;
using Library.Domain.Abstractions;
using Library.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.DataAccess.RepositorIes
{
    public class BooksRepository : IBooksRepository
    {
        private readonly LibraryDbContext _context;
        public BooksRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> Get() 
        {
            var bookEntities = await _context.Books.AsNoTracking().ToListAsync();

            var books = bookEntities.Select(b => Book.BookCreate(b.Id, b.ISBN, b.BookName, b.Genre, b.Description, b.BookAuthorId, b.BookTook, b.BookReturned, b.Image).Book).ToList();
            return books;
        }

        public async Task<Guid> Create(Book book) 
        {
            var bookEntity = new BookEntity 
            { 
                Id = book.Id,
                ISBN = book.ISBN,
                BookName = book.BookName,
                Genre = book.Genre,
                Description = book.Description,
                BookTook = book.BookTook,
                BookReturned = book.BookReturned,
                Image = book.Image
            };

            await _context.AddAsync(bookEntity);
            await _context.SaveChangesAsync();

            return bookEntity.Id;
        }

        public async Task<Guid> Update(Guid id, string isbn, string name, string genre, string description, Guid authorId, DateTime bookTook, DateTime bookRerutn, byte[] image)
        {
            await _context.Books.Where(b => b.Id == id).ExecuteUpdateAsync(s => s
                .SetProperty(b => b.ISBN, b => isbn)
                .SetProperty(b => b.BookName, b => name)
                .SetProperty(b => b.Genre, b => genre)
                .SetProperty(b => b.Description, b => description)
                .SetProperty(b => b.BookAuthorId, b => authorId)
                .SetProperty(b => b.BookTook, b => bookTook)
                .SetProperty(b => b.BookReturned, b => bookRerutn)
                .SetProperty(b => b.Image, b => image)
                );

            return id;
        }

        public async Task<Guid> Delete(Guid id) 
        {
            await _context.Books.Where(b => b.Id == id).ExecuteDeleteAsync();

            return id;
        }
    }
}
