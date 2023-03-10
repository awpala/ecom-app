using System.Net;
using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
  public class ProductsController : BaseApiController
  {
    private readonly IGenericRepository<Product> _productsRepo;
    private readonly IGenericRepository<ProductBrand> _productBrandRepo;
    private readonly IGenericRepository<ProductType> _productTypeRepo;
    private readonly IMapper _mapper;

    public ProductsController(
      IGenericRepository<Product> productsRepo,
      IGenericRepository<ProductBrand> productBrandRepo,
      IGenericRepository<ProductType> productTypeRepo,
      IMapper mapper
    )
    {
      _productsRepo = productsRepo;
      _productBrandRepo = productBrandRepo;
      _productTypeRepo = productTypeRepo;
      _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<Pagination<ProductsWithTypesAndBrandsSpecification>>> GetProducts([FromQuery]ProductSpecParams productParams)
    {
      var spec = new ProductsWithTypesAndBrandsSpecification(productParams);
      var products = await _productsRepo.ListAsync(spec);
      var count = new ProductWithFiltersForCountSpecification(productParams);
      var totalItems = await _productsRepo.CountAsync(count);
      var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
      var paginatedProductsToReturnDto = new Pagination<ProductToReturnDto>(
        productParams.PageIndex,
        productParams.PageSize,
        totalItems,
        data
      );
      return Ok(paginatedProductsToReturnDto);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
    {
      var spec = new ProductsWithTypesAndBrandsSpecification(id);
      var product = await _productsRepo.GetEntityWithSpec(spec);
      var productToReturnDto = _mapper.Map<Product, ProductToReturnDto>(product);
      
      if (productToReturnDto == null) {
        var errorCode = (int)HttpStatusCode.NotFound;
        return NotFound(new ApiResponse(errorCode));
      }

      return Ok(productToReturnDto);
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
    {
      var brands = await _productBrandRepo.ListAllAsync();
      return Ok(brands);
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
    {
      var types = await _productTypeRepo.ListAllAsync();
      return Ok(types);
    }
  }
}
