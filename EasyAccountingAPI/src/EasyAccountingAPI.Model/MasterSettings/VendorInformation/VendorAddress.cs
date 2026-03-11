namespace EasyAccountingAPI.Model.MasterSettings.VendorInformation
{
    [Table("VendorAddresses", Schema = "MasterSettings")]
    public class VendorAddress
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vendor is required.")]
        public int VendorId { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "Address must be 5 to 500 character.")]
        public string Address { get; set; }

        public string? Fax { get; set; }
        public string? Zip { get; set; }
        public string? Website { get; set; }
        public string? Notes { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public int CityId { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        public int CountryId { get; set; }

        public Vendor Vendor { get; set; }  
        public City City { get; set; }
        public Country Country { get; set; }
    }
}