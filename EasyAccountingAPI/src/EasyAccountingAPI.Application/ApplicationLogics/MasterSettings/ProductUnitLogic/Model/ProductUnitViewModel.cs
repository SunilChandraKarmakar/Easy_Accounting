namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.ProductUnitLogic.Model
{
    public class ProductUnitViewModel
    {
        public ProductUnitCreateModel CreateModel { get; set; }
        public ProductUnitUpdateModel UpdateModel { get; set; }
        public ProductUnitGridModel GridModel { get; set; }
    }

    public class ProductUnitCreateModel : IMapFrom<ProductUnit>
    {

        [Required(ErrorMessage = "Unit name is required.")]
        [StringLength(50, ErrorMessage = "Unit name cannot exceed 50 characters.")]
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ProductUnitCreateModel, ProductUnit>();
        }
    }

    public class ProductUnitUpdateModel : IMapFrom<ProductUnit>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Unit name is required.")]
        [StringLength(50, ErrorMessage = "Unit name cannot exceed 50 characters.")]
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ProductUnitUpdateModel, ProductUnit>();
            profile.CreateMap<ProductUnit, ProductUnitUpdateModel>();
        }
    }

    public class ProductUnitGridModel : IMapFrom<ProductUnit>
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Unit name is required.")]
        [StringLength(50, ErrorMessage = "Unit name cannot exceed 50 characters.")]
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ProductUnit, ProductUnitGridModel>();
        }
    }
}