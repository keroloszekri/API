using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Description;
using BookStoreApi.Models;
using Microsoft.AspNet.Identity;

namespace BookStoreApi.Controllers
{
    [Authorize]
    public class BooksController : ApiController
    {
        private ApplicationDBContext db = new ApplicationDBContext();

        // GET: api/Books
        public List<Bookdto> GetBooks()
        {
            // string Name = User.Identity.Name;  // Get User Name
            // var Claims = ((ClaimsIdentity)(User.Identity)).Claims.ToList();// Get User ID
            List<Bookdto> bookListdto = new List<Bookdto>();
            foreach (var item in db.Books)
            {
                Bookdto bookdto = new Bookdto();
                bookdto.ISBN = item.ISBN;
                bookdto.Price = item.Price;
                bookdto.Title = item.Title;
                bookdto.Year = item.Year;
                if (item.UserID != null)
                {
                    bookdto.UserID = item.UserID;
                }
                bookListdto.Add(bookdto);
            }
            return bookListdto;
        }

        // GET: api/Books/5
        [ResponseType(typeof(Bookdto))]
        public IHttpActionResult GetBook(string id)
        {
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }
            Bookdto bookdto = new Bookdto();
            bookdto.ISBN = book.ISBN;
            bookdto.Price = book.Price;
            bookdto.Title = book.Title;
            bookdto.Year = book.Year;
            if (book.UserID != null)
            {
                bookdto.UserID = book.UserID;
            }
            return Ok(bookdto);
        }

        // PUT: api/Books/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBook(string id, Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != book.ISBN)
            {
                return BadRequest();
            }

            db.Entry(book).State = EntityState.Modified;

            try
            {
                book.UserID = User.Identity.GetUserId();
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Books
        [ResponseType(typeof(Book))]
        public IHttpActionResult PostBook(Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            book.UserID = User.Identity.GetUserId();
            db.Books.Add(book);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (BookExists(book.ISBN))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = book.ISBN }, book);
        }

        // DELETE: api/Books/5
        [ResponseType(typeof(Book))]
        public IHttpActionResult DeleteBook(string id)
        {
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }

            db.Books.Remove(book);
            db.SaveChanges();

            return Ok(book);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BookExists(string id)
        {
            return db.Books.Count(e => e.ISBN == id) > 0;
        }
    }
}