using System.Collections.Generic;

namespace Spv.Usuarios.Domain.Entities.V2
{
    public class InputsV2
    {
        public int InputId { get; set; }
        public string InputName { get; set; }
        public string Description { get; set; }
        public List<ReglaValidacionV2> ValidationRules { get; set; }
    }
}