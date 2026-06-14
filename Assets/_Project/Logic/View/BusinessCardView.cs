using _Project.Logic.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Logic.View
{
    public class BusinessCardView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _incomeText;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _levelUpText;
        [SerializeField] private TextMeshProUGUI _upgrade1Text;
        [SerializeField] private TextMeshProUGUI _upgrade2Text;
        
        [SerializeField] private Slider _incomeProgressSlider;
        
        [SerializeField] private Button _levelUpButton;
        [SerializeField] private Button _upgrade1Button;
        [SerializeField] private Button _upgrade2Button;
        
        public Button.ButtonClickedEvent OnLevelUpClick => _levelUpButton.onClick;
        public Button.ButtonClickedEvent OnUpgrade1Click => _upgrade1Button.onClick;
        public Button.ButtonClickedEvent OnUpgrade2Click => _upgrade2Button.onClick;
        
        public void SetNameText(string value) => _nameText.text = value;
        
        public void SetIncomeProgress(float value) => _incomeProgressSlider.value = value;
        
        public void SetIncomeText(int value) => _incomeText.text = $"Income: \n {value}";

        public void SetLevelText(int value) => _levelText.text = $"Level: \n {value}";
        
        public void SetLevelUpgradeCost(int cost) => _levelUpText.text = $"LEVEL UP: \n Price: {cost}$";

        public void SetUpgrade1Interactable(bool value) => _upgrade1Button.interactable = value;
        public void SetUpgrade1Text(string text) => _upgrade1Text.text = text;

        public void SetUpgrade2Interactable(bool value) => _upgrade2Button.interactable = value;
        public void SetUpgrade2Text(string text) => _upgrade2Text.text = text;
    }
}