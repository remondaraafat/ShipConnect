namespace ShipConnect.Enums
{
    public class Enums
    {
        public enum UserRole
        {
            Startup,
            ShippingCompany,
            Admin
        }

        public enum TransportType
        {
            Land,     // بري
            Sea,      // بحري
            Air       // جوي
        }

        public enum PackagingOptions
        {
            Standard,
            FragileProtection ,
            ThermalPackaging,
            Other
        }
        public enum ShippingScope
        {
            Domestic,     // داخلي
            International // خارجي
        }

        public enum ShipmentStatus
        {
            Pending,
            Preparing,
            InTransit,
            AtWarehouse,
            OutForDelivery,
            Delivered,
            Failed
        }

        public enum PaymentMethod
        {
            Cash,
            CreditCard,
            BankTransfer,
            Wallet
        }

        public enum PaymentStatus
        {
            Pending,
            Completed,
            Failed,
            Refunded
        }

        public enum NotificationType
        {
            General,
            NewOffer,
            OfferAccepted,
            NewMessage,
            PaymentReceived,
            ShipmentStatusChanged,
            ShipmentDelivered,
            RatingReceived
        }
    }
}
