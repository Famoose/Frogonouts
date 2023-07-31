using System.Collections;
using Main.Scripts.Player;
using UnityEngine;
using UnityEngine.UIElements;

namespace Main.Scripts.ui
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private ScoreCounter _scoreCounter;
        [SerializeField] private Movement _movement;
        [SerializeField] private DeathTracker _deathTracker;

        private VisualElement _root;
        private Label _score;

        public void Start()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;

            _score = _root.Q<Label>("score");
            _scoreCounter.onScoreChange.AddListener(UpdateScore);
            _movement.onMoveForce.AddListener(UpdateForce);
        }

        private void UpdateForce(Vector2 arg0)
        {
        }

        public void UpdateScore(int score)
        {
            _score.text = $"Score: {score}";
            StartCoroutine(ShakeVisualElement(_score, 5f, 0.3f));
        }

        //shake visual element amount and duration with use of translate property
        private IEnumerator ShakeVisualElement(VisualElement element, float amount, float duration)
        {
            var originalPos = element.style.translate;
            var elapsed = 0f;
            while (elapsed < duration)
            {
                var y = originalPos.value.y.value + Random.Range(-1f, 1f) * amount;
                element.style.translate = new Translate(originalPos.value.x.value, y);
                elapsed += Time.deltaTime;
                yield return null;
            }

            element.style.translate = originalPos;
        }
    }
}