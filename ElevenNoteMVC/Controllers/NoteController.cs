using ElevenNote.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElevenNoteMVC.Controllers
{
    // This annotation makes it so that the views are accessible only to logged in users
    [Authorize]
    public class NoteController : Controller
    {
        // GET: /Note/Index
        public ActionResult Index()
        {
            var model = new NoteListItem[0];
            return View(model);
        }

        //GET: /Note/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Note/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NoteCreate model)
        {
            if (ModelState.IsValid)
            {

            }
            return View(model);
        }
    }
}