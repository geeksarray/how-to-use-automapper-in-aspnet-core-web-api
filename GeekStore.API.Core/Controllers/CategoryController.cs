using AutoMapper;
using AutoMapper.QueryableExtensions;
using GeekStore.API.Core.Data.Models;
using GeekStore.API.Core.DTOs.Category;
using GeekStore.API.Core.DTOs.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace GeekStore.API.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryController> _logger;
        private readonly GeeksStoreContext _context;

        public CategoryController(IMapper mapper,
            ILogger<CategoryController> logger,
            GeeksStoreContext context)
        {
            this._mapper = mapper;
            this._logger = logger;
            this._context = context;
        }

        [HttpGet]

        public async Task<ActionResult<List<GetCategoryDetailsDto>>> GetCategories()
        {
            var countries = await this._context.Categories.ToListAsync();
            var records = _mapper.Map<List<GetCategoryDetailsDto>>(countries);
            return Ok(records);
        }

        [HttpGet("GetCategoryProducts/{categoryID}")]
        public async Task<ActionResult<GetCategoryDetailsDto>> GetCategoryProducts(int categoryID)
        {

            var products = await this._context.Categories.Where(c => c.Id == categoryID)
                .Include(_ => _.Products).ToListAsync();

            var result = _mapper.Map<List<Category>, List<GetCategoryProductsDto>>(products);
            return Ok(result);
        }



        [HttpGet("GetProductDetails/{productID}")]

        public async Task<ActionResult<ProductDto>> GetProductDetails(int productID)
        {
            var product = await _context.Products.FindAsync(productID);

            var category =
                    this._context.Products.Where(p => p.Id == productID).Include(c => c.Category);

            if (product == null)
            {
                throw new Exception($"Product {productID} is not found.");
            }
            var productDto = this._mapper.Map<ProductDto>(product);

            return Ok(productDto);
        }

        [HttpGet("GetCategoryDetails/{categoryID}")]
        public async Task<ActionResult<GetCategoryDetailsDto>> GetCategoryDetails(int categoryID)
        {
            //var country = await  _countriesRepository.GetDetails(id);
            var category = await this._context.Categories.FindAsync(categoryID);
            if (category == null)
            {
                throw new Exception($"CategoryID {categoryID} is not found.");
            }

            var categoryDto = this._mapper.Map<GetCategoryDetailsDto>(category);

            return Ok(categoryDto);
        }

        [HttpPut("{id}")]
        
        public async Task<IActionResult> UpdateCategoryDiscount(int id, 
            UpdateCategoryDiscountDto updateCategoryDiscount)
        {
            var category = await this._context.Categories.FindAsync(id);

            if (category == null)
            {
                throw new Exception($"CategoryID {id} is not found.");
            }

           
            this._mapper.Map(updateCategoryDiscount, category);
            

            var result  = this._context.Categories.Update(category);
            
            await this._context.SaveChangesAsync();

            return Ok();
        }
    }
}
