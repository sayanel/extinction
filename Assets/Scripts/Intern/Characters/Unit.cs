// Created by Florian, Mehdi-Antoine & Pascale

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Extinction
{
    namespace Characters
    {
        public abstract class Unit : Character
        {
            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------

            /// <summary>
            /// List of potentials targets near the target
            /// </summary>
            protected List<Character> _potentialTargets;

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            /// <summary>
            /// Add a potential target to potentialTargets List
            /// </summary>
            /// <param name="target">The Character to add as a potential target</param>
            public abstract void addPotentialTarget( Character target );

            /// <summary>
            /// Remove a Character from the potentialTargets List if it's present
            /// </summary>
            /// <param name="target">The Character to remove</param>
            public abstract void removePotentialTarget( Character target );

            /// <summary>
            /// Returns the nearest target
            /// </summary>
            public abstract Character getPriorityTarget();

            public abstract Character getTarget( int index );

            /// <summary>
            /// Stop PathFinding
            /// </summary>
            public abstract void stopWalking();

            /// <summary>
            /// Attack a new target.
            /// Can be implemented the way you like
            /// </summary>
            /// <param name="target"></param>
            public abstract void attack( Character target );

            /// <summary>
            /// launch an attack function
            /// example : fire with a weapon
            /// </summary>
            public abstract void attack();

        }
    }
}
