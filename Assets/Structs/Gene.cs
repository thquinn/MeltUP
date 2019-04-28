using System.Collections.Generic;
using UnityEngine;

namespace Assets.Structs {
    public class Gene {
        public GeneID id;

        public Gene(GeneID id) {
            this.id = id;
        }
    }

    public enum GeneID {
        Speed, Jump, JumpControl, AirControl
    }
}
