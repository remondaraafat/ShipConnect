namespace ShipConnect.DTOs.AccountDTOs
{
    public class PendingAccountDTO
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ProfileImageUrl { get; set; }
        public string AccountType { get; set; }
        public DateTime RegisterAt { get; set; }
    }
}
