using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.vvv.tennis
{
    public class ScoreVisuals : MonoBehaviour
    {
        #region Private Fields

        [Tooltip("UI Text to display Player's Name")]
        [SerializeField]
        private TMP_Text playerNameText;

        /// <summary>
        /// Dots to display score
        /// </summary>
        [SerializeField]
        private Image[] _dots;

        #endregion


        public void SetName(string name)
        {
            playerNameText.text = name;
        }

        public void SetScore(int score)
        {

            // Update vis
            for (int i=0; i< _dots.Length; ++i)
            {
                if (_dots[i] != null)
                {
                    if (score > i)
                    {
                        _dots[i].color = Color.white;
                    }
                    else
                    {
                        _dots[i].color = Color.black;
                    }
                }
            }
        }
    }
}