using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Extinction.FX;
using System;


public class FXShoot : FXEvent 
{

    [SerializeField]
    private ParticleSystem _particleSystem;

    [SerializeField]
    private Light _light;

    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private float _lightOnDuration = 0.5f;

    [SerializeField]
    private float _lightMaxIntensty = 10;

    private Coroutine _lightOnCoroutine;

    void Awake()
    {
        if( _particleSystem == null )
            _particleSystem = GetComponent<ParticleSystem>();

        if( _light == null )
            _light = GetComponent<Light>();

        if( _audioSource == null )
            _audioSource = GetComponent<AudioSource>();
    }

    public override void On()
    {
        if( _particleSystem != null )
        {
            _particleSystem.Play();
        }
        if( _audioSource != null )
        {
            _audioSource.Play();
        }
        if( _light != null )
        {
            _lightOnCoroutine = StartCoroutine(lightOn());
        }
    }

    private IEnumerator lightOn()
    {
        _light.intensity = _lightMaxIntensty;
        yield return new WaitForSeconds( _lightOnDuration );
        _light.intensity = 0;
    }

    public override void Off()
    {
        if( _particleSystem != null )
        {
            _particleSystem.Stop();
        }
        if( _audioSource != null )
        {
            _audioSource.Stop();
        }
        if(_lightOnCoroutine != null)
        {
            StopCoroutine( _lightOnCoroutine );
        }
    }
}
