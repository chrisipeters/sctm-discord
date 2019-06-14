using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCTM.Q.Models
{
    public class Manufacturer : BaseDbObjectWithId
    {
        [Required(ErrorMessage = "Manufacturer > Name is required")]
        public string Name { get; set; }
        public string KnownAs { get; set; }
        public string Description { get; set; }
        public string LogoPath { get; set; }

        [Required(ErrorMessage = "Manufacturer > MatchingStrings is required")]
        public List<ManufacturerMatchingString> MatchingStrings { get; set; }
    }

    public class ManufacturerMatchingString : BaseDbObjectWithId
    {
        [Required(ErrorMessage = "ManufacturerMatchingString > Value is required"), StringLength(50, MinimumLength = 2, ErrorMessage = "ManufacturerMatchingString>Value must be 2 - 50 chars")]
        public string Value { get; set; }

        [Required(ErrorMessage = "ManufacturerMatchingString > Manufacturer is required")]
        public Manufacturer Manufacturer { get; set; }
        public int ManufacturerId { get; set; }
    }
}
