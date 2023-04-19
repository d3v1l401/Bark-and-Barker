using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarkAndBarker.Shared.Persistence.Models
{
    public class ModelProperty : IModel
    {
        public string PropertyBlueprint { get; set; }
        public short PropertyValue { get; set; }
    }
}
