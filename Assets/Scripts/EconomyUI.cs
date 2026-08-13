using UnityEngine;
using TMPro; 

public class EconomyUI : MonoBehaviour
{
    public static EconomyUI Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI totalMoneyText;
    [SerializeField] private TextMeshProUGUI lastEarningText; // Para mostrar lo ganado recientemente

    private float totalMoney = 0f;
    private float lastAmount = 0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateUI(0f);
    }

    public void AddEarnings(float basePrice, float tip)
    {
        float earnedThisTurn = basePrice + tip;
        totalMoney += earnedThisTurn;

        UpdateUI(earnedThisTurn);
    }

    private void UpdateUI(float lastAmount)
    {
        if (totalMoneyText != null)
        {
            totalMoneyText.text = $"Dinero Ganado: ${totalMoney:F2}";
        }

        if (lastEarningText != null)
        {
            lastEarningText.text = $"Ultima propina: ${lastAmount:F2}";
        }
    }
}
