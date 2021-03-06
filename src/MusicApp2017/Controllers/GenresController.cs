﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicApp2017.Models;
using Microsoft.AspNetCore.Authorization;

namespace MusicApp2017.Controllers
{
    public class GenresController : Controller
    {
        private readonly MusicDbContext _context;

        public GenresController(MusicDbContext context)
        {
            _context = context;
        }

        // GET: Genres
        public async Task<IActionResult> Index()
        {
            var musicDbContext = _context.Genres;
            return View(await musicDbContext.ToListAsync());
        }

        // GET: Genres/Create
        [Authorize]
        public IActionResult Create()
        {
            //Nothing in here because genre only has a name and an ID
            return View();
        }

        // POST: Genres/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GenreID,Likes,Name")] Genre genre)
        {
            //checks for any other name in database
            if (ModelState.IsValid && !_context.Genres.Any(g => g.Name == genre.Name))
            {
                _context.Add(genre);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
         
            return View(genre);
        }

        // GET: Genres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genreContext = _context.Genres;
             
            var genre = await genreContext
                .SingleOrDefaultAsync(g => g.GenreID == id);
            //puts albums into a list
            ViewData["Albums"] = _context.Albums.Where(a => a.GenreID == id).ToList();
            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        // GET: Genres/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres.SingleOrDefaultAsync(g => g.GenreID == id);
            if (genre == null)
            {
                return NotFound();
            }
           
            return View(genre);
        }

        // POST: Genres/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GenreID,Likes,Name")] Genre genre)
        {
            if (id != genre.GenreID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(genre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GenreExists(genre.GenreID))
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
           
            return View(genre);
        }

        private bool GenreExists(int id)
        {
            return _context.Genres.Any(g => g.GenreID == id);
        }
    }
}
