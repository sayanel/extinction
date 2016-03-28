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

            [SerializeField]
            private bool _offlineMode = true;

            private Survivor _survivor;

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------

            public void Awake()
            {
                _survivor = GetComponent<Survivor>();

                if ( _offlineMode )
                {
                    if(_firstPersonMode) firstPersonMode();
                    else thirdPersonMode();
                    return;
                } 

                thirdPersonMode();
            }

            public void firstPersonMode()
            {
                activateHUD();
                _firstPersonMode = true;
                SkinnedMeshRenderer[] skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
                foreach(SkinnedMeshRenderer renderer in skinnedMeshRenderers){
                    if ( renderer.ToString().Contains( "1st" ) )
                    {
                        renderer.enabled = true;
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
                        renderer.enabled = true;
                        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    }
                    else
                    {
                        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                    }
                }

                GetComponentInChildren<InputControllerSurvivor>().enabled = true;
                GetComponentInChildren<Camera>().enabled = true;
                GetComponentInChildren<CameraFPS>().enabled = true;
                GetComponentInChildren<HUDWeaponMarker>().enabled = true;
                GetComponentInChildren<GUILayer>().enabled = true;
                GetComponentInChildren<FlareLayer>().enabled = true;
            }

            public void thirdPersonMode()
            {
                _firstPersonMode = false;

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

            public void deadMode()
            {
                _firstPersonMode = false;

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
                //GetComponentInChildren<Camera>().enabled = false;
                //////GetComponentInChildren<CameraFPS>().enabled = false;
                GetComponentInChildren<HUDWeaponMarker>().enabled = false;
                GetComponentInChildren<GUILayer>().enabled = false;
                GetComponentInChildren<FlareLayer>().enabled = false;
            }

            public void activateHUD()
            {
                GameObject hudSurvivor =  Instantiate( Resources.Load<GameObject>( "HUD/HUDSurvivor" ) );
                hudSurvivor.GetComponentInChildren<HUDSurvivorHealth>().survivor = _survivor;
                hudSurvivor.GetComponentInChildren<HUDWeaponMagazine>().weapon = _survivor.weapon;
            }
        }
    }
}

