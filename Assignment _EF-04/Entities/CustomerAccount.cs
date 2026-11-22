
namespace NationalBankManagementSystem.Entities
{
    public class CustomerAccount
    {
        public int CustomerId { get; set; }

        public string AccountNumber { get; set; } = string.Empty;

        public string OwnershipType { get; set; } = string.Empty;

        public DateTime OwnershipStartDate { get; set; }

        public Customer? Customer { get; set; }

        public Account? Account { get; set; }
    }
}
