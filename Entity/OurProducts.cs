using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KhadeFarm_Web_API.Entity
{
    public class OurProducts
    {
        [Column("PRODUCT_ID")]
        [Key]
        public int ProductID { get; set; }
        [Column("FEATURE_ID")]
        public int FeatureID { get; set; }
        [Column("IS_NEW_ITEM")]
        public bool IsNewItem { get; set; }
        [Column("IMAGE_URL")]
        public string? ImageUrl { get; set; }
        [Column("PRODUCT_NAME")]
        public string? ProductName { get; set; }
        [Column("PRODUCT_ORIGINAL_PRICE")]
        public decimal ProductOriginalPrice { get; set; }
        [Column("PRODUCT_DISCOUNT_PRICE")]
        public decimal ProductDiscountPrice { get; set; }
        [Column("PRODUCT_VIEW_URL")]
        public string? ProductViewUrl { get; set; }
        [Column("PRODUCT_DESCRIPTION")]
        public string? ProductDescription { get; set; }

    }
}
