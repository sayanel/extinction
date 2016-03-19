// @author : mehdi-antoine

using UnityEngine;
using System.Collections;
using Extinction.Controllers;
using Extinction.Cameras;
using Extinction.HUD;

namespace Extinction {
    namespace Characters {
        public class SurvivorComponentActivator : MonoBehaviour
        {
            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------

            [SerializeField]
            private bool _firstPersonMode = true;

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            public void Awake()
            {
                if ( _firstPersonMode ) 
                    firstPersonMode();
                else 
                    thirdPersonMode();
            }

            public void firstPersonMode()
            {
                SkinnedMeshRenderer[] skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
                foreach(SkinnedMeshRenderer renderer in skinnedMeshRenderers){
                    if ( renderer.ToString().Contains( "1st" ) )
                    {
                        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    }
                    else
                    {
                        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                    }
                }

                MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();

                foreach ( MeshRenderer renderer in meshRenderers )
                {
                    if ( renderer.ToString().Contains( "1st" ) )
                    {
                        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    }
                    else
                    {
                        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                    }
                }
            }

            public void thirdPersonMode()
            {
                SkinnedMeshRenderer[] skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
                foreach ( SkinnedMeshRenderer renderer in skinnedMeshRenderers )
                {
                    if ( renderer.ToString().Contains( "3rd" ) )
                    {
                        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                    }
                    else
                    {
                        renderer.enabled = false;
                    }
                }

                MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();

                foreach ( MeshRenderer renderer in meshRenderers )
                {
                    if ( renderer.ToString().Contains( "3rd" ) )
                    {
                        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                    }
                    else
                    {
                        renderer.enabled = false;
                    }
                }

                GetComponentInChildren<InputControllerSurvivor>().enabled = false;
                GetComponentInChildren<Camera>().enabled = false;
                GetComponentInChildren<CameraFPS>().enabled = false;
                GetComponentInChildren<HUDWeaponMarker>().enabled = false;
                GetComponentInChildren<GUILayer>().enabled = false;
                GetComponentInChildren<FlareLayer>().enabled = false;
            }
        }
    }
}

