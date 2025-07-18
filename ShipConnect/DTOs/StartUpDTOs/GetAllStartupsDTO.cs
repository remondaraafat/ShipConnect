namespace ShipConnect.DTOs.StartUpDTOs
{
    public class GetAllStartupsDTO:GetAllUsersDTO
    {

        public string? Description { get; set; }


        public string? BusinessCategory { get; set; }


        public string Address { get; set; } = string.Empty;
        public string? Website { get; set; }

        public string Phone { get; set; } = string.Empty;


        public string Email { get; set; }

        public string? TaxId { get; set; }
    }
}

