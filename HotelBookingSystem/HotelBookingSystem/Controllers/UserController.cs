using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;
using HotelBookingSystem.Models;

namespace HotelBookingSystem.Controllers
{
    public class UserController : Controller
    {
        private HotelBookingSystemEntities db = new HotelBookingSystemEntities();

        #region "Displaying list of hotels"
        /// <summary>
        /// Displaying list of hotels with pagination
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (Session["UserRole"] != null && Session["UserRole"].ToString() == "2")
            {
                ViewBag.CategoryId = new SelectList(db.HotelCategories, "CategoryId", "CategoryName");
                var hotelDetails = db.HotelDetails.Include(h => h.HotelCategory);
                return View(hotelDetails.ToList());
            }
            else
            {
                Response.Write("<script>alert('Session logged out. Sign in again');</script>");
                FormsAuthentication.SignOut();
                Session.Clear();
                return RedirectToAction("SignIn", "Auth");
            }
        }

        /// <summary>
        /// Search list of hotels according to their category
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(int CategoryId)
        {
            if (Session["UserRole"] != null && Session["UserRole"].ToString() == "2")
            {
                ViewBag.CategoryId = new SelectList(db.HotelCategories, "CategoryId", "CategoryName");
                var hotelDetails = db.HotelDetails.Include(h => h.HotelCategory).Where(x => x.CategoryId == CategoryId);
                return View(hotelDetails.ToList());
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
            if (Session["UserRole"] != null && Session["UserRole"].ToString() == "2")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                HotelDetail hotelDetail = db.HotelDetails.Find(id);
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

        #region "Book Hotel"
        /// <summary>
        /// Form for booking a hotel
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            if (Session["UserRole"] != null && Session["UserRole"].ToString() == "2")
            {
                ViewBag.UserName = new SelectList(db.UserDetails.Where(u => u.UserTypeId == 2), "FirstName", "FirstName");
                ViewBag.CategoryId = new SelectList(db.HotelCategories, "CategoryId", "CategoryName");
                ViewBag.HotelId = new SelectList(db.HotelDetails, "HotelId", "HotelName");
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
        public ActionResult Create(BookingDetail bookingDetail)
        {
            if (Session["UserRole"] != null && Session["UserRole"].ToString() == "2")
            {
                if (ModelState.IsValid)
                {
                    bookingDetail.UserId = Convert.ToInt32(Session["UserId"]);
                    bookingDetail.Status = "Book";
                    db.BookingDetails.Add(bookingDetail);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.UserName = new SelectList(db.UserDetails.Where(u => u.UserTypeId == 2), "FirstName", "FirstName");
                ViewBag.HotelId = new SelectList(db.HotelDetails, "HotelId", "HotelName", bookingDetail.HotelId);
                return View(bookingDetail);
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

        #region "Hotels booked by user"
        /// <summary>
        /// Displaying reservation tokens for hotels booked by user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult BookedHotels(int? id)
        {
            if (Session["UserRole"] != null && Session["UserRole"].ToString() == "2")
            {
                var bookingDetails = db.BookingDetails.Include(o => o.UserDetail);
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }              
                List<BookingDetail> orderDetail = db.BookingDetails.Where(u => u.UserId == id).OrderByDescending(u => u.BookDate).ToList();
                return View(orderDetail);
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
        public ActionResult BookedHotels()
        {
            if (Session["UserRole"] != null && Session["UserRole"].ToString() == "2")
            {
                var bookings = db.BookingDetails.AsQueryable();
                var id = Convert.ToInt32(Session["UserId"]);   
                return View(bookings.Include(e => e.UserDetail).Where(x => x.UserId == id).OrderByDescending(u => u.BookDate).ToList());
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

        #region "Send Cancel Booking Request"
        /// <summary>
        /// GET : Get Booking Id  Which is to be cancelled
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
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

        /// <summary>
        /// POST : Change the booking status "Book" to "Cancel"
        /// </summary>
        /// <param name="bookingDetail"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookId,HotelId,Rooms,BookDate,UserId,Status")] BookingDetail bookingDetail)
        {
            if (ModelState.IsValid)
            {
                BookingDetail booking = db.BookingDetails.Where(x => x.BookId == bookingDetail.BookId).FirstOrDefault();
                var status = " Cancel";
                booking.Status = status;                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bookingDetail);
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
