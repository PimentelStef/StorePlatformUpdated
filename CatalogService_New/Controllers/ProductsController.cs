using Microsoft.AspNetCore.Mvc;
using CatalogService.Models;

namespace CatalogService.Controllers;

[ApiController]
[Route("catalog/v1/products")]
public class ProductsController : ControllerBase
{
    private static readonly List<Product> products = new();

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(products);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        var product = products.FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            return NotFound(new ProblemDetails
            {
                Status = 404,
                Title = "Product Not Found",
                Detail = "The requested product was not found."
            });
        }

        return Ok(product);
    }

    [HttpPost]
    public IActionResult Create(ProductInput input)
    {
        if (string.IsNullOrWhiteSpace(input.Name) || input.Price <= 0)
        {
            return BadRequest(new ProblemDetails
            {
                Status = 400,
                Title = "Validation Error",
                Detail = "Name is required and price must be greater than zero."
            });
        }

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = input.Name,
            Description = input.Description,
            Price = input.Price
        };

        products.Add(product);

        return CreatedAtAction(
            nameof(GetById),
            new { id = product.Id },
            product);
    }

    [HttpPut("{id:guid}")]
    public IActionResult Update(Guid id, ProductInput input)
    {
        if (string.IsNullOrWhiteSpace(input.Name) || input.Price <= 0)
        {
            return BadRequest(new ProblemDetails
            {
                Status = 400,
                Title = "Validation Error",
                Detail = "Name is required and price must be greater than zero."
            });
        }

        var product = products.FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            return NotFound(new ProblemDetails
            {
                Status = 404,
                Title = "Product Not Found",
                Detail = "The requested product was not found."
            });
        }

        product.Name = input.Name;
        product.Description = input.Description;
        product.Price = input.Price;

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        var product = products.FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            return NotFound(new ProblemDetails
            {
                Status = 404,
                Title = "Product Not Found",
                Detail = "The requested product was not found."
            });
        }

        products.Remove(product);

        return NoContent();
    }
}