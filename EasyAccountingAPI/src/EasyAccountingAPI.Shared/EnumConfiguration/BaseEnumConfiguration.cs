namespace EasyAccountingAPI.Shared.EnumConfiguration
{
    public static class BaseEnumConfiguration
    {
        public static class DslFactoryErp
        {
            public readonly static int CompanyStatus = 1;            
        }
    }

    public static class BaseEnumCollection
    {
        public static class CompanyStatus
        {
            public readonly static int Active = 1;
            public readonly static int Inactive = 2;
        }
    }
}