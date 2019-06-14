using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCTM.Q.Models
{
    public class Ship : BaseDbObjectWithId
    {
        [Required(ErrorMessage = "Ship > Model is required")]
        public string Model { get; set; }

        public int ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; }

        public string Focus { get; set; }

        public string Description { get; set; }
        public GameState GameState { get; set; }

        public string ThumbnailPath { get; set; }

        public string RSILink { get; set; }

        public string Size { get; set; }

        // Specifications
        public double? Length { get; set; }
        public double? Beam { get; set; }
        public double? Height { get; set; }
        public double? Mass { get; set; }
        public int? CargoCapacity { get; set; }
        public int? SCMSpeed { get; set; }
        public int? AfterburnerSpeed { get; set; }
        public int? CrewMin { get; set; }
        public int? CrewMax { get; set; }
        public double? PitchMax { get; set; }
        public double? YawMax { get; set; }
        public double? RollMax { get; set; }
        public double? XAxisAcceleration { get; set; }
        public double? YAxisAcceleration { get; set; }
        public double? ZAxisAcceleration { get; set; }

        [Required(ErrorMessage = "Ship > MatchingStrings is required")]
        public List<ShipMatchingString> MatchingStrings { get; set; }

        /*
        // Avionics
        public IEnumerable<ComponentSlot> Radar { get; set; }
        public IEnumerable<ComponentSlot> Computers { get; set; }

        //Propulsion
        public IEnumerable<ComponentSlot> FuelIntakes { get; set; }
        public IEnumerable<ComponentSlot> FuelTanks { get; set; }
        public IEnumerable<ComponentSlot> QuantumDrives { get; set; }
        public IEnumerable<ComponentSlot> JumpModules { get; set; }
        public IEnumerable<ComponentSlot> QuantumFuelTanks { get; set; }

        //Thrusters
        public IEnumerable<ComponentSlot> MainThrusters { get; set; }
        public IEnumerable<ComponentSlot> ManeuveringThrusters { get; set; }

        // Systems
        public IEnumerable<ComponentSlot> PowerPlants { get; set; }
        public IEnumerable<ComponentSlot> Coolers { get; set; }
        public IEnumerable<ComponentSlot> ShieldGenerators { get; set; }

        // Weaponry
        public IEnumerable<ComponentSlot> Weapons { get; set; }
        public IEnumerable<ComponentSlot> Turrets { get; set; }
        public IEnumerable<ComponentSlot> Missiles { get; set; }
        public IEnumerable<ComponentSlot> UtilityItems { get; set; }
        */
    }

    public class ShipMatchingString : BaseDbObjectWithId
    {
        [Required(ErrorMessage = "ShipMatchingString > Value is required"), StringLength(50, MinimumLength = 2, ErrorMessage = "ShipMatchingString > Value must be 2 - 50 chars")]
        public string Value { get; set; }

        [Required(ErrorMessage = "ShipMatchingString > Ship is required")]
        public Ship Ship { get; set; }
        public int ModelId { get; set; }
    }
}
