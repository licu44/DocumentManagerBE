namespace DocumentManager.Data
{
    public class DocumentDto
    {
        public int DocId { get; set; }
        public string DocName { get; set; }
        public DateTime? CreationDate { get; set; }
        public string Status { get; set; }
        public string Data { get; set; }
    }

}
