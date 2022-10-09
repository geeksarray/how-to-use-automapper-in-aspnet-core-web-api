namespace GeekStore.API.Core.DTOs.Category
{
    public class CreateCategoryDto
    {
        public int Id { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int CategoryDiscount { get; set; }
    }
}
