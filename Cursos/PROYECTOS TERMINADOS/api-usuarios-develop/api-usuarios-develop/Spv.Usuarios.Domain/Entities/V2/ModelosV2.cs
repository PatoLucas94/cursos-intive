using System.Collections.Generic;

namespace Spv.Usuarios.Domain.Entities.V2
{
    public class ModelosV2
    {
        public int ModelId { get; set; }
        public string ModelName { get; set; }
        public string Description { get; set; }
        public List<ReglaValidacionV2> ValidationRules { get; set; }
    }
}