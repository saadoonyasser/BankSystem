using BankSystem.Contexts;
using BankSystem.Models;
using BankSystem.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace BankSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var context = new BankDbContext();

            // Apply pending migrations before using the database.
            context.Database.Migrate();

            // Seed Branches and Managers only when the database is empty.
            if (!context.Branches.Any())
            {
                var branch1 = new Branch
                {
                    Code = "CAI-01",
                    Name = "Cairo Main Branch",
                    Address = "Downtown - Cairo",
                    PhoneNumber = "0223456789"
                };

                var branch2 = new Branch
                {
                    Code = "ALX-01",
                    Name = "Alexandria Branch",
                    Address = "Smouha - Alexandria",
                    PhoneNumber = "0334567890"
                };

                context.Branches.AddRange(branch1, branch2);

                context.Managers.AddRange(
                    new Manager
                    {
                        FullName = "Ahmed Hassan",
                        Email = "ahmed@nationalbank.com",
                        PhoneNumber = "01011112222",
                        HireDate = new DateTime(2020, 1, 10),
                        BranchCode = "CAI-01"
                    },
                    new Manager
                    {
                        FullName = "Mona Ali",
                        Email = "mona@nationalbank.com",
                        PhoneNumber = "01033334444",
                        HireDate = new DateTime(2021, 5, 15),
                        BranchCode = "ALX-01"
                    });

                context.SaveChanges();
                Console.WriteLine("Branches and managers seeded successfully.");
            }

            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine("======================================");
                Console.WriteLine("       National Bank - Management");
                Console.WriteLine("======================================");
                Console.WriteLine("  1) Add a new Customer");
                Console.WriteLine("  2) Open a new Account for a Customer");
                Console.WriteLine("  3) Update Account Status (Active / Closed)");
                Console.WriteLine("  4) Remove an Account from a Customer");
                Console.WriteLine("  5) List all Customers (with accounts)");
                Console.WriteLine("  0) Exit");
                Console.WriteLine("--------------------------------------");
                Console.Write("Enter choice: ");

                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                    {
                        Console.WriteLine("\n--- Add New Customer ---");

                        Console.Write("Full Name: ");
                        string fullName = Console.ReadLine() ?? "";

                        Console.Write("National ID: ");
                        string nationalId = Console.ReadLine() ?? "";

                        Console.Write("Date of Birth (yyyy-MM-dd): ");
                        DateTime dateOfBirth;
                        while (!DateTime.TryParse(Console.ReadLine(), out dateOfBirth) ||
                               dateOfBirth >= DateTime.Today)
                        {
                            Console.Write("Invalid date. Enter a valid date (yyyy-MM-dd): ");
                        }

                        Console.Write("Email: ");
                        string email = Console.ReadLine() ?? "";

                        Console.Write("Phone: ");
                        string phone = Console.ReadLine() ?? "";

                        Console.Write("Address: ");
                        string address = Console.ReadLine() ?? "";

                        Console.WriteLine("Customer Type:");
                        Console.WriteLine("  1) Individual");
                        Console.WriteLine("  2) Business");
                        Console.Write("Choice: ");
                        string customerTypeChoice = Console.ReadLine() ?? "";

                        while (customerTypeChoice != "1" && customerTypeChoice != "2")
                        {
                            Console.Write("Invalid choice. Enter 1 or 2: ");
                            customerTypeChoice = Console.ReadLine() ?? "";
                        }

                        if (string.IsNullOrWhiteSpace(fullName) ||
                            string.IsNullOrWhiteSpace(nationalId) ||
                            string.IsNullOrWhiteSpace(email) || !email.Contains('@') ||
                            string.IsNullOrWhiteSpace(phone) ||
                            string.IsNullOrWhiteSpace(address))
                        {
                            Console.WriteLine("Customer was not created. Please enter valid required data.");
                        }
                        else if (context.Customers.Any(c => c.NationalId == nationalId))
                        {
                            Console.WriteLine("A customer with this National ID already exists.");
                        }
                        else
                        {
                            var customer = new Customer
                            {
                                FullName = fullName,
                                NationalId = nationalId,
                                DateOfBirth = dateOfBirth,
                                Email = email,
                                PhoneNumber = phone,
                                Address = address,
                                CustomerType = customerTypeChoice == "1"
                                    ? CustomerType.Individual
                                    : CustomerType.Business
                            };

                            context.Customers.Add(customer);
                            context.SaveChanges();

                            Console.WriteLine($"Customer created successfully. CustomerId = {customer.Id}");
                        }

                        break;
                    }

                    case "2":
                    {
                        Console.WriteLine("\n--- Open New Account ---");

                        Console.Write("Account Number: ");
                        string accountNumber = Console.ReadLine() ?? "";

                        Console.WriteLine("Account Type:");
                        Console.WriteLine("  1) Savings");
                        Console.WriteLine("  2) Current");
                        Console.WriteLine("  3) Business");
                        Console.Write("Choice: ");
                        string accountTypeChoice = Console.ReadLine() ?? "";

                        while (accountTypeChoice != "1" &&
                               accountTypeChoice != "2" &&
                               accountTypeChoice != "3")
                        {
                            Console.Write("Invalid choice. Enter 1, 2 or 3: ");
                            accountTypeChoice = Console.ReadLine() ?? "";
                        }

                        Console.Write("Branch Code: ");
                        string branchCode = Console.ReadLine() ?? "";

                        Console.Write("Customer Id: ");
                        int customerId;
                        while (!int.TryParse(Console.ReadLine(), out customerId) || customerId <= 0)
                        {
                            Console.Write("Invalid Customer Id. Enter a positive number: ");
                        }

                        Console.WriteLine("Ownership Role:");
                        Console.WriteLine("  1) Primary");
                        Console.WriteLine("  2) CoHolder");
                        Console.Write("Choice: ");
                        string ownershipChoice = Console.ReadLine() ?? "";

                        while (ownershipChoice != "1" && ownershipChoice != "2")
                        {
                            Console.Write("Invalid choice. Enter 1 or 2: ");
                            ownershipChoice = Console.ReadLine() ?? "";
                        }

                        var branch = context.Branches
                            .FirstOrDefault(b => b.Code == branchCode);

                        var customer = context.Customers
                            .FirstOrDefault(c => c.Id == customerId);

                        if (string.IsNullOrWhiteSpace(accountNumber))
                        {
                            Console.WriteLine("Account number cannot be empty.");
                        }
                        else if (branch == null)
                        {
                            Console.WriteLine($"Branch '{branchCode}' was not found.");
                        }
                        else if (customer == null)
                        {
                            Console.WriteLine($"Customer {customerId} was not found.");
                        }
                        else if (context.Accounts.Any(a => a.AccountNumber == accountNumber))
                        {
                            Console.WriteLine("This account number already exists.");
                        }
                        else
                        {
                            var account = new Account
                            {
                                AccountNumber = accountNumber,
                                AccountType = (AccountType)int.Parse(accountTypeChoice),
                                CurrentBalance = 0,
                                OpeningDate = DateTime.Now,
                                BranchCode = branchCode
                            };

                            var customerAccount = new CustomerAccount
                            {
                                CustomerId = customerId,
                                AccountNumber = accountNumber,
                                OwnershipType = (OwnershipType)int.Parse(ownershipChoice),
                                OwnershipStartDate = DateTime.Now,
                                AccountStatus = AccountStatus.Active
                            };

                            context.Accounts.Add(account);
                            context.CustomerAccounts.Add(customerAccount);
                            context.SaveChanges();

                            Console.WriteLine($"Validating branch '{branchCode}' and customer {customerId}...");
                            Console.WriteLine($"Account '{accountNumber}' created and linked to customer {customerId} as {customerAccount.OwnershipType} owner.");
                        }

                        break;
                    }

                    case "3":
                    {
                        Console.WriteLine("\n--- Update Account Status ---");

                        Console.Write("Account Number: ");
                        string accountNumber = Console.ReadLine() ?? "";

                        Console.Write("Customer Id: ");
                        int customerId;
                        while (!int.TryParse(Console.ReadLine(), out customerId) || customerId <= 0)
                        {
                            Console.Write("Invalid Customer Id. Enter a positive number: ");
                        }

                        var customerAccount = context.CustomerAccounts
                            .FirstOrDefault(ca =>
                                ca.AccountNumber == accountNumber &&
                                ca.CustomerId == customerId);

                        if (customerAccount == null)
                        {
                            Console.WriteLine("This customer is not linked to the account.");
                        }
                        else
                        {
                            Console.WriteLine("New Status:");
                            Console.WriteLine("  1) Active");
                            Console.WriteLine("  2) Closed");
                            Console.Write("Choice: ");
                            string statusChoice = Console.ReadLine() ?? "";

                            while (statusChoice != "1" && statusChoice != "2")
                            {
                                Console.Write("Invalid choice. Enter 1 or 2: ");
                                statusChoice = Console.ReadLine() ?? "";
                            }

                            customerAccount.AccountStatus = (AccountStatus)int.Parse(statusChoice);
                            context.SaveChanges();

                            Console.WriteLine($"Status updated to {customerAccount.AccountStatus}.");
                        }

                        break;
                    }

                    case "4":
                    {
                        Console.WriteLine("\n--- Remove Account From Customer ---");

                        Console.Write("Account Number: ");
                        string accountNumber = Console.ReadLine() ?? "";

                        Console.Write("Customer Id: ");
                        int customerId;
                        while (!int.TryParse(Console.ReadLine(), out customerId) || customerId <= 0)
                        {
                            Console.Write("Invalid Customer Id. Enter a positive number: ");
                        }

                        var customerAccount = context.CustomerAccounts
                            .FirstOrDefault(ca =>
                                ca.AccountNumber == accountNumber &&
                                ca.CustomerId == customerId);

                        if (customerAccount == null)
                        {
                            Console.WriteLine("This customer is not linked to the account.");
                        }
                        else
                        {
                            context.CustomerAccounts.Remove(customerAccount);
                            context.SaveChanges();

                            Console.WriteLine("Ownership link deleted.");

                            bool hasOtherOwners = context.CustomerAccounts
                                .Any(ca => ca.AccountNumber == accountNumber);

                            if (!hasOtherOwners)
                            {
                                var account = context.Accounts
                                    .FirstOrDefault(a => a.AccountNumber == accountNumber);

                                if (account != null)
                                {
                                    context.Accounts.Remove(account);
                                    context.SaveChanges();

                                    Console.WriteLine($"That was the last owner - account '{accountNumber}' was also removed.");
                                }
                            }
                        }

                        break;
                    }

                    case "5":
                    {
                        Console.WriteLine("\n--- All Customers ---\n");

                        var customers = context.Customers
                            .Include(c => c.CustomerAccounts)
                            .ThenInclude(ca => ca.Account)
                            .ThenInclude(a => a.Branch)
                            .OrderBy(c => c.Id)
                            .ToList();

                        if (!customers.Any())
                        {
                            Console.WriteLine("No customers found.");
                        }
                        else
                        {
                            foreach (var customer in customers)
                            {
                                Console.WriteLine($"#{customer.Id} {customer.FullName} ({customer.CustomerType})");

                                if (!customer.CustomerAccounts.Any())
                                {
                                    Console.WriteLine("   (no accounts)");
                                }
                                else
                                {
                                    foreach (var link in customer.CustomerAccounts)
                                    {
                                        Console.WriteLine(
                                            $"   {link.AccountNumber}   {link.Account.AccountType,-10} " +
                                            $"Balance: {link.Account.CurrentBalance,10:N2}   " +
                                            $"{link.OwnershipType,-8} {link.AccountStatus,-7}   " +
                                            $"@ {link.Account.Branch.Name}");
                                    }
                                }

                                Console.WriteLine();
                            }
                        }

                        break;
                    }

                    case "0":
                    {
                        isRunning = false;
                        Console.WriteLine("Thank you for using National Bank Management System.");
                        break;
                    }

                    default:
                    {
                        Console.WriteLine("Invalid choice. Please choose a number from the menu.");
                        break;
                    }
                }

                if (isRunning)
                {
                    Console.WriteLine("\nPress any key to return to the menu...");
                    Console.ReadKey();
                }
            }
        }
    }
}
