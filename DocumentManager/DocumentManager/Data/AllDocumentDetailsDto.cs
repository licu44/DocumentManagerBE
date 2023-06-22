namespace DocumentManager.Data
{
    public class AllDocumentDetailsDto
    {
        public IdCardDetailsDto IdCardDetails { get; set; }
    }
    public class IdCardDetailsDto
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string CNP { get; set; }
        public string Series { get; set; }
        public string Number { get; set; }
        public string Address { get; set; }
    }
}
