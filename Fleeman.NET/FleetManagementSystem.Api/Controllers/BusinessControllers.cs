using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using FleetManagementSystem.Api.Models;
using FleetManagementSystem.Api.Services;
using FleetManagementSystem.Api.DTOs;

namespace FleetManagementSystem.Api.Controllers;

[ApiController]
[Route("api/v1")] 
public class AddOnController : ControllerBase
{
    private readonly IAddOnService _addOnService;
    public AddOnController(IAddOnService addOnService) => _addOnService = addOnService;

    [HttpGet("addons")]
    public async Task<ActionResult<List<AddOnMaster>>> GetAllAddOns() => Ok(await _addOnService.GetAllAddOnsAsync());

    [HttpPost("addons")]
    public async Task<ActionResult<AddOnMaster>> AddAddOn([FromBody] AddOnMaster addOn) => Ok(await _addOnService.AddAddOnAsync(addOn));
    
    [HttpDelete("addons/{id}")]
    public async Task<IActionResult> DeleteAddOn(int id)
    {
        await _addOnService.DeleteAddOnAsync(id);
        return Ok(new MessageResponse("Deleted successfully"));
    }
}

[ApiController]
[Route("api/v1")]
public class VendorController : ControllerBase
{
    private readonly IVendorService _vendorService;
    public VendorController(IVendorService vendorService) => _vendorService = vendorService;

    [HttpGet("vendors")]
    public async Task<ActionResult<List<Vendor>>> GetAllVendors() => Ok(await _vendorService.GetAllVendorsAsync());

    [HttpPost("vendors")]
    public async Task<ActionResult<Vendor>> AddVendor([FromBody] Vendor vendor) => Ok(await _vendorService.AddVendorAsync(vendor));

    [HttpDelete("vendors/{id}")]
    public async Task<IActionResult> DeleteVendor(int id)
    {
        await _vendorService.DeleteVendorAsync(id);
        return Ok(new MessageResponse("Deleted successfully"));
    }
}

[ApiController]
[Route("api/v1")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;
    public CustomerController(ICustomerService customerService) => _customerService = customerService;

    [HttpGet("customers")]
    public async Task<ActionResult<List<CustomerMaster>>> GetAllCustomers() => Ok(await _customerService.GetAllCustomersAsync());

    [HttpGet("customers/{email}")]
    public async Task<ActionResult<CustomerMaster>> GetCustomerByEmail(string email)
    {
        var cust = await _customerService.GetCustomerByEmailAsync(email);
        if (cust != null) return Ok(cust);
        return NotFound();
    }
    
    [HttpPost("customers")]
    public async Task<ActionResult<CustomerMaster>> AddCustomer([FromBody] CustomerMaster customer)
    {
        return Ok(await _customerService.AddCustomerAsync(customer));
    }
    
    [HttpGet("customers/id/{id}")]
    public async Task<ActionResult<CustomerMaster>> GetCustomerById(int id)
    {
        var cust = await _customerService.GetCustomerByIdAsync(id);
        if (cust != null) return Ok(cust);
        return NotFound();
    }

    [HttpPut("customers/{id}")]
    public async Task<ActionResult<CustomerMaster>> UpdateCustomer(int id, [FromBody] CustomerMaster customer)
    {
        customer.CustId = id;
        var updated = await _customerService.UpdateCustomerAsync(customer);
        if (updated != null) return Ok(updated);
        return NotFound();
    }
}
