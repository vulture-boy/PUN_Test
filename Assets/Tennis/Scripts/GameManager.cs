using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

namespace com.vvv.tennis
{
    public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        #region Public Fields

        public static GameManager Instance;

        [SerializeField]
        private ScoreVisuals playerScoreVisuals;

        [SerializeField]
        private ScoreVisuals opponentScoreVisuals;

        [SerializeField]
        private GameObject p1Camera;

        [SerializeField]
        private GameObject p2Camera;

        [SerializeField]
        private GameObject winVisual;

        [SerializeField]
        private GameObject loseVisual;

        [SerializeField]
        private BallMotion ball;

        /// <summary>
        /// Time to display the win or lose screen.
        /// </summary>
        [SerializeField]
        private float winScreenTime = 5f;

        private bool winnerDeclared;

        private int scoreTotal = 3;
        private int p1Score;
        private int p2Score;

        #endregion

        #region MonoBehaviour Methods

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            p1Score = scoreTotal;
            p2Score = scoreTotal;

            if (PhotonNetwork.IsConnected == true)
            {
                if (p1Camera != null)
                {
                    p1Camera.SetActive(PhotonNetwork.IsMasterClient);
                }

                if (p2Camera != null)
                {
                    p2Camera.SetActive(!PhotonNetwork.IsMasterClient);
                }
            }

            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    playerScoreVisuals.SetName(PhotonNetwork.PlayerList[0].NickName);
                    opponentScoreVisuals.SetName(PhotonNetwork.PlayerList[1].NickName);
                }
                else
                {
                    playerScoreVisuals.SetName(PhotonNetwork.PlayerList[1].NickName);
                    opponentScoreVisuals.SetName(PhotonNetwork.PlayerList[0].NickName);
                }
                
            }
        }

        private void Update()
        {
            if (winnerDeclared && winScreenTime > 0f)
            {
                winScreenTime -= Time.deltaTime;
                if (winScreenTime <= 0f)
                {
                    Instance.LeaveRoom();
                }
            }

            playerScoreVisuals.SetScore(p2Score);
            opponentScoreVisuals.SetScore(p1Score);
        }

        #endregion

        #region Photon Callbacks

        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

                LoadArena();
            }
        }

        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

            if (PhotonNetwork.IsMasterClient && !winnerDeclared)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

                LoadArena();
            }
        }

        #endregion

        #region Public Methods

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void SetPlayerName(bool isPlayer, string name)
        {
            if (isPlayer)
            {
                playerScoreVisuals.SetName(name);
            }
            else
            {
                opponentScoreVisuals.SetName(name);
            }
        }

        public void ReportHit(bool isPlayer)
        {
            if (winnerDeclared)
            {
                return;
            }

            if (isPlayer)
            {
                p1Score--;
            }
            else
            {
                p2Score--;
            }

            CheckEndgame();
        }

        #endregion

        #region Private Methods

        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
                return;
            }
            Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);

            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                PhotonNetwork.LoadLevel("Tennis");
            }
            else if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                PhotonNetwork.LoadLevel("Waiting");
            }
             
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(p1Score);
                stream.SendNext(p2Score);
            }
            else
            {
                // Network player, receive data
                this.p2Score = (int)stream.ReceiveNext();
                this.p1Score = (int)stream.ReceiveNext();

                if (!winnerDeclared)
                {
                    CheckEndgame();
                }
            }
        }

        private void CheckEndgame()
        {
            if (p1Score <= 0 || p2Score <= 0)
            {

                if (p1Score <= 0)
                {
                    winVisual.SetActive(true);
                }
                else
                {
                    loseVisual.SetActive(true);
                }

                winnerDeclared = true;

                //Destroy(ball);
            }
        }

        #endregion
    }
}