using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.vvv.tennis
{ 
    public class BallMotion : MonoBehaviourPun
    {
        [SerializeField]
        private float boostValue = 0.05f;

        [SerializeField]
        private float startStall = 2f;

        [SerializeField]
        private Transform[] p1Posts;

        [SerializeField]
        private Transform[] p2Posts;

        private Vector3 origin;

        private Vector3 target;

        [SerializeField]
        private float baseSpeed = 1f;

        private float multiplier = 1f;

        private bool lastTargetWasP1;

        private float startStallTimer = 0f;

        public PaddleBehaviour _lastPaddle
        {
            get;
            set;
        }

        // Start is called before the first frame update
        void Start()
        {
            if (!photonView.IsMine && PhotonNetwork.IsConnected == true)
            {
                return;
            }

            origin = transform.position;
            ResetBall();
        }

        private void Update()
        {
            if (!photonView.IsMine && PhotonNetwork.IsConnected == true)
            {
                return;
            }

            if (startStallTimer > 0f)
            {
                startStallTimer -= Time.deltaTime;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, target, baseSpeed * multiplier);
            }
        }

        public void BoostSpeed()
        {
            if (!photonView.IsMine && PhotonNetwork.IsConnected == true)
            {
                return;
            }

            multiplier += boostValue;
        }

        public void ChangeTarget()
        {
            if (!photonView.IsMine && PhotonNetwork.IsConnected == true)
            {
                return;
            }

            ChangeTarget(!lastTargetWasP1);
        }

        public void ChangeTarget(bool isP1)
        {
            if (!photonView.IsMine && PhotonNetwork.IsConnected)
            {
                return;
            }

            if (isP1)
            {
                target = Vector3.Lerp(p1Posts[0].position, p1Posts[1].position, Random.value);
            }
            else
            {
                target = Vector3.Lerp(p2Posts[0].position, p2Posts[1].position, Random.value);
            }

            // Use our Y position
            target = new Vector3(target.x, transform.position.y, target.z);

            lastTargetWasP1 = isP1;
        }

        public void ResetBall()
        {
            if (!photonView.IsMine && PhotonNetwork.IsConnected == true)
            {
                return;
            }

            ChangeTarget(Random.value > 0.5f);
            multiplier = 1;
            startStallTimer = startStall;
            transform.position = origin;
            _lastPaddle = null;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!photonView.IsMine && PhotonNetwork.IsConnected == true)
            {
                return;
            }

            if (other.tag == "Wall")
            {
                GameManager.Instance.ReportHit(photonView.IsMine == lastTargetWasP1);
                ResetBall();
            }
        }
    }

}