using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicApp2017.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace MusicApp2017.Controllers
{
    public class AlbumsController : Controller
    {
        private readonly MusicDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AlbumsController(MusicDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Albums
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                //can't find a way to pass the favorite genre   
                //NullReferenceException: Object reference not set to an instance of an object.
                ViewData["FavoriteGenre"] = _context.Genres.SingleOrDefault(g => g.GenreID == user.FavoriteGenre).Name;
                var musicDbContext = _context.Albums.Include(a => a.Artist).Include(a => a.Genre).Where(a => a.GenreID == user.FavoriteGenre);
                return View(await musicDbContext.ToListAsync());
            }
            else
            {
                var musicDbContext = _context.Albums.Include(a => a.Artist).Include(a => a.Genre);
                return View("DisplayAllAlbums", await musicDbContext.ToListAsync());
            }
        }

        public async Task<IActionResult> DisplayAllAlbums()
        {
            var musicDbContext = _context.Albums.Include(a => a.Artist).Include(a => a.Genre);
            return View(await musicDbContext.ToListAsync());
        }
        // GET: Albums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var albumContext = _context.Albums
                .Include(a => a.Artist)
                .Include(a => a.Genre);
            var album = await albumContext
                .SingleOrDefaultAsync(m => m.AlbumID == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // GET: Albums/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["ArtistID"] = new SelectList(_context.Artists, "ArtistID", "Name");
            ViewData["GenreID"] = new SelectList(_context.Genres, "GenreID", "Name");
            return View();
        }

        // POST: Albums/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AlbumID,Title,ArtistID,GenreID,Likes")] Album album)
        {
            if (ModelState.IsValid)
            {
                _context.Add(album);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ArtistID"] = new SelectList(_context.Artists, "ArtistID", "Name", album.ArtistID);
            ViewData["GenreID"] = new SelectList(_context.Genres, "GenreID", "Name", album.GenreID);
            return View(album);
        }

        // GET: Albums/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.Albums.SingleOrDefaultAsync(m => m.AlbumID == id);
            if (album == null)
            {
                return NotFound();
            }
            ViewData["ArtistID"] = new SelectList(_context.Artists, "ArtistID", "Name", album.ArtistID);
            ViewData["GenreID"] = new SelectList(_context.Genres, "GenreID", "Name", album.GenreID);
            return View(album);
        }

        // POST: Albums/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AlbumID,Title,ArtistID,GenreID,Likes")] Album album)
        {
            if (id != album.AlbumID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(album);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlbumExists(album.AlbumID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["ArtistID"] = new SelectList(_context.Artists, "ArtistID", "Name", album.ArtistID);
            ViewData["GenreID"] = new SelectList(_context.Genres, "GenreID", "Name", album.GenreID);
            return View(album);
        }

        // GET: Albums/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.Albums
                .Include(a => a.Artist)
                .Include(a => a.Genre)
                .SingleOrDefaultAsync(m => m.AlbumID == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // POST: Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var album = await _context.Albums.SingleOrDefaultAsync(m => m.AlbumID == id);
            _context.Albums.Remove(album);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool AlbumExists(int id)
        {
            return _context.Albums.Any(e => e.AlbumID == id);
        }

        [HttpPost]
        public async Task<IActionResult> SaveRating(int? id, Album album)
        {
                var currentAlbum = await _context.Albums.SingleOrDefaultAsync(a => a.AlbumID == id);
                var newRating = new Rating { AlbumID = id.Value, RatingNumber = album.RatingNumber };
                _context.Add(newRating);
                await _context.SaveChangesAsync();

                currentAlbum.AverageRating = AverageRating(currentAlbum.AlbumID);
            

                _context.Update(currentAlbum);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
        }

        private double AverageRating(int id)
        {
            if (_context.Rating.Where(a => a.AlbumID == id) != null)
            {
                var ratingAver = _context.Rating.Where(a => a.AlbumID == id).Average(r => r.RatingNumber);
                return Math.Round(ratingAver, 1);
            }
            else
            {
                return 0;
            }
        }

        private int NumberOfRatings(int id)
        {
            if (_context.Rating.Where(a => a.AlbumID == id) != null)
            {
                var number = _context.Rating.Count(a => a.AlbumID == id);
                return number;
            }
            else
            {
                return 0;
            }
        }
    }
}
