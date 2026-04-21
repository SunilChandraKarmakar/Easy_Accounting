namespace EasyAccountingAPI.Shared.EnumConfiguration
{
    public static class BaseEnumConfiguration
    {
        public static class DslFactoryErp
        {
            public readonly static int GlobalStatus = 1;    
            public readonly static int PaymentStatus = 2;
            public readonly static int PaymentMethod = 3;
        }
    }

    public static class BaseEnumCollection
    {
        public static class GlobalStatus
        {
            public readonly static int Active = 1;
            public readonly static int Inactive = 2;
        }

        public static class PaymentStatus
        {
            public readonly static int Paid = 3;
            public readonly static int Partial = 4;
            public readonly static int Unpaid = 5;
        }

        public static class PaymentMethod
        {
            public readonly static int BankPayment = 6;
            public readonly static int Cheque = 7;
            public readonly static int CreditCard = 8;
            public readonly static int PayPal = 9;
            public readonly static int Other = 10;
        }
    }
}