using Domain.Base.SeedWork;

namespace API.Catalog.Domain.Enums
{
    public class StockItemRecordStatus : Enumeration
    {
        public static StockItemRecordStatus Reserved = new StockItemRecordStatus(1, "reserved");
        public static StockItemRecordStatus Sold = new StockItemRecordStatus(2, "sold");
        public static StockItemRecordStatus Returned = new StockItemRecordStatus(3, "returned");

        public StockItemRecordStatus(int id, string name) : base(id, name)
        {
        }
    }
}
