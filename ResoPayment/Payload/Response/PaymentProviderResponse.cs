namespace ResoPayment.Payload.Response
{
    public class PaymentProviderResponse
    {

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string? PicUrl { get; set; }

        public PaymentProviderResponse(Guid id, string name, string type, string? picUrl)
        {
            Id = id;
            Name = name;
            Type = type;
            PicUrl = picUrl;
        }
    }
}
