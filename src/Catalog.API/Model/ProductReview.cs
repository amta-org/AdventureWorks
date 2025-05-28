using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eShop.Catalog.API.Model;

public class ProductReview
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required]
    public int CatalogItemId { get; set; }

    [Range(1, 5)]
    [Required]
    public int Rating { get; set; }

    [MaxLength(1000)]
    public string ReviewText { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public CatalogItem CatalogItem { get; set; }
}