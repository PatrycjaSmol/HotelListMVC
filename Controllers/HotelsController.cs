using HotelListMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListMVC.Controllers
{
    public class HotelsController : Controller
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public Hotel Hotel { get; set; } 

        public HotelsController(ApplicationDbContext db) => _db = db;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Hotel = new Hotel();
            if (id == null)
            {
                //create
                return View(Hotel);
            }
            //update
            Hotel = _db.Hotels.FirstOrDefault(u => u.Id == id);
            if(Hotel == null)
            {
                return NotFound();  
            }
            return View(Hotel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert()
        {
           if(ModelState.IsValid)
            {
                if (Hotel.Id == 0)
                {
                    //create
                    _db.Hotels.Add(Hotel);
                }
                else
                {
                    _db.Hotels.Update(Hotel);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Hotel);
        }

        #region API Calls
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.Hotels.ToListAsync() });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var hotelFromDb = await _db.Hotels.FirstOrDefaultAsync(u => u.Id == id);
            if (hotelFromDb == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
            _db.Hotels.Remove(hotelFromDb);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Delete successful" });
        }
        #endregion
    }

}
