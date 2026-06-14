using TMPro;
using UnityEngine;

namespace _Project.Logic.UI
{
    public class GameplayUI : MonoBehaviour
    {
        [SerializeField] private RectTransform cardsContainer;
        public RectTransform CardsContainer => cardsContainer;

        [SerializeField] private TextMeshProUGUI _balanceText;

        public void SetBalance(int balance)
        {
            _balanceText.text = $"Баланс: {balance}";
        }
    }
}