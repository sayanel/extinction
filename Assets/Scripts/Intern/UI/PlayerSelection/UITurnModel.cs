using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UITurnModel : MonoBehaviour 
{
    [SerializeField]
    float _rotationSpeed = -1f;

    [SerializeField]
    Transform _model;

    void Start()
    {
        //if model is null, try to find a Transform called AnchorModel, and get it's first child (normaly, it's the model we want) to initialyze _model parameter.
        _model = transform.parent.Find( "AnchorModel" ).GetChild( 0 );
    }

    //Turn the model.
    public void turn( float rotation )
    {
        if( _model != null )
            _model.rotation = Quaternion.Euler( 0, rotation * _rotationSpeed, 0 );
    }
}
