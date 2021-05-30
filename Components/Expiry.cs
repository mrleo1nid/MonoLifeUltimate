using System;
using System.Collections.Generic;
using System.Text;

namespace MonoLifeUltimate.Components
{
    public class Expiry
    {
        public Expiry(float timeRemaining)
        {
            TimeRemaining = timeRemaining;
        }

        public float TimeRemaining;
    }
}
