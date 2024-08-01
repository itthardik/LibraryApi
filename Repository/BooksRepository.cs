﻿using LMS2.DataContext;
using LMS2.Models;
using LMS2.Models.ViewModels;
using LMS2.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json.Serialization;

namespace LMS2.Repository
{
    
    
    /// <summary>
    /// Book Repo
    /// </summary>
    public class BooksRepository : IBooksRepository
    {
        private readonly ApiContext _context;
        
        
        /// <summary>
        /// Book Repo Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public BooksRepository(ApiContext? context) { 
            if(context != null)
                _context = context;
            else
                throw new ArgumentNullException(nameof(context));
        }



        /// <summary>
        /// Get All Books
        /// </summary>
        /// <returns></returns>
        public IQueryable<Book> GetAllBooks()
        {
            var allBooks = _context.Books
                                .Where<Book>(b => b.IsDeleted == false);
                                
            if (!allBooks.Any())
                throw new Exception("No Books found");

            return allBooks; 
        }
        
        
        /// <summary>
        /// Get Book by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Book GetBookById(int id) {
            var books = GetAllBooks()
                            .Where( b => b.Id == id)
                            .ToList();

            if (books.IsNullOrEmpty())
                throw new Exception("No book found with this Id");

            return books[0];
        }
        
        
        /// <summary>
        /// Add new Book
        /// </summary>
        /// <param name="inputBook"></param>
        /// <exception cref="Exception"></exception>
        public void AddBook(InputBook? inputBook) {
            
            if (inputBook == null)
                throw new Exception("Invalid Format");

            ValidationUtility.IsBookAlreadyExist( GetAllBooks(), inputBook);

            Book newBook = CustomUtility.ConvertInputBookToBook(inputBook);

            _context.Books.Add(newBook);
        }
        
        
        /// <summary>
        /// Delete Book By Id
        /// </summary>
        /// <param name="id"></param>
        public void DeleteBook(int id)
        {
            var foundBook = GetBookById(id);
            foundBook.IsDeleted = true;
        }
        
        
        /// <summary>
        /// Update book by id and InputBook
        /// </summary>
        /// <param name="id"></param>
        /// <param name="book"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Book UpdateBook(int id, InputBook book)
        {
            if (id == 0)
                throw new Exception("Id cannot be Zero");

            if(book == null) 
                throw new Exception("Invalid Format");

            var foundBook = GetBookById(id);

            CustomUtility.UpdateObject1WithObject2(foundBook, book);

            return foundBook;            
        }
        
        
        /// <summary>
        /// Get Filter data by search parms and with pagination
        /// </summary>
        /// <param name="newBook"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IQueryable<Book> GetBooksBySearchParams( int pageNumber, int pageSize, InputBook newBook)
            {

            var result = CustomUtility.FilterBooksBySearchParams( _context, newBook, pageNumber, pageSize);
            
            if (result.IsNullOrEmpty())
                throw new Exception("No Books Found");
            
            return result;
        }
        
        
        /// <summary>
        /// Save Changes to DB
        /// </summary>
        public void Save()
        {
            _context.SaveChanges();
        }

    }
}