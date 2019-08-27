using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using HotelBookingSystem.Models;

namespace HotelBookingSystem.Controllers
{
    public class CancelledBookingsController : Controller
    {
        private HotelBookingSystemEntities db = new HotelBookingSystemEntities();

        #region "Cancelled request"
        /// <summary>
        /// Displaying request's for cancellation of bookings by users
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var bookingDetails = db.BookingDetails.Include(b => b.HotelDetail).Include(b => b.UserDetail);
            var statustype = from s in db.BookingDetails select s;
            statustype = statustype.Where(s => s.Status.Contains("Cancel"));
            return View(statustype.ToList());
        }
        #endregion

        #region "Delete request"
        /// <summary>
        /// Deleting the users booking cancellation request
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookingDetail bookingDetail = db.BookingDetails.Find(id);
            if (bookingDetail == null)
            {
                return HttpNotFound();
            }
            return View(bookingDetail);
        }
      
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BookingDetail bookingDetail = db.BookingDetails.Find(id);
            db.BookingDetails.Remove(bookingDetail);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

        #region "Disposing"
        /// <summary>
        /// Disposing
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}
