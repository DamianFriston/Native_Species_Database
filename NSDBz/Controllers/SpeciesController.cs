using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NSDBz.DAL;
using NSDBz.Models;
using NSDBz.ViewModels;
using System.Data.Entity.Infrastructure;
using PagedList;
using PagedList.Mvc;

namespace NSDBz.Controllers
{
    public class SpeciesController : Controller
    {
        private ZoologyContext db = new ZoologyContext();

        // GET: Species
        public ActionResult Index(string searchString, string Poison, string Venom, string Classification, int? page, string sortOrder, string currentFilter)
        {
            ViewBag.CurrentSort = sortOrder;
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var species = db.Species.Include(s => s.Countries);
            if (!String.IsNullOrEmpty(searchString))
                
            {                
                species = species.Where(s => s.Name.Contains(searchString) || s.Countries.Any(c => c.Name.Contains(searchString)));
                  
            }
            
            if (Poison == "on")

            {
                species = species.Where(s => s.Toxicity == "Poisonous");                           
            }

            if (Venom == "on")

            {
                species = species.Where(s => s.Toxicity == "Venomous");
            }

            if (!String.IsNullOrEmpty(Classification))

            {
                species = species.Where(s => s.Class == Classification);
            }

            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(species.OrderBy(s => s.Name).ToPagedList(pageNumber, pageSize));
        }

        // GET: Species/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Species species = db.Species.Find(id);
            if (species == null)
            {
                return HttpNotFound();
            }
            return View(species);
        }

        
        
        // GET: Species/Create
        public ActionResult Create()
        {
            var species = new Species();
            species.Countries = new List<Country>();
            PopulateAssignedCountryData(species);
            return View();
        }




        // POST: Species/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ImgUrl,Name,Class,Bio,Legs,Toxicity")] Species species, string[] selectedCountries)
        {
            if (selectedCountries != null)
            {
                species.Countries = new List<Country>();
                foreach (var ctry in selectedCountries)
                {
                    var ctryToAdd = db.Countries.Find(int.Parse(ctry));
                    species.Countries.Add(ctryToAdd);
                }
            }
            if (ModelState.IsValid)
            { 
                db.Species.Add(species);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateAssignedCountryData(species);
            return View(species);
        }




        // GET: Species/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Species species = db.Species
                .Include(s => s.Countries)
                .Where(i => i.ID == id)
                .Single();  

            if (species == null)
            {
                return HttpNotFound();
            }
            PopulateAssignedCountryData(species);
            return View(species);
        }



        // POST: Species/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, string[] selectedCountries)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var speciesToUpdate = db.Species
                .Include(s => s.Countries)
                .Where(i => i.ID == id)
                .Single();
            if (TryUpdateModel(speciesToUpdate, "",
                    new string[] { "ID", "ImgUrl", "Name", "Class", "Bio", "Legs", "Toxicity" }))
            {
                try
                {
                    UpdateSpeciesCountries(selectedCountries, speciesToUpdate);

                    db.Entry(speciesToUpdate).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes, please try again later or contact site owner");
                }
            }

            PopulateAssignedCountryData(speciesToUpdate);
            return View(speciesToUpdate);
        }




        // GET: Species/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Species species = db.Species.Find(id);
            if (species == null)
            {
                return HttpNotFound();
            }
            return View(species);
        }

        // POST: Species/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Species species = db.Species.Find(id);
            db.Species.Remove(species);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        private void UpdateSpeciesCountries(string[] selectedCountries, Species speciesToUpdate)
        {
            if (selectedCountries == null)
            {
                speciesToUpdate.Countries = new List<Country>();
                return; 
            }
            var selectedCountriesHS = new HashSet<string>(selectedCountries);
            var speciesCountries = new HashSet<int>
                (speciesToUpdate.Countries.Select(c => c.ID));
            foreach ( var ctry in db.Countries)
            {
                if (selectedCountriesHS.Contains(ctry.ID.ToString()))
                {
                    if (!speciesCountries.Contains(ctry.ID))
                    {
                        speciesToUpdate.Countries.Add(ctry);    
                    }
                }
                else
                {
                    if (speciesCountries.Contains(ctry.ID))
                    {
                        speciesToUpdate.Countries.Remove(ctry);
                    }
                }
            }
           

        }




        private void PopulateAssignedCountryData(Species species)
        {
            var allCtrs = db.Countries; //allCtrs = All Countries in database
            var speciesCtrs = new HashSet<int>(species.Countries.Select(c => c.ID)); //speciesCtrs is list storing the species countries by ID
            var viewModelAvailable = new List<SpeciesCountryVM>(); //viewModel is list of type SpeciesCountryVM, which is class to store info of country
            var viewModelSelected = new List<SpeciesCountryVM>();
            foreach (var ctry in allCtrs) // foreach country in database
            {
                if (speciesCtrs.Contains(ctry.ID))
                {
                    viewModelSelected.Add(new SpeciesCountryVM
                    {
                        CountryID = ctry.ID,
                        CountryName = ctry.Name,
                        //Assigned = true
                    });
                }
                else
                {
                    viewModelAvailable.Add(new SpeciesCountryVM
                    {
                        CountryID = ctry.ID,
                        CountryName = ctry.Name,
                        //Assigned = false
                    });
                }
            }


            ViewBag.selOpts = new MultiSelectList(viewModelSelected.OrderBy(c => c.CountryName), "CountryID", "CountryName");
            ViewBag.availOpts = new MultiSelectList(viewModelAvailable.OrderBy(c => c.CountryName), "CountryID", "CountryName");

            
            
        }

        protected override void Dispose(bool disposing) 
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
