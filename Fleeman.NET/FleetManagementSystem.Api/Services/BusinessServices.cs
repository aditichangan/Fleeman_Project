using System;
using System.Collections.Generic;
using System.Linq;
using FleetManagementSystem.Api.Data;
using FleetManagementSystem.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FleetManagementSystem.Api.Services;

public class AddOnService : IAddOnService
{
    private readonly ApplicationDbContext _context;
    public AddOnService(ApplicationDbContext context) => _context = context;
    public async Task<List<AddOnMaster>> GetAllAddOnsAsync() => await _context.AddOns.ToListAsync();
    public async Task<AddOnMaster> AddAddOnAsync(AddOnMaster addOn)
    {
        _context.AddOns.Add(addOn);
        await _context.SaveChangesAsync();
        return addOn;
    }
    public async Task<AddOnMaster> GetAddOnByIdAsync(int id) => await _context.AddOns.FindAsync(id);
    public async Task DeleteAddOnAsync(int id)
    {
        var addon = await _context.AddOns.FindAsync(id);
        if (addon != null) { _context.AddOns.Remove(addon); await _context.SaveChangesAsync(); }
    }
}

public class VendorService : IVendorService
{
    private readonly ApplicationDbContext _context;
    public VendorService(ApplicationDbContext context) => _context = context;
    public async Task<List<Vendor>> GetAllVendorsAsync() => await _context.Vendors.ToListAsync();
    public async Task<Vendor> AddVendorAsync(Vendor vendor)
    {
        _context.Vendors.Add(vendor);
        await _context.SaveChangesAsync();
        return vendor;
    }
    public async Task DeleteVendorAsync(int id)
    {
        var vendor = await _context.Vendors.FindAsync(id);
        if (vendor != null) { _context.Vendors.Remove(vendor); await _context.SaveChangesAsync(); }
    }
}

public class CustomerService : ICustomerService
{
    private readonly ApplicationDbContext _context;
    public CustomerService(ApplicationDbContext context) => _context = context;

    public async Task<List<CustomerMaster>> GetAllCustomersAsync() => await _context.Customers.ToListAsync();
    
    public async Task<CustomerMaster> GetCustomerByEmailAsync(string email) => await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);
    
    public async Task<CustomerMaster> AddCustomerAsync(CustomerMaster customer)
    {
        // Check if customer exists by Email to prevent duplicates
        var existingCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == customer.Email);
        if (existingCustomer != null)
        {
             // Update existing customer fields
             existingCustomer.FirstName = customer.FirstName;
             existingCustomer.LastName = customer.LastName;
             existingCustomer.MobileNumber = customer.MobileNumber;
             existingCustomer.AddressLine1 = customer.AddressLine1;
             
             // Handle nullable/optional updates
             if (!string.IsNullOrEmpty(customer.AddressLine2)) existingCustomer.AddressLine2 = customer.AddressLine2;
             if (!string.IsNullOrEmpty(customer.City)) existingCustomer.City = customer.City;
             if (!string.IsNullOrEmpty(customer.Pincode)) existingCustomer.Pincode = customer.Pincode;
             // Ensure PhoneNumber is set
             existingCustomer.PhoneNumber = !string.IsNullOrEmpty(customer.PhoneNumber) 
                                             ? customer.PhoneNumber 
                                             : (!string.IsNullOrEmpty(customer.MobileNumber) ? customer.MobileNumber : existingCustomer.PhoneNumber);

             if (!string.IsNullOrEmpty(customer.DrivingLicenseNumber)) existingCustomer.DrivingLicenseNumber = customer.DrivingLicenseNumber;
             // ... map other fields as needed or just save
             
             await _context.SaveChangesAsync();
             return existingCustomer;
        }

        // New Customer Logic
        customer.AddressLine2 ??= "";
        customer.City ??= "";
        customer.Pincode ??= "";
        
        // Fix for NOT NULL constraint on PhoneNumber
        if (string.IsNullOrEmpty(customer.PhoneNumber))
        {
            customer.PhoneNumber = customer.MobileNumber;
        }

        // Leave MembershipId null to avoid UNIQUE constraint violation on empty strings
        // customer.MembershipId ??= ""; 

        customer.IdpNumber ??= "";
        customer.PassportNumber ??= "";
        customer.PassportIssuedBy ??= "";
        
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task<CustomerMaster> GetCustomerByIdAsync(int id)
    {
        return await _context.Customers.FindAsync(id);
    }

    public async Task<CustomerMaster> UpdateCustomerAsync(CustomerMaster customer)
    {
        var existing = await _context.Customers.FindAsync(customer.CustId);
        if (existing != null)
        {
             // Update properties logic. 
             // Simple mapping or manual update
             _context.Entry(existing).CurrentValues.SetValues(customer);
             await _context.SaveChangesAsync();
             return existing;
        }
        return null; // Or throw
    }
}
