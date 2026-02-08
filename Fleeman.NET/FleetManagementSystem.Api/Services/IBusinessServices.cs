using System.Collections.Generic;
using FleetManagementSystem.Api.Models;
using System.Threading.Tasks;

namespace FleetManagementSystem.Api.Services;

public interface IAddOnService
{
    Task<List<AddOnMaster>> GetAllAddOnsAsync();
    Task<AddOnMaster> AddAddOnAsync(AddOnMaster addOn);
    Task<AddOnMaster> GetAddOnByIdAsync(int id); 
    Task DeleteAddOnAsync(int id);
}

public interface IVendorService
{
    Task<List<Vendor>> GetAllVendorsAsync();
    Task<Vendor> AddVendorAsync(Vendor vendor);
    Task DeleteVendorAsync(int id);
}

public interface ICustomerService
{
    Task<List<CustomerMaster>> GetAllCustomersAsync();
    Task<CustomerMaster> GetCustomerByEmailAsync(string email);
    Task<CustomerMaster> GetCustomerByIdAsync(int id);
    Task<CustomerMaster> AddCustomerAsync(CustomerMaster customer);
    Task<CustomerMaster> UpdateCustomerAsync(CustomerMaster customer); 
}
