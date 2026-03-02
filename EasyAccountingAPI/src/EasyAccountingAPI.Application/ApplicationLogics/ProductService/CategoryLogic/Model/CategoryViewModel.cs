namespace EasyAccountingAPI.Application.ApplicationLogics.ProductService.CategoryLogic.Model
{
    public class CategoryViewModel
    {
        public CategoryCreateModel CreateModel { get; set; }    
        public CategoryUpdateModel UpdateModel { get; set; }
        public CategoryGridModel GridModel { get; set; }
        public dynamic OptionsDataSources { get; set; } = new ExpandoObject();
    }

    public class CategoryCreateModel : IMapFrom<Category>
    {
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
        public string Name { get; set; }

        public int? ParentId { get; set; }

        [Required(ErrorMessage = "CompanyId is required.")]
        public int CompanyId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CategoryCreateModel, Category>();
        }
    }

    public class CategoryUpdateModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
        public string Name { get; set; }

        public int? ParentId { get; set; }

        [Required(ErrorMessage = "CompanyId is required.")]
        public int CompanyId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Category, CategoryUpdateModel>();
            profile.CreateMap<CategoryUpdateModel, Category>();
        }
    }

    public class CategoryGridModel : IMapFrom<Category>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? ParentName { get; set; }
        public string CompanyName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Category, CategoryGridModel>()
                .ForMember(d => d.ParentName, s => s.MapFrom(m => m.ParentCategory.Name))
                .ForMember(d => d.CompanyName, s => s.MapFrom(m => m.Company.Name));
        }
    }
}