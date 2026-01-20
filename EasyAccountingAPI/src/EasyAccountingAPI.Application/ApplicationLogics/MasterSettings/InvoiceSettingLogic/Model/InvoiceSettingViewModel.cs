namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.InvoiceSettingLogic.Model
{
    public class InvoiceSettingViewModel
    {
        public InvoiceSettingCreateModel CreateModel { get; set; }
        public InvoiceSettingUpdateModel UpdateModel { get; set; }
        public InvoiceSettingGridModel GridModel { get; set; }
        public dynamic OptionsDataSources { get; set; } = new ExpandoObject();
    }

    public class InvoiceSettingCreateModel : IMapFrom<InvoiceSetting>
    {
        [Required(ErrorMessage = "Please, provide company.")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Please, provide invoice due date count.")]
        public int InvoiceDueDateCount { get; set; }

        public string? InvoiceColor { get; set; }
        public string? InvoiceFooter { get; set; }
        public string? InvoiceTerms { get; set; }
        public bool IsHideInvoiceHeader { get; set; }
        public bool IsShowPreviousDue { get; set; }
        public bool IsShowInvoiceCreatorName { get; set; }
        public bool IsShowCustomerSignature { get; set; }
        public bool IsCreateInvoiceWithoutPurchase { get; set; }
        public bool IsServiceProviderAttributionUnderInvoice { get; set; }
        public bool IsDefaultInvoiceSetting { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<InvoiceSettingCreateModel, InvoiceSetting>();
            profile.CreateMap<InvoiceSetting, InvoiceSettingCreateModel>();
        }
    }

    public class InvoiceSettingUpdateModel : IMapFrom<InvoiceSetting>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please, provide company.")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Please, provide invoice due date count.")]
        public int InvoiceDueDateCount { get; set; }

        public string? InvoiceColor { get; set; }
        public string? InvoiceFooter { get; set; }
        public string? InvoiceTerms { get; set; }
        public bool IsHideInvoiceHeader { get; set; }
        public bool IsShowPreviousDue { get; set; }
        public bool IsShowInvoiceCreatorName { get; set; }
        public bool IsShowCustomerSignature { get; set; }
        public bool IsCreateInvoiceWithoutPurchase { get; set; }
        public bool IsServiceProviderAttributionUnderInvoice { get; set; }
        public bool IsDefaultInvoiceSetting { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<InvoiceSettingUpdateModel, InvoiceSetting>();
            profile.CreateMap<InvoiceSetting, InvoiceSettingUpdateModel>();
        }
    }

    public class InvoiceSettingGridModel : IMapFrom<InvoiceSetting>
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public int InvoiceDueDateCount { get; set; }
        public string? InvoiceColor { get; set; }
        public string? InvoiceFooter { get; set; }
        public string? InvoiceTerms { get; set; }
        public bool IsHideInvoiceHeader { get; set; }
        public bool IsShowPreviousDue { get; set; }
        public bool IsShowInvoiceCreatorName { get; set; }
        public bool IsShowCustomerSignature { get; set; }
        public bool IsCreateInvoiceWithoutPurchase { get; set; }
        public bool IsServiceProviderAttributionUnderInvoice { get; set; }
        public bool IsDefaultInvoiceSetting { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<InvoiceSetting, InvoiceSettingGridModel>()
                .ForMember(d => d.CompanyName, s => s.MapFrom(m => m.Company.Name));
            profile.CreateMap<InvoiceSettingGridModel, InvoiceSetting>();
        }
    }
}