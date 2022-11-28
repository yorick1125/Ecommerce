using AutoMapper;
using ECommerce.Api.Orders.Db;
using ECommerce.Api.Orders.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Orders.Providers
{
    public class OrderProvider : IOrdersProvider
    {
        private readonly OrdersDbContext dbContext;
        private readonly ILogger<OrderProvider> logger;
        private readonly IMapper mapper;

        public OrderProvider(OrdersDbContext dbContext, ILogger<OrderProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;
            SeedData();
        }

        private void SeedData()
        {
            if (!dbContext.Orders.Any())
            {
                List<Db.OrderItem> items = new List<Db.OrderItem>();
                Db.OrderItem item1 = new Db.OrderItem();
                item1.Id = 11;
                item1.ProductId = 11;
                item1.Quantity = 11;
                item1.UnitPrice = 11;

                Db.OrderItem item2 = new Db.OrderItem();
                item1.Id = 12;
                item1.ProductId = 12;
                item1.Quantity = 12;
                item1.UnitPrice = 12;


                items.Add(item1);
                items.Add(item2);


                dbContext.Orders.Add(
                    new Db.Order() 
                    { 
                        Id = 1, CustomerId = 1, 
                        OrderDate = DateTime.Now, 
                        Total = 2, 
                        Items = items
                    });;

                dbContext.Orders.Add(
                    new Db.Order()
                    {
                        Id = 2,
                        CustomerId = 1,
                        OrderDate = DateTime.Now,
                        Total = 2,
                        Items = items
                    });

                dbContext.Orders.Add(
                    new Db.Order()
                    {
                        Id = 3,
                        CustomerId = 1,
                        OrderDate = DateTime.Now,
                        Total = 2,
                        Items = items
                    });

                dbContext.Orders.Add(
                    new Db.Order()
                    {
                        Id = 4,
                        CustomerId = 1,
                        OrderDate = DateTime.Now,
                        Total = 2,
                        Items = items
                    });

                dbContext.SaveChanges();
            }
        }

        async Task<(bool IsSuccess, IEnumerable<Models.Order> Orders, string ErrorMessage)> IOrdersProvider.GetOrdersAsync(int customerId)
        {
            try
            {
                logger?.LogInformation("Querying orders");
                var orders = await dbContext.Orders.Where(o => o.CustomerId == customerId).ToListAsync();
                if (orders != null && orders.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Order>, IEnumerable<Models.Order>>(orders);
                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
