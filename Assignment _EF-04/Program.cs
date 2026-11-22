
using Microsoft.EntityFrameworkCore;
using NationalBankManagementSystem.Data;
using NationalBankManagementSystem.Entities;

using var db = new BankDbContext();

if (!db.Branches.Any())
{
    var branch1 = new Branch
    {
        Code = "CAI-01",
        Name = "Cairo Main Branch",
        Address = "Cairo, Egypt",
        PhoneNumber = "0223456789",
        Manager = new Manager
        {
            FullName = "Ahmed Ali",
            Email = "ahmed@bank.com",
            PhoneNumber = "01012345678",
            HireDate = new DateTime(2020, 1, 15),
            BranchCode = "CAI-01"
        }
    };

    var branch2 = new Branch
    {
        Code = "ALX-01",
        Name = "Alexandria Branch",
        Address = "Alexandria, Egypt",
        PhoneNumber = "0334567890",
        Manager = new Manager
        {
            FullName = "Mohamed Hassan",
            Email = "mohamed@bank.com",
            PhoneNumber = "01112345678",
            HireDate = new DateTime(2021, 5, 10),
            BranchCode = "ALX-01"
        }
    };

    db.Branches.AddRange(branch1, branch2);

    db.SaveChanges();

    Console.WriteLine("Seed data added successfully!");
}
else
{
    Console.WriteLine("Seed data already exists.");
}


bool exit = false;

while (!exit)
{
    Console.Clear();

    Console.WriteLine("====================================");
    Console.WriteLine("       National Bank Management");
    Console.WriteLine("====================================");

    Console.WriteLine("1) Add a new Customer");
    Console.WriteLine("2) Open a new Account for a Customer");
    Console.WriteLine("3) Update Account Status");
    Console.WriteLine("4) Remove an Account from a Customer");
    Console.WriteLine("5) List all Customers (with accounts)");
    Console.WriteLine("0) Exit");

    Console.WriteLine("------------------------------------");
    Console.Write("Enter choice: ");

    string? input = Console.ReadLine();

    if (!int.TryParse(input, out int choice))
    {
        Console.WriteLine("Invalid input. Please enter a number.");
        Console.ReadKey();
        continue;
    }

    switch (choice)
    {
        case 0:
            exit = true;
            Console.WriteLine("Goodbye!");
            break;



        case 1:
            {
                Console.Clear();
                Console.WriteLine("--- Add New Customer ---");

                Console.Write("Full Name: ");
                string fullName = (Console.ReadLine() ?? "").Trim();

                Console.Write("National ID: ");
                string nationalId = (Console.ReadLine() ?? "").Trim();

                Console.Write("Date of Birth (yyyy-MM-dd): ");
                string dobInput = Console.ReadLine() ?? "";

                Console.Write("Email: ");
                string email = (Console.ReadLine() ?? "").Trim();

                Console.Write("Phone: ");
                string phone = (Console.ReadLine() ?? "").Trim();

                Console.Write("Address: ");
                string address = (Console.ReadLine() ?? "").Trim();

                Console.WriteLine("Customer Type:");
                Console.WriteLine("1) Individual");
                Console.WriteLine("2) Business");
                Console.Write("Choose: ");

                string typeInput = Console.ReadLine() ?? "";

                bool validName = !string.IsNullOrWhiteSpace(fullName) &&
                                 fullName.All(c => char.IsLetter(c) || char.IsWhiteSpace(c));

                bool validNationalId = nationalId.Length == 14 &&
                                       nationalId.All(char.IsDigit);

                bool validDob = DateTime.TryParse(dobInput, out DateTime dob) &&
                                dob < DateTime.Today;

                bool validEmail = System.Net.Mail.MailAddress.TryCreate(email, out var mailAddress) &&
                                  mailAddress.Address == email;

                bool validPhone = phone.Length >= 10 &&
                                  phone.Length <= 15 &&
                                  phone.All(char.IsDigit);

                bool validAddress = !string.IsNullOrWhiteSpace(address);

                bool validType = typeInput == "1" || typeInput == "2";

                if (!validName)
                {
                    Console.WriteLine("Invalid name.");
                }
                else if (!validNationalId)
                {
                    Console.WriteLine("National ID must contain exactly 14 digits.");
                }
                else if (!validDob)
                {
                    Console.WriteLine("Invalid date of birth.");
                }
                else if (!validEmail)
                {
                    Console.WriteLine("Invalid email address.");
                }
                else if (!validPhone)
                {
                    Console.WriteLine("Phone must contain 10 to 15 digits.");
                }
                else if (!validAddress)
                {
                    Console.WriteLine("Address cannot be empty.");
                }
                else if (!validType)
                {
                    Console.WriteLine("Invalid customer type.");
                }
                else
                {
                    bool customerExists = db.Customers
                        .Any(c => c.NationalId == nationalId);

                    if (customerExists)
                    {
                        Console.WriteLine("A customer with this National ID already exists.");
                    }
                    else
                    {
                        var customer = new Customer
                        {
                            FullName = fullName,
                            NationalId = nationalId,
                            DateOfBirth = dob,
                            Email = email,
                            Phone = phone,
                            Address = address,
                            CustomerType = typeInput == "1" ? "Individual" : "Business"
                        };

                        db.Customers.Add(customer);
                        db.SaveChanges();

                        Console.WriteLine("Customer added successfully.");
                    }
                }

                Console.WriteLine("Press any key to return to the menu...");
                Console.ReadKey();
                break;
            }


        case 2:
            {
                Console.Clear();
                Console.WriteLine("--- Open New Account ---");

                Console.Write("Account Number: ");
                string accountNumber = (Console.ReadLine() ?? "").Trim();

                Console.WriteLine("Account Type:");
                Console.WriteLine("1) Savings");
                Console.WriteLine("2) Current");
                Console.WriteLine("3) Business");
                Console.Write("Choice: ");
                string accountTypeInput = Console.ReadLine() ?? "";

                Console.Write("Branch Code: ");
                string branchCode = (Console.ReadLine() ?? "").Trim();

                Console.Write("Customer Id: ");
                string customerIdInput = Console.ReadLine() ?? "";

                Console.WriteLine("Ownership Role:");
                Console.WriteLine("1) Primary");
                Console.WriteLine("2) CoHolder");
                Console.Write("Choice: ");
                string ownershipInput = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(accountNumber) ||
                    string.IsNullOrWhiteSpace(branchCode) ||
                    !int.TryParse(customerIdInput, out int customerId) ||
                    !int.TryParse(accountTypeInput, out int accountTypeChoice) ||
                    accountTypeChoice < 1 || accountTypeChoice > 3 ||
                    !int.TryParse(ownershipInput, out int ownershipChoice) ||
                    (ownershipChoice != 1 && ownershipChoice != 2))
                {
                    Console.WriteLine("Invalid input.");
                }
                else if (db.Accounts.Any(a => a.AccountNumber.Trim() == accountNumber))
                {
                    Console.WriteLine("Account number already exists.");
                }
                else
                {
                    var branch = db.Branches
                        .FirstOrDefault(b => b.Code == branchCode);

                    var customer = db.Customers
                        .FirstOrDefault(c => c.Id == customerId);

                    if (branch == null)
                    {
                        Console.WriteLine("Branch not found.");
                    }
                    else if (customer == null)
                    {
                        Console.WriteLine("Customer not found.");
                    }
                    else
                    {
                        string[] accountTypes = { "Savings", "Current", "Business" };

                        var newAccount = new Account
                        {
                            AccountNumber = accountNumber,
                            AccountType = accountTypes[accountTypeChoice - 1],
                            CurrentBalance = 0,
                            OpeningDate = DateTime.Today,
                            AccountStatus = "Active",
                            BranchCode = branchCode
                        };

                        var newCustomerAccount = new CustomerAccount
                        {
                            CustomerId = customerId,
                            AccountNumber = accountNumber,
                            OwnershipType = ownershipChoice == 1
                                ? "Primary"
                                : "CoHolder",
                            OwnershipStartDate = DateTime.Today
                        };

                        db.Accounts.Add(newAccount);
                        db.CustomerAccounts.Add(newCustomerAccount);

                        db.SaveChanges();

                        Console.WriteLine(
                            $"Account '{accountNumber}' created and linked to customer {customerId} successfully.");
                    }
                }

                Console.WriteLine("Press any key to return to the menu...");
                Console.ReadKey();
                break;
            }



        case 3:
            {
                Console.Clear();
                Console.WriteLine("--- Toggle Account Status ---");

                Console.Write("Account Number: ");
                string accountNumber = (Console.ReadLine() ?? "").Trim();

                Console.Write("Customer Id: ");
                string customerIdInput = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(accountNumber) ||
                    !int.TryParse(customerIdInput, out int customerId))
                {
                    Console.WriteLine("Invalid input.");
                }
                else
                {
                    var customerAccount = db.CustomerAccounts
                        .FirstOrDefault(ca =>
                            ca.AccountNumber.Trim() == accountNumber &&
                            ca.CustomerId == customerId);

                    if (customerAccount == null)
                    {
                        Console.WriteLine("Account is not linked to this customer.");
                    }
                    else
                    {
                        var account = db.Accounts
                            .FirstOrDefault(a => a.AccountNumber.Trim() == accountNumber);

                        if (account == null)
                        {
                            Console.WriteLine("Account not found.");
                        }
                        else if (account.AccountStatus == "Active")
                        {
                            account.AccountStatus = "Closed";
                            db.SaveChanges();
                            Console.WriteLine("Account status changed from Active to Closed.");
                        }
                        else if (account.AccountStatus == "Closed")
                        {
                            account.AccountStatus = "Active";
                            db.SaveChanges();
                            Console.WriteLine("Account status changed from Closed to Active.");
                        }
                        else
                        {
                            Console.WriteLine("Unsupported account status.");
                        }
                    }
                }

                Console.WriteLine("Press any key to return to the menu...");
                Console.ReadKey();
                break;
            }


        case 4:
            {
                Console.Clear();
                Console.WriteLine("--- Remove Account from Customer ---");

                Console.Write("Customer ID: ");
                string customerIdInput = Console.ReadLine() ?? "";

                if (!int.TryParse(customerIdInput, out int customerId))
                {
                    Console.WriteLine("Invalid Customer ID.");
                }
                else
                {
                    Console.Write("Account Number: ");
                    string accountNumber = (Console.ReadLine() ?? "").Trim();

                    var customerAccount = db.CustomerAccounts
                        .FirstOrDefault(ca =>
                            ca.CustomerId == customerId &&
                            ca.AccountNumber.Trim() == accountNumber);

                    if (customerAccount == null)
                    {
                        Console.WriteLine("Account is not linked to this customer.");
                    }
                    else
                    {
                        db.CustomerAccounts.Remove(customerAccount);
                        db.SaveChanges();

                        Console.WriteLine("Account removed from customer successfully.");
                    }
                }

                Console.WriteLine("Press any key to return to the menu...");
                Console.ReadKey();
                break;
            }



        case 5:
            {
                var allCustomers = db.Customers
                    .Include(c => c.CustomerAccounts)
                    .ThenInclude(ca => ca.Account)
                    .ToList();

                if (allCustomers.Count == 0)
                {
                    Console.WriteLine("No customers found.");
                    Console.ReadKey();
                    break;
                }

                foreach (var listedCustomer in allCustomers)
                {
                    Console.WriteLine("-----------------------------------");
                    Console.WriteLine($"Customer ID: {listedCustomer.Id}");
                    Console.WriteLine($"Full Name: {listedCustomer.FullName}");
                    Console.WriteLine($"National ID: {listedCustomer.NationalId}");
                    Console.WriteLine($"Email: {listedCustomer.Email}");
                    Console.WriteLine($"Phone: {listedCustomer.Phone}");
                    Console.WriteLine($"Customer Type: {listedCustomer.CustomerType}");

                    Console.WriteLine("Accounts:");

                    if (listedCustomer.CustomerAccounts.Count == 0)
                    {
                        Console.WriteLine("No accounts linked to this customer.");
                    }
                    else
                    {
                        foreach (var linkedAccount in listedCustomer.CustomerAccounts)
                        {
                            if (linkedAccount.Account == null)
                                continue;

                            Console.WriteLine($"Account Number: {linkedAccount.Account.AccountNumber.Trim()}");
                            Console.WriteLine($"Account Type: {linkedAccount.Account.AccountType}");
                            Console.WriteLine($"Balance: {linkedAccount.Account.CurrentBalance:F2}");
                            Console.WriteLine($"Status: {linkedAccount.Account.AccountStatus}");
                            Console.WriteLine($"Ownership: {linkedAccount.OwnershipType}");
                        }
                    }
                }

                Console.WriteLine("-----------------------------------");
                Console.ReadKey();
                break;
            }



        default:
            Console.WriteLine("Invalid choice.");
            Console.ReadKey();
            break;
    }
}
