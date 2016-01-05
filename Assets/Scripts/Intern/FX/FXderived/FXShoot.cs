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
    }
}
