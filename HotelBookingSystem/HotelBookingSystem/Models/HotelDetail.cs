using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace HotelBookingSystem.Models
{
    public partial class HotelDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HotelDetail()
        {
            this.BookingDetails = new HashSet<BookingDetail>();
        }
    
        public int HotelId { get; set; }
        [Required(ErrorMessage = "Hotel name cannot be empty")]
        [RegularExpression("^([a-zA-Z '-]+)(?<!\\s)$", ErrorMessage = "Please enter valid Name (Alphabets only)")]
        public string HotelName { get; set; }

        [Required(ErrorMessage = "Category cannot be empty")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Description cannot be empty")]
        public string HotelDescription { get; set; }
        public string HotelImage { get; set; }
    
        public virtual HotelCategory HotelCategory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BookingDetail> BookingDetails { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
    }
}
