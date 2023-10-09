namespace BankTrans.Data
{
    public class CityBankTransaction
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string? CurrencyName { get; set; }
        public string? ErrorMessage { get; set; }
        public string? FullString { get; set; }
        public string? InvoiceId { get; set; }
        public bool? IsError { get; set; }
        public string? MaskedCaditCardNo { get; set; }
        public string? MerchantId { get; set; }
        public string? TerminalId { get; set; }
        public string? TraceNo { get; set; }
        public string? TransactionStatus { get; set; }
        public DateTime? TransectionDateTime { get; set; }
        public bool? IsSetteledInvoice { get; set; }
        public string? POSInvoiceNo { get; set; }
        public string? RRNNumber { get; set; }
    }
}
