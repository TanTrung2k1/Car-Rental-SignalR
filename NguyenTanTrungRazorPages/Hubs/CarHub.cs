using BusinessObjects.Models;
using BusinessObjects.Validation;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NguyenTanTrungRazorPages.GenerateId;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace NguyenTanTrungRazorPages.Hubs
{
    public class CarHub : Hub
    {
        private readonly CarRentalSystemDBContext _dbContext;
        public CarHub()
        {
            _dbContext = new CarRentalSystemDBContext();
        }

        public async Task GetAllCars()
        {
            // Get all the Car data from the database
            List<Car> cars = await _dbContext.Cars.ToListAsync();

            // Send the Car data to the clients
            await Clients.All.SendAsync("ReceiveAllCars", cars);
        }

        
        

        public async Task CreateNewCar(string carName, string carModelYear, string color, string capacity, string description, string importDate, string rentPrice, string producerId)
        {
            GenerateNewId idGenerator = new GenerateNewId();
            Car car = new Car();
            car.CarId = idGenerator.GenerateCarId();
            car.CarName = carName;
            car.CarModelYear = int.Parse(carModelYear);
            car.Color = color;
            car.Capacity = int.Parse(capacity);
            car.Description = description;
            car.ImportDate = DateTime.Parse(importDate);
            car.RentPrice = Decimal.Parse(rentPrice);
            car.Status = 1;
            car.ProducerId = producerId;

            _dbContext.Cars.Add(car);
            await _dbContext.SaveChangesAsync();

            // Send the new car data to the clients
            await Clients.All.SendAsync("ReceiveNewCar", car);

        }

        public async Task GetAllCars01()
        {
            // Get all the Car data from the database
            List<Car> cars = await _dbContext.Cars.ToListAsync();

            // Send the Car data to the clients
            await Clients.All.SendAsync("ReceiveAllCars", cars);
        }

        public async Task DeleteCar(string carId)
        {
            var car = await _dbContext.Cars.FindAsync(carId);
            if (car != null)
            {
                _dbContext.Cars.Remove(car);
                await _dbContext.SaveChangesAsync();
                await Clients.All.SendAsync("ReceiveDeletedCar", carId);
            }
        }


    }

    
}
