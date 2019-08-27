using System;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;
using HotelBookingSystem.Models;
using PagedList;


namespace HotelBookingSystem.Controllers
{
    public class AdminController : Controller
    {
        private HotelBookingSystemEntities db = new HotelBookingSystemEntities();

        #region "Index"
        /// <summary>
        /// Displaying hotels list
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Index(string search,int? page)
        {

            if (Session["UserRole"] != null && Session["UserRole"].ToString() == "1")
            {
                var hotelDetails = db.HotelDetails.Include(h => h.HotelCategory);
                return View(hotelDetails.Where(x => x.HotelName.StartsWith(search) || x.HotelDescription.StartsWith(search) || search == null).ToList().ToPagedList(page ?? 1, 5));
            }
            else
            {
                Response.Write("<script>alert('Session logged out. Sign in again');</script>");
                FormsAuthentication.SignOut();
                Session.Clear();
                return RedirectToAction("SignIn", "Auth");
            }
        }
        #endregion

        #region "Hotel Details"
        /// <summary>
        /// Displaying hotel details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {
            if (Session["UserRole"] != null && Session["UserRole"].ToString() == "1")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                HotelDetail hotelDetail = db.HotelDetails.Find(id);
                hotelDetail = db.HotelDetails.Where(x => x.HotelId == id).FirstOrDefault();
                if (hotelDetail == null)
                {
                    return HttpNotFound();
                }
                return View(hotelDetail);
            }
            else
            {
                Response.Write("<script>alert('Session logged out. Sign in again');</script>");
                FormsAuthentication.SignOut();
                Session.Clear();
                return RedirectToAction("SignIn", "Auth");
            }
        }
        #endregion

        #region "Daily Bookings"
        /// <summary>
        /// Displaying daily bookings of hotel
        /// </summary>
        /// <returns></returns>
        public ActionResult BookingDetails()
        {
            if (Session["UserRole"] != null && Session["UserRole"].ToString() == "1")
            {
                var orders = db.BookingDetails.Where(u => u.BookDate == DateTime.Today);
                return View(orders.ToList());
            }
            else
            {
                Response.Write("<script>alert('Session logged out. Sign in again');</script>");
                FormsAuthentication.SignOut();
                Session.Clear();
                return RedirectToAction("SignIn", "Auth");
            }
        }
        #endregion

        #region "Adding Hotels"
        /// <summary>
        /// Form to add hotels
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            if (Session["UserRole"] != null && Session["UserRole"].ToString() == "1")
            {
                ViewBag.CategoryId = new SelectList(db.HotelCategories, "CategoryId", "CategoryName");
                return View();
            }
            else
            {
                Response.Write("<script>alert('Session logged out. Sign in again');</script>");
                FormsAuthentication.SignOut();
                Session.Clear();
                return RedirectToAction("SignIn", "Auth");
            }
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HotelDetail hotelDetail)
        {
            if (Session["UserRole"] != null && Session["UserRole"].ToString() == "1")
            {
                if (ModelState.IsValid)
                {
                    string fileName = Path.GetFileName(hotelDetail.ImageFile.FileName);
                    hotelDetail.HotelImage = "~/Image/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Image"), fileName);
                    hotelDetail.ImageFile.SaveAs(fileName);
                    db.HotelDetails.Add(hotelDetail);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.CategoryId = new SelectList(db.HotelCategories, "CategoryId", "CategoryName", hotelDetail.CategoryId);
                return View(hotelDetail);
            }
            else
            {
                Response.Write("<script>alert('Session logged out. Sign in again');</script>");
                FormsAuthentication.SignOut();
                Session.Clear();
                return RedirectToAction("SignIn", "Auth");
            }
        }
        #endregion

        #region "Disposing"
        /// <summary>
        /// disposing
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
