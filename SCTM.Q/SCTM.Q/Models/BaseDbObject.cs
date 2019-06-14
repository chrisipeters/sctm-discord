using System;
using System.ComponentModel.DataAnnotations;

namespace SCTM.Q.Models
{
    public abstract class BaseDbObject
    {
        public DateTime Created { get; set; }
        public DateTime LastChanged { get; set; }

        [Required(ErrorMessage = "BaseDbObject > Source is required")]
        public string Source { get; set; }
    }

    public abstract class BaseDbObjectWithId : BaseDbObject
    {
        [Key]
        public int Id { get; set; }
    }
}
