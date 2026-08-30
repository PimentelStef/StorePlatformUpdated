using Microsoft.AspNetCore.Mvc;
using OrderService.Models;

namespace OrderService.Controllers;

[ApiController]
[Route("orders/v1/orders")]
public class OrdersController : ControllerBase
{
    private static readonly List<Order> orders = new();

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(orders);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        var order = orders.FirstOrDefault(o => o.Id == id);

        if (order == null)
        {
            return NotFound(new ProblemDetails
            {
                Status = 404,
                Title = "Order Not Found",
                Detail = "The requested order was not found."
            });
        }

        return Ok(order);
    }

    [HttpGet("{id:guid}/items")]
    public IActionResult GetItems(Guid id)
    {
        var order = orders.FirstOrDefault(o => o.Id == id);

        if (order == null)
        {
            return NotFound(new ProblemDetails
            {
                Status = 404,
                Title = "Order Not Found",
                Detail = "The requested order was not found."
            });
        }

        return Ok(order.Items);
    }

    [HttpPost]
    public IActionResult Create(OrderInput input)
    {
        if (string.IsNullOrWhiteSpace(input.CustomerName) ||
            input.Items.Count == 0)
        {
            return BadRequest(new ProblemDetails
            {
                Status = 400,
                Title = "Validation Error",
                Detail = "Customer name and at least one item are required."
            });
        }

        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerName = input.CustomerName,
            Items = input.Items.Select(item => new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            }).ToList()
        };

        orders.Add(order);

        return CreatedAtAction(
            nameof(GetById),
            new { id = order.Id },
            order);
    }

    [HttpPut("{id:guid}")]
    public IActionResult Update(Guid id, OrderInput input)
    {
        if (string.IsNullOrWhiteSpace(input.CustomerName) ||
            input.Items.Count == 0)
        {
            return BadRequest(new ProblemDetails
            {
                Status = 400,
                Title = "Validation Error",
                Detail = "Customer name and at least one item are required."
            });
        }

        var order = orders.FirstOrDefault(o => o.Id == id);

        if (order == null)
        {
            return NotFound(new ProblemDetails
            {
                Status = 404,
                Title = "Order Not Found",
                Detail = "The requested order was not found."
            });
        }

        order.CustomerName = input.CustomerName;

        order.Items = input.Items.Select(item => new OrderItem
        {
            ProductId = item.ProductId,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice
        }).ToList();

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        var order = orders.FirstOrDefault(o => o.Id == id);

        if (order == null)
        {
            return NotFound(new ProblemDetails
            {
                Status = 404,
                Title = "Order Not Found",
                Detail = "The requested order was not found."
            });
        }

        orders.Remove(order);

        return NoContent();
    }
}