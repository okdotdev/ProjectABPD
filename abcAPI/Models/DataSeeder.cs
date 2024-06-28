using abcAPI.Models.TableModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace abcAPI.Models
{
    public static class DataSeeder
    {
        public static void SeedData(IServiceProvider serviceProvider)
        {
            using var context = new AppDbContext(serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>());

            if (context.Clients.Any())
                return;

            // Add ClientIndividual records
            var individualClients = new List<ClientIndividual>
            {
                new ClientIndividual
                {
                    FirstName = "John", LastName = "Doe", Pesel = "12345678901", Address = "123 Main St",
                    Email = "john.doe@example.com", PhoneNumber = "123-456-7890", IsDeleted = false
                },
                new ClientIndividual
                {
                    FirstName = "Jane", LastName = "Smith", Pesel = "23456789012", Address = "456 Elm St",
                    Email = "jane.smith@example.com", PhoneNumber = "234-567-8901", IsDeleted = false
                },
                new ClientIndividual
                {
                    FirstName = "Alice", LastName = "Johnson", Pesel = "34567890123", Address = "789 Oak St",
                    Email = "alice.johnson@example.com", PhoneNumber = "345-678-9012", IsDeleted = false
                }
            };

            // Add ClientCompany records
            var companyClients = new List<ClientCompany>
            {
                new ClientCompany
                {
                    CompanyName = "TechCorp", Krs = "1234567890", Address = "123 Corporate Blvd",
                    Email = "info@techcorp.com", PhoneNumber = "123-456-7890", IsDeleted = false
                },
                new ClientCompany
                {
                    CompanyName = "BizInc", Krs = "2345678901", Address = "456 Business Ave", Email = "contact@bizinc.com",
                    PhoneNumber = "234-567-8901", IsDeleted = false
                }
            };

            // Add Software records
            var softwares = new List<Software>
            {
                new Software
                {
                    Name = "Software A", Description = "Description for Software A", CurrentVersion = "1.0.0",
                    Category = "Category A"
                },
                new Software
                {
                    Name = "Software B", Description = "Description for Software B", CurrentVersion = "2.0.0",
                    Category = "Category B"
                },
                new Software
                {
                    Name = "Software C", Description = "Description for Software C", CurrentVersion = "3.0.0",
                    Category = "Category C"
                }
            };

            context.Clients.AddRange(individualClients);
            context.Clients.AddRange(companyClients);
            context.Softwares.AddRange(softwares);
            context.SaveChanges();

            // Retrieve software IDs
            var softwareIds = context.Softwares.Select(s => s.Id).ToList();

            // Add Contract records
            var contracts = new List<Contract>
            {
                new Contract
                {
                    SoftwareId = softwareIds[0], StartDate = DateTime.Now.AddMonths(-6),
                    EndDate = DateTime.Now.AddMonths(6), Price = 2300, AmountPaid = 500, Version = "1.0.0",
                    AdditionalSupportYears = 2, IsSigned = true, IsPaid = true
                },
                new Contract
                {
                    SoftwareId = softwareIds[1], StartDate = DateTime.Now.AddMonths(-12),
                    EndDate = DateTime.Now.AddMonths(12), Price = 3600, AmountPaid = 2000, Version = "2.0.0",
                    AdditionalSupportYears = 3, IsSigned = true, IsPaid = true
                },
                new Contract
                {
                    SoftwareId = softwareIds[2], StartDate = DateTime.Now.AddMonths(-3),
                    EndDate = DateTime.Now.AddMonths(9), Price = 1500, AmountPaid = 750, Version = "3.0.0",
                    AdditionalSupportYears = 1, IsSigned = false, IsPaid = false
                }
            };

            context.Contracts.AddRange(contracts);
            context.SaveChanges();

            // Retrieve contract IDs
            var contractIds = context.Contracts.Select(c => c.Id).ToList();

            // Add ClientContract records
            var clientContracts = new List<ClientContract>
            {
                new ClientContract
                {
                    ClientId = individualClients[0].IdClient, ContractId = contractIds[0]
                },
                new ClientContract
                {
                    ClientId = individualClients[1].IdClient, ContractId = contractIds[1]
                },
                new ClientContract
                {
                    ClientId = individualClients[2].IdClient, ContractId = contractIds[2]
                },
                new ClientContract
                {
                    ClientId = companyClients[0].IdClient, ContractId = contractIds[0]
                },
                new ClientContract
                {
                    ClientId = companyClients[1].IdClient, ContractId = contractIds[1]
                }
            };

            context.ClientContracts.AddRange(clientContracts);
            context.SaveChanges();

            // Add Subscription records
            var subscriptions = new List<Subscription>
            {
                new Subscription
                {
                    OfferName = "Basic Plan", ContractId = contractIds[0], PriceOfRenewal = 100, IsMonthly = true,
                    IsActive = true
                },
                new Subscription
                {
                    OfferName = "Pro Plan", ContractId = contractIds[1], PriceOfRenewal = 200, IsMonthly = false,
                    IsActive = true
                },
                new Subscription
                {
                    OfferName = "Enterprise Plan", ContractId = contractIds[2], PriceOfRenewal = 300, IsMonthly = true,
                    IsActive = false
                }
            };

            context.Subscriptions.AddRange(subscriptions);
            context.SaveChanges();
        }
    }
}
