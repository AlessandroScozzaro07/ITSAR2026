using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUi : MonoBehaviour
{
    [SerializeField] Image BarFill;
    [SerializeField] TMP_Text healthValueText;
    Health myHealth; 
    private void Awake()
    {
        if (myHealth == null)
        {
                
        }

    }
    private void OnEnable()
    {
        if (myHealth == null) myHealth = Health.PlayerHealth;
        myHealth.HealthChanged += UpdateBarValue;
        
    }
    private void OnDisable()
    {
        myHealth.HealthChanged -= UpdateBarValue;
    }
    void UpdateBarValue(float curHp, float maxHp)
    {
        BarFill.fillAmount = curHp / maxHp;
        healthValueText.text = $"{curHp}/{maxHp}";
    }
}
