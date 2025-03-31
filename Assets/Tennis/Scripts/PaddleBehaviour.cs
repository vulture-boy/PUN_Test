using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.vvv.tennis
{

    /// <summary>
    /// Controls the behaviour of the paddle.
    /// </summary>
    public class PaddleBehaviour : MonoBehaviour
    {
        /// <summary>
        /// Cooldown before a boost can be used again
        /// </summary>
        [SerializeField]
        private float boostCooldownTime = 2f;

        /// <summary>
        /// Duration of a boost.
        /// </summary>
        [SerializeField]
        private float boostActiveTime = 0.5f;

        /// <summary>
        /// Material to use when boosted.
        /// </summary>
        [Header(@"Rendering")]
        [SerializeField]
        private Material boostedMaterial;

        /// <summary>
        /// Material to use when not boosted.
        /// </summary>
        [SerializeField]
        private Material unboostedMaterial;

        /// <summary>
        /// Renderer used to display the paddle.
        /// </summary>
        [SerializeField]
        private Renderer paddleRenderer;

        private float boostCooldownTimer = 0f;
        private float boostActiveTimer = 0f;

        private bool boostActive = false;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (boostCooldownTimer > 0f)
            {
                boostCooldownTimer -= Time.deltaTime;
            }

            if (boostActiveTimer > 0f)
            {
                boostActiveTimer -= Time.deltaTime;
                if (boostActiveTimer <= 0f)
                {
                    boostActive = false;
                    paddleRenderer.material = unboostedMaterial;
                }
            }
        }

        public void BoostPaddle()
        {
            if (boostCooldownTimer <= 0f)
            {
                paddleRenderer.material = boostedMaterial;
                boostCooldownTimer = boostCooldownTime;
                boostActiveTimer = boostActiveTime;
                boostActive = true;
            }
            else
            {
                return;
            }
        }

        private void OnTriggerStay(Collider other)
        {

            if (other.TryGetComponent(out BallMotion ball))
            {
                if (ball._lastPaddle == this)
                {
                    return;
                }

                if (boostActive)
                {
                    ball.BoostSpeed();
                }
                ball.ChangeTarget();
                boostActive = false;

                ball._lastPaddle = this;
            }
        }
    }
}
