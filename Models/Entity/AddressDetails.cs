namespace InventoryManagementSystem.Models.Entity
{
    /// <summary>
    /// Represents the complete address information of a user.
    /// </summary>
    public class AddressDetails
    {
        /// <summary>
        /// Unique identifier for the address record.
        /// </summary>
        public long AddressId { get; set; }

        /// <summary>
        /// Identifier of the user associated with this address.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Contact phone number linked to this address.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Full street address or house location.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// City where the address is located.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// State or province of the address.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Country of residence for this address.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Postal code or ZIP code of the location.
        /// </summary>
        public string Pincode { get; set; }

        public Users User { get; set; }
    }
}
