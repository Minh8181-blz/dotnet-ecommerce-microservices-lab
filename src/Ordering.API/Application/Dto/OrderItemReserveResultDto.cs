namespace API.Ordering.Application.Dto
{
    public class OrderItemReserveResultDto
    {
        public bool Succeeded { get; set; }
        public int ProductId { get; set; }
        public string Code { get; set; }
    }
}
