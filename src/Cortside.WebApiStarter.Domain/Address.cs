using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cortside.WebApiStarter.Domain {
    [Table("Address")]
    public class Address : IImmutableEntity {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AddressId { get; set; }

        [StringLength(100)]
        public string Address1 { get; set; }

        [StringLength(100)]
        public string Address2 { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(2)]
        public string State { get; set; }

        [StringLength(9)]
        public string ZipCode { get; set; }

        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string UniqueKey {
            get {
                var cols = new string[] {
                    Address1.Trim() ?? "",
                    Address2.Trim() ?? "",
                    City.Trim() ?? "",
                    State.Trim() ?? "",
                    ZipCode.Trim() ?? ""
                };
                return string.Join("|", cols).ToLower();
            }
        }
    }
}
