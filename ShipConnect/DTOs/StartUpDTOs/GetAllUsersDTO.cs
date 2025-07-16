namespace ShipConnect.DTOs.StartUpDTOs
{
    public class GetAllUsersDTO
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; }
        public string? AccountType { get; set; }
        public DateTime RegisterAt { get; set; }
    }
}
