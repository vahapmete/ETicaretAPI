using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.RequestParameters;
using ETicaretAPI.Application.Services;
using ETicaretAPI.Application.Services.Storage;
using ETicaretAPI.Application.Services.Storage.Local;
using ETicaretAPI.Application.ViewModels.Products;
using ETicaretAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ETicaretAPI.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        readonly private IProductWriteRepository _productWriteRepository;
        readonly private IProductReadRepository _productReadRepository;
        readonly private IWebHostEnvironment _webHostEnvironment;
        readonly private IFileWriteRepository _fileWriteRepository;
        readonly private IFileReadRepository _fileReadRepository;
        readonly private IProductImageFileReadRepository _productImageFileReadRepository;
        readonly private IProductImageFileWriteRepository _productImageFileWriteRepository;
        readonly private IInvoiceFileReadRepository _invoiceFileReadRepository;
        readonly private IInvoiceFileWriteRepository _invoiceFileWriteRepository;
        readonly private IStorageService _storageService;
        readonly IConfiguration configuration;
        public ProductsController(
            IProductWriteRepository productWriteRepository,
            IProductReadRepository productReadRepository,
            IWebHostEnvironment webHostEnvironment,
            IFileWriteRepository fileWriteRepository,
            IFileReadRepository fileReadRepository,
            IProductImageFileReadRepository productImageFileReadRepository,
            IProductImageFileWriteRepository productImageFileWriteRepository,
            IInvoiceFileReadRepository invoiceFileReadRepository,
            IInvoiceFileWriteRepository invoiceFileWriteRepository,
            IStorageService storageService
,
            IConfiguration configuration)
        {
            _webHostEnvironment = webHostEnvironment;
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _fileWriteRepository = fileWriteRepository;
            _fileReadRepository = fileReadRepository;
            _productImageFileReadRepository = productImageFileReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _invoiceFileReadRepository = invoiceFileReadRepository;
            _invoiceFileWriteRepository = invoiceFileWriteRepository;
            _storageService = storageService;
            this.configuration = configuration;
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
        public async Task<IActionResult> Upload(string id)
        {

           List<(string fileName, string pathOrContainerName)> result = await _storageService.UploadAsync("products-images", Request.Form.Files);


            Product product = await _productReadRepository.GetByIdAsync(id);
             await _productImageFileWriteRepository.AddRangeAsync(result.Select(d => new ProductImageFile()
             {
                FileName = d.fileName,
                Path = d.pathOrContainerName,
                Storage=_storageService.StorageName,
                Products= new List<Product>() { product}
             }).ToList());
             await _productImageFileWriteRepository.SaveAsync();

            return Ok();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetProductImages(string id)
        {
            Product? product = await _productReadRepository.Table.Include(p => p.ProductImageFiles).FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));
            return Ok(product.ProductImageFiles.Select(p => new
            {
                Path = $"{configuration["BaseStorageUrl"]}/{p.Path}",
                p.FileName,
                p.Id
            }));
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteProductImage(string id,string imageId )
        {
            Product? product = await _productReadRepository.Table.Include(p => p.ProductImageFiles).FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));

            ProductImageFile imageFile=product.ProductImageFiles.FirstOrDefault(i=>i.Id==Guid.Parse(imageId));
            product.ProductImageFiles.Remove(imageFile);
            await _productWriteRepository.SaveAsync();
            return Ok();
        }

    }
}
 