using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Extinction.Characters;
using Extinction.Utils;
using Extinction.Herbie;
using Extinction.Enums;


public class RCamera : Unit, ITriggerable
{
    /// <summary>
    /// A reference to the FogManager owned by herbie.
    /// </summary>
    [SerializeField]
    FogManager _fogManager;

    [SerializeField]
    private HUDLifeBar _worldLifeBar; // WORLD life bar

    /// <summary>
    /// Layer names of the objects which can block the visibility of the robot.
    /// Set to Terrain by default.
    /// </summary>   
    [SerializeField]
    private string[] _terrainMasks = new string[] { "Terrain" };

    public string[] TerrainMasks{
        get{ return _terrainMasks; }
        set{ _terrainMasks = value; }
    }

    public FogManager FogManager
    {
        get { return _fogManager; }
        set { _fogManager = value; }
    }



    void Awake()
    {
        _potentialTargets = new List<Character>();
    }

    void Start()
    {
        setAnimationState( "Idle" );
    }

    public override void activateSkill1()
    {
        //nothing
    }

    public override void activateSkill2()
    {
        //nothing
    }

    public override void addPotentialTarget( Character target )
    {
        _potentialTargets.Add( target );

        Vector3 rayDirection = target.transform.position - transform.position;
        float rayLength = ( rayDirection ).magnitude;
        rayDirection.Normalize();

        if( !Physics.Raycast( transform.position, rayDirection, rayLength, LayerMask.GetMask( _terrainMasks ) ) )
        {
            if( _fogManager != null )
                _fogManager.gameObjectEnterFieldOfView( target.gameObject );
        }
    }

    public override void attack()
    {
        //nothing
    }

    public override void attack( Character target )
    {
        //nothing
    }

    public override void getDamage( int amount )
    {
        float health = _health - amount;
        GetComponent<PhotonView>().RPC( "SetHealth", PhotonTargets.All, health );

        if( _worldLifeBar != null )
        {
            _worldLifeBar.changeHealth( _health, _maxHealth );
        }

        if( _health <= 0 )
        {
            _isAlive = false;
        }
    }

    public override Character getPriorityTarget()
    {
        return null;
    }

    public override Character getTarget( int index )
    {
        return null;
    }

    public override void move( Vector3 vec )
    {
        //nothing
    }

    public override void removePotentialTarget( Character target )
    {
        if( _fogManager )
            _fogManager.gameObjectLeaveFieldOfView( target.gameObject );

        _potentialTargets.Remove( target );
    }

    public override void stopWalking()
    {
        //nothing
    }

    public void triggerEnter( Collider other, string tag )
    {
        Character characterComponent = other.GetComponent<Character>();

        if( characterComponent != null )
        {
            if( characterComponent.getCharacterType() == CharacterType.Survivor )
            {
                addPotentialTarget( characterComponent );
            }
        }
    }

    public void triggerExit( Collider other, string tag )
    {
        Character characterComponent = other.GetComponent<Character>();

        if( characterComponent != null )
        {
            if( characterComponent.getCharacterType() == CharacterType.Survivor )
            {
                removePotentialTarget( characterComponent );
            }
        }
    }

    public override void turn( float angle )
    {
        transform.Rotate( 0, angle, 0 );
    }

    public override void die()
    {
        throw new System.NotImplementedException();
    }
}
