// Created by Florian, Mehdi-Antoine & Pascale

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Extinction.Enums;

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

            protected UnitBehavior _unitBehavior;

            public UnitBehavior UnitBehaviour
            {
                get { return _unitBehavior; }
                set { _unitBehavior = value; }
            }

            /// <summary>
            /// canAttack is false if the special robot can't attack (robot shut down, destroy,...)
            /// </summary>
            [SerializeField]
            protected bool _canAttack = true;

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            /// <summary>
            /// return true if this agent is able to attack this a weapon
            /// </summary>
            public virtual bool canAttack()
            {
                return _canAttack;
            }

            /// <summary>
            /// return true if this agent can directly attack the target
            /// </summary>
            public virtual bool canAttack( Character target )
            {
                return _canAttack;
            }

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
