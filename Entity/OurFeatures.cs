using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KhadeFarm_Web_API.Entity
{
    [Table("KF_FEATURES")]   // actual table name
    public class OurFeatures
    {
        public OurFeatures() { }
        [Column("FEATURE_ID")]
        [Key] // ✅ Explicitly mark as primary key
        public int FeatureID { get; set; }
        [Column("IMG_URL")]
        public string? ImgUrl { get; set; }
        [Column("Title")]
        public string? Title { get; set; }
        [Column("Description")]
        public string? Description { get; set; }
        [Column("REDIRECT_URL")]
        public string? RedirectUrl { get; set; }
    }
}
