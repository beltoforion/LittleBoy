using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public enum GameState
    {
        Idle,
        Running
    };

    public enum AtomState
    {
        /// <summary>
        /// Atom is created but does not decay
        /// </summary>
        Inactive,

        /// <summary>
        /// Atom is subject to decay
        /// </summary>
        Alive,

        /// <summary>
        /// Atom will decay in the next timestep
        /// </summary>
        AboutToDecay,

        Decaying,

        /// <summary>
        /// Atom is decayed
        /// </summary>
        Decayed
    };
}
