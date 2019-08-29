using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystem.Models
{
    public partial class HotelCategory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HotelCategory()
        {
            this.HotelDetails = new HashSet<HotelDetail>();
        }
    
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Category field cannot be empty")]
        public string CategoryName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HotelDetail> HotelDetails { get; set; }
    }
}
