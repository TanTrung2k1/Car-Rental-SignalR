using BusinessObjects.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NguyenTanTrungRazorPages.GenerateId;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;

namespace NguyenTanTrungRazorPages.Hubs
{
    public class CustomerHub : Hub
    {
        private readonly CarRentalSystemDBContext _dbContext;
        public CustomerHub()
        {
            _dbContext = new CarRentalSystemDBContext();
        }

        public async Task GetAllCustomer()
        {
            // Get all the Car data from the database
            List<Customer> customers = await _dbContext.Customers.ToListAsync();

            // Send the Car data to the clients
            await Clients.All.SendAsync("ReceiveAllCustomers", customers);
        }

        public async Task CreateNewCustomer(string customerName, string mobile, string customerEmail, string customerPassword, string identityCard, string licenceNumber, string licenceDate)
        {
            GenerateNewId idGenerator = new GenerateNewId();
            Customer cus = new Customer();
            cus.CustomerId = idGenerator.GenerateCustomerId();
            cus.CustomerName = customerName;
            cus.CustomerEmail = customerEmail;
            cus.CustomerPassword = customerPassword;
            cus.IdentityCard = identityCard;
            cus.LicenceNumber = licenceNumber;
            cus.LicenceDate = DateTime.Parse(licenceDate);
            _dbContext.Customers.Add(cus);
            await _dbContext.SaveChangesAsync();
            await Clients.All.SendAsync("ReceiveNewCustomer", cus);
        }

        public async Task CreateNewCustomer01(string customerName, string mobile, string customerEmail, string customerPassword, string identityCard, string licenceNumber, string licenceDate)
        {
            if (_dbContext.Customers.Any(c => c.CustomerEmail == customerEmail))
            {
                throw new Exception("Customer email already exists.");
            }
            GenerateNewId idGenerator = new GenerateNewId();
            
            Customer cus = new Customer();
            cus.CustomerId = idGenerator.GenerateCustomerId();
            cus.CustomerName = customerName;
            cus.Mobile = mobile;
            cus.CustomerEmail = customerEmail;
            cus.CustomerPassword = customerPassword;
            cus.IdentityCard = identityCard;
            cus.LicenceNumber = licenceNumber;
            cus.LicenceDate = DateTime.Parse(licenceDate);
            _dbContext.Customers.Add(cus);
            await _dbContext.SaveChangesAsync();
            await Clients.All.SendAsync("ReceiveNewCustomer", cus);
        }
    }
}
