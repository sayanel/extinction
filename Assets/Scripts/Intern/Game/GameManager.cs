// @author: Alex
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Extinction.Enums;

namespace Extinction
{
    namespace Game
    {
        public class GameManager : Utils.SingletonMonoBehavior<GameManager>
        {
            [SerializeField]
            Dictionary<CharacterName, CharacterStatus> _survivorStatus = new Dictionary<CharacterName, CharacterStatus>();
            [SerializeField]
            Dictionary<CharacterName, CharacterStatus> _robotStatus = new Dictionary<CharacterName, CharacterStatus>();
            [SerializeField]
            GameObject winWidget;
            [SerializeField]
            GameObject loseWidget;

            void Start()
            {
                _survivorStatus.Add( CharacterName.Anton, CharacterStatus.Alive );
                _survivorStatus.Add( CharacterName.Hal, CharacterStatus.Alive );
                _survivorStatus.Add( CharacterName.Malik, CharacterStatus.Alive );
                _survivorStatus.Add( CharacterName.Red, CharacterStatus.Alive );

                _robotStatus.Add( CharacterName.RController, CharacterStatus.Alive );
                _robotStatus.Add( CharacterName.RScout, CharacterStatus.Alive );
                _robotStatus.Add( CharacterName.RTank, CharacterStatus.Alive );
            }

            public void changeRobotStatus( CharacterName robotName, CharacterStatus newStatus )
            {
                GetComponent<PhotonView>().RPC( "changeRobotStatus_rpc", PhotonTargets.All, (int)robotName, (int)newStatus );
            }

            [PunRPC]
            public void changeRobotStatus_rpc( int _robotName, int _newStatus )
            {
                CharacterName robotName = (CharacterName)_robotName;
                CharacterStatus newStatus = (CharacterStatus)_newStatus;

                if( _robotStatus.ContainsKey( robotName ) )
                {
                    _robotStatus[robotName] = newStatus;
                    Debug.Log( "new state for robot status " + robotName.ToString() + " = " + _robotStatus.ToString() );
                }

                onChangePlayerStatus();
            }

            public void changeSurvivorStatus( CharacterName survivorName, CharacterStatus newStatus )
            {
                GetComponent<PhotonView>().RPC( "changeSurvivorStatus_rpc", PhotonTargets.All, (int)survivorName, (int)newStatus );
            }

            [PunRPC]
            public void changeSurvivorStatus_rpc( int _survivorName, int _newStatus )
            {
                CharacterName survivorName = (CharacterName)_survivorName;
                CharacterStatus newStatus = (CharacterStatus)_newStatus;

                if( _robotStatus.ContainsKey( survivorName ) )
                {
                    _robotStatus[survivorName] = newStatus;
                    Debug.Log( "new state for robot status " + survivorName.ToString() + " = " + _robotStatus.ToString() );
                }

                onChangePlayerStatus();
            }

            private void onChangePlayerStatus()
            {
                if( !_robotStatus.ContainsValue( CharacterStatus.Alive ) ) {
                    playSurvivorWin();
                }
                else if( !_survivorStatus.ContainsValue( CharacterStatus.Alive ) )
                {
                    playHerbieWin();
                }
            }

            private void displayWinLoseWidget( GameObject winLoseWidgetToPlay )
            {
                //Canvas sceneCanvas = FindObjectOfType<Canvas>();
                //if( sceneCanvas == null )
                //{
                    //Debug.LogError( "Can't find a UI canvas in the scene, can't display win infos." );

                    GameObject winWidgetInstance = Instantiate( winLoseWidgetToPlay, Vector3.zero, Quaternion.identity ) as GameObject;
                    //RectTransform winWidgetInstanceTransform = winWidgetInstance.GetComponent<RectTransform>();
                    //if( winWidgetInstanceTransform != null )
                    //{
                    //    winWidgetInstanceTransform.SetParent( sceneCanvas.transform );
                    //}
                //}
            }

            private void playSurvivorWin()
            {
                Debug.Log( "Survivor win !" );
                GetComponent<PhotonView>().RPC( "playSurvivorWin_rpc", PhotonTargets.All );
            }

            [PunRPC]
            private void playSurvivorWin_rpc()
            {
                GameObject winLoseWidgetToPlay = null;

                if(PhotonNetwork.player.name != "herbie" )
                    winLoseWidgetToPlay = winWidget;
                else
                    winLoseWidgetToPlay = loseWidget;

                displayWinLoseWidget( winLoseWidgetToPlay );
            }

            private void playHerbieWin()
            {
                Debug.Log( "Herbie win !" );
                GetComponent<PhotonView>().RPC( "playHerbieWin_rpc", PhotonTargets.All );
            }

            [PunRPC]
            private void playHerbieWin_rpc()
            {
                GameObject winLoseWidgetToPlay = null;

                if( PhotonNetwork.player.name != "herbie" )
                    winLoseWidgetToPlay = winWidget;
                else
                    winLoseWidgetToPlay = loseWidget;

                displayWinLoseWidget( winLoseWidgetToPlay );
            }
        }
    }
}
