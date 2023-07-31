using System;
using UnityEngine;
using UnityEngine.Events;

namespace Main.Scripts.Player
{
    public class ScoreCounter : MonoBehaviour
    {
        private int _score = 0;
        public UnityEvent<int> onScoreChange;

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Plant"))
            {
                other.gameObject.tag = "Untagged";
                _score++;
                Debug.Log("Score: " + _score);
                onScoreChange?.Invoke(_score);
            }
        }
    }
}