using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.RequestParameters;
using ETicaretAPI.Application.ViewModels.Products;
using ETicaretAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ETicaretAPI.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        readonly private IProductWriteRepository _productWriteRepository;
        readonly private IProductReadRepository _productReadRepository;
         private IWebHostEnvironment _webHostEnvironment;
        public ProductsController(
            IProductWriteRepository productWriteRepository,
            IProductReadRepository productReadRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;  
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var data = _productReadRepository.GetByIdAsync(id, false);
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]PaginationModel paginationModel)
        {
            var totalCount = _productReadRepository.GetAll().Count();
            var products = _productReadRepository.GetAll(false).Skip(paginationModel.Page * paginationModel.Size).Take(paginationModel.Size).Select(p=> new
            {
                p.Id,
                p.Name,
                p.Stock,
                p.Price,
                p.CreatedDate,
                p.UpdatedDate
            });
            return Ok(new
            {
                totalCount,
                products
                
            });
        }
         
        [HttpPost]
        public async Task<IActionResult> Post(Product_Create_VM model)
        {
            await _productWriteRepository.AddAsync(new Product
            {
                Name = model.Name,
                Price = model.Price,
                Stock = model.Stock,
            });
            await _productWriteRepository.SaveAsync();
            return Ok((int)HttpStatusCode.Created);
        }

        [HttpPut]
        public async Task<IActionResult> Put(Product_Update_VM model)
        {
            Product product = await _productReadRepository.GetByIdAsync(model.Id);
            product.Name = model.Name;
            product.Price = model.Price;
            product.Stock = model.Stock;
            await _productWriteRepository.SaveAsync();
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _productWriteRepository.Remove(id);
            await _productWriteRepository.SaveAsync();
            return Ok();
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Upload()
        {
            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "resource/product-images");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            Random random = new Random();
            foreach(IFormFile file in Request.Form.Files)
            {
                string fullPath= Path.Combine(uploadPath, $"{random.Next()}{Path.GetExtension(file.Name)}");
                using FileStream fileStream = new(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);
                await file.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
            }
            return Ok();
        }

    }
}
 