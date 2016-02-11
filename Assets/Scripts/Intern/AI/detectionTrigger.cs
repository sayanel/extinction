using UnityEngine;
using System.Collections;
using Extinction.AI;
using Extinction.Characters;

public class detectionTrigger : Creaker
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // We check for any collider collision 
    public void OnTriggerEnter(Collider other)
    {

        Vector3 enemyPos = other.transform.position;
        Vector3 direction = Vector3.Normalize(enemyPos - this._position);
        Debug.Log(this.tag);
        if (_AIstate == AIState.WANDER)
        {
            // If the entering collider is the survivor himself (we are on him) we change the state to ATTACK
            if (other.gameObject.tag == "rangeCollider") // range collider
            {

                Character survivor = other.gameObject.transform.parent.gameObject.GetComponent<Survivor>();
                _characterTarget = survivor;
                _target = _characterTarget.transform;
                attackSurvivor((Survivor)survivor);
                Debug.Log(this.gameObject.name + " : I AM ATTACKING THE SURVIVOR");
                _AIstate = AIState.ATTACK;
            }

            // If the entering collider is the stealthCollider of the survivor we follow the survivor
            // We need to cast a ray to check if the creaker can see the survivor 
            else if (other.gameObject.tag == "stealthCollider") // stealth collider
            {

                Character survivor = other.gameObject.transform.parent.gameObject.GetComponent<Survivor>();
                _characterTarget = survivor;
                _target = _characterTarget.transform;
                Debug.Log(this.gameObject.name + " : I AM FOLLOWING THE SURVIVOR!");
                _AIstate = AIState.FOLLOWSURVIVOR;
            }

            //If the entering collider is an other creaker
            else if (other.gameObject.tag == "detectionCollider") // detection collider, other creaker
            {
                AIState creakerState = other.gameObject.transform.parent.gameObject.GetComponent<Creaker>().getState();

                _characterTarget = other.gameObject.transform.parent.gameObject.GetComponent<Creaker>();
                _target = _characterTarget.transform;
                //_target = other.gameObject.GetComponent<Creaker>().getTarget();

                if (creakerState != AIState.WANDER) // if the other creaker is following a survivor or another creaker we follow him
                {
                    _AIstate = AIState.FOLLOWCREAKER;
                }
                else
                {
                    _AIstate = AIState.FOLLOWCREAKER;
                }

                Debug.Log(this.gameObject.name + " : IS FOLLOWING CREAKER " + getTarget().gameObject.name);
            }
        }

        else if (_AIstate == AIState.FOLLOWCREAKER)
        {
            // If the entering collider is the survivor himself (we are on him) we change the state to ATTACK
            if (other.gameObject.tag == "rangeCollider") // range collider
            {

                Character survivor = other.gameObject.transform.parent.gameObject.GetComponent<Survivor>();
                _characterTarget = survivor;
                _target = _characterTarget.transform;
                attackSurvivor((Survivor)survivor);
                Debug.Log(this.gameObject.name + " : I AM ATTACKING THE SURVIVOR");
                _AIstate = AIState.ATTACK;
            }

            // If the entering collider is the stealthCollider of the survivor we follow the survivor
            // We need to cast a ray to check if the creaker can see the survivor 
            else if (other.gameObject.tag == "stealthCollider") // stealth collider
            {

                Character survivor = other.gameObject.transform.parent.gameObject.GetComponent<Survivor>();
                _characterTarget = survivor;
                _target = _characterTarget.transform;
                Debug.Log(this.gameObject.name + " : I AM FOLLOWING THE SURVIVOR!");
                _AIstate = AIState.FOLLOWSURVIVOR;
            }

            //If the entering collider is an other creaker
            else if (other.gameObject.tag == "detectionCollider") // detection collider, other creaker
            {
                //TODO: Implémenter gestion des groupes dans la Horde
                Debug.Log(this.gameObject.name + " JUST PASSED BY " + getTarget().gameObject.name);
            }
        }

        else if (_AIstate == AIState.FOLLOWSURVIVOR)
        {
            // If the entering collider is the survivor himself (we are on him) we change the state to ATTACK
            if (other.gameObject.tag == "rangeCollider") // range collider
            {
                Character survivor = other.gameObject.transform.parent.gameObject.GetComponent<Survivor>();
                _characterTarget = survivor;
                _target = _characterTarget.transform;
                attackSurvivor((Survivor)survivor);
                Debug.Log(this.gameObject.name + " : I AM ATTACKING THE SURVIVOR");
                _AIstate = AIState.ATTACK;
            }
        }


    }

    public void OnTriggerExit(Collider other)
    {
        // If the exiting collider is the survivor himself (we are not on him anymore) we follow him
        //if (other.gameObject.GetComponent<Character>() == _target && (_AIstate == AIState.ATTACK || _AIstate == AIState.FOLLOWCREAKER))
        //{
        //    _AIstate = AIState.FOLLOWSURVIVOR;
        //    followTarget();
        //    Debug.Log(this.gameObject.name + " : I AM FOLLOWING <AGAIN> THE SURVIVOR");
        //}

        //if (other.gameObject.tag == "rangeCollider" && _AIstate == AIState.ATTACK)
        //{

        //    Debug.Log(this.gameObject.name + " : I AM FOLLOWING <AGAIN> THE SURVIVOR " + other.gameObject.name);
        //}

        //else if (other.gameObject.tag == "stealthCollider")
        //{

        //    Debug.Log(this.gameObject.name + " : I AM WANDERING <AGAIN> ");
        //}

        if (_AIstate == AIState.FOLLOWCREAKER)
        {
            //If the exit collider is an other creaker
            if (other.gameObject.tag == "detectionCollider") // detection collider, other creaker
            {
                _AIstate = AIState.WANDER;
                Debug.Log(this.gameObject.name + " EXIT TRIGGER CREAKER COLLIDER " + getTarget().gameObject.name);
            }
        }

        else if (_AIstate == AIState.FOLLOWSURVIVOR)
        {
            if (other.gameObject.tag == "stealthCollider") // stealth collider
            {
                _AIstate = AIState.WANDER;
                //_characterTarget = null;
                Debug.Log(getTarget());
                Debug.Log(this.gameObject.name + " : EXIT STEALTH COLLIDER");
            }
        }

        else if (_AIstate == AIState.ATTACK)
        {
            if (other.gameObject.tag == "rangeCollider") // range collider
            {
                _AIstate = AIState.FOLLOWSURVIVOR;
                Debug.Log(this.gameObject.name + " : EXIT RANGE COLLIDER");
            }
        }


    }


}
