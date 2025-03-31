using UnityEngine;
using System.Collections;
using Photon.Pun;

namespace com.vvv.tennis
{
    public class PlayerAnimatorManager : MonoBehaviourPun
    {
        #region Private Fields

        [SerializeField]
        float xRange = 9.5f;

        [SerializeField]
        float speedModifier = 0.25f;

        [SerializeField]
        bool flipInput;

        bool hasSetName;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            // We want the joining client to take over this if the input is flipped
            if (!PhotonNetwork.IsMasterClient && flipInput)
            {
                photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (!photonView.IsMine && PhotonNetwork.IsConnected == true)
            {
                return;
            }

            if (!hasSetName && PhotonNetwork.IsConnected == true)
            {
                GameManager.Instance.SetPlayerName(photonView.IsMine, photonView.Owner.NickName);
                hasSetName = true;
            }

            float h = Input.GetAxis("Horizontal");
            h = flipInput ? h * -1 : h;

            transform.localPosition += Vector3.right * h * speedModifier;
 
            transform.localPosition = new Vector3(
                Mathf.Clamp(transform.localPosition.x, xRange * -1, xRange),
                transform.localPosition.y,
                transform.localPosition.z);

        }

        #endregion
    }
}
