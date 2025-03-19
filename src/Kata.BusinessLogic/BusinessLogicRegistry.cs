﻿using Kata.BusinessLogic.Interfaces;
using Kata.BusinessLogic.Services;
using Kata.DataAccess;
using Kata.DataAccess.Interfaces;
using Kata.DataAccess.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Kata.BusinessLogic
{
    public static class BusinessLogicRegistry
    {
        public static IServiceCollection RegisterBusinessLogicRepositories(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<ISqlDataAccess>(provider => new SqlDataAccess(connectionString));

            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IVideoService, VideoService>();
            services.AddScoped<IMembershipProductService, MembershipProductService>();
            services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
            services.AddScoped<IOrderItemService, OrderItemService>();
            services.AddScoped<IMembershipService, MembershipService>();
            services.AddScoped<IShippingSlipService, ShippingSlipService>();

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IVideoRepository, VideoRepository>();
            services.AddScoped<IMembershipProductRepository, MembershipProductRepository>();
            services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            services.AddScoped<IMembershipRepository, MembershipRepository>();
            services.AddScoped<IShippingSlipRepository, ShippingSlipRepository>();

            return services;
        }
    }
}
