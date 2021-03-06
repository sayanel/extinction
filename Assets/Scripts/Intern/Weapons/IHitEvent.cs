﻿// @author : Pascale

using UnityEngine;
using System.Collections;

namespace Extinction
{
    namespace Weapons
    {

        public interface IHitEvent
        {
            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------
            void onHit(Vector3 position);
        }
    }
}