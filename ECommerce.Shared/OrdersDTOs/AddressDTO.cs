namespace ECommerce.Shared.OrdersDTOs
{
    public record AddressDTO
    {
        public string Street { get; init; } = default!;
        public string City { get; init; } = default!;
        public string Country { get; init; } = default!;
        public string FirstName { get; init; } = default!;
        public string LastName { get; init; } = default!;
    }
}