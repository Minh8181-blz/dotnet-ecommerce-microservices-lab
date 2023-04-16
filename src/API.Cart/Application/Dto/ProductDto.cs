using System;
using System.ComponentModel.DataAnnotations;

namespace API.Carts.Application.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; private set; }
    }
}
