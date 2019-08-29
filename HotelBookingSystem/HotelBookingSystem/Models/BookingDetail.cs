using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystem.Models
{
    public partial class BookingDetail
    {
        public int BookId { get; set; }
        [Required(ErrorMessage = "Hotel field cannot be empty")]

        public int HotelId { get; set; }
        [Required(ErrorMessage = "Room field cannot be empty")]
        public int Rooms { get; set; }

        [Required(ErrorMessage = "Date field cannot be empty")]
        [DataType(DataType.Date)]
        [Display(Name = "Booking Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime BookDate { get; set; }

        public int UserId { get; set; }
        public string Status { get; set; }
    
        public virtual HotelDetail HotelDetail { get; set; }
        public virtual UserDetail UserDetail { get; set; }
    }
}
