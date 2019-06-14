using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCTM.Q.Models
{
    public class GameState : BaseDbObjectWithId
    {
        [Required(ErrorMessage = "GameState > Name is required")]
        public string Name { get; set; }

        public bool InGame { get; set; } = false;
    }
}
