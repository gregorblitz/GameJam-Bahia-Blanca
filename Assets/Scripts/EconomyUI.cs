using System.Collections;
using UnityEngine;
using TMPro;

public class EconomyUI : MonoBehaviour
{
    public static EconomyUI Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI totalMoneyText;
    [SerializeField] private TextMeshProUGUI lastEarningText;       // Muestra la propina o ganancia del turno
    [SerializeField] private TextMeshProUGUI satisfactionStatusText; // Muestra satisfecho o insatisfecho

    [Header("Aviso Fin de Clientes")]
    [SerializeField] private TextMeshProUGUI noMoreCustomersText; // Texto "Sin clientes"

    private float totalMoney = 0f;

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
        UpdateTotalMoneyUI();
        LimpiarFeedback();

        if (noMoreCustomersText != null)
        {
            noMoreCustomersText.gameObject.SetActive(false);
        }
    }

    // Procesa el resultado de la atención: suma el dinero y muestra el estado en pantalla.
    public void AddEarnings(float basePrice, float tip, float satisfaction)
    {
        float earnedThisTurn = basePrice + tip;
        totalMoney += earnedThisTurn;

        UpdateTotalMoneyUI();

        // Evalua el mensaje y color según el nivel de satisfacción
        string statusMsg = "";
        Color statusColor = Color.white;

        if (satisfaction >= 80f)
        {
            statusMsg = "¡Cliente Muy Satisfecho!";
            statusColor = Color.green;
        }
        else if (satisfaction >= 50f)
        {
            statusMsg = "Cliente Satisfecho";
            statusColor = Color.yellow;
        }
        else
        {
            statusMsg = "Cliente Insatisfecho";
            statusColor = Color.red;
        }

        // Muestra el feedback en pantalla con temporizador
        StopAllCoroutines();
        StartCoroutine(MostrarFeedbackTemporalCo(statusMsg, statusColor, basePrice, tip));
    }

    private IEnumerator MostrarFeedbackTemporalCo(string statusMsg, Color color, float basePrice, float tip)
    {
        // Mostrar estado de satisfacción
        if (satisfactionStatusText != null)
        {
            satisfactionStatusText.text = statusMsg;
            satisfactionStatusText.color = color;
            satisfactionStatusText.gameObject.SetActive(true);
        }

        // Mostrar detalle de la ganancia / propina
        if (lastEarningText != null)
        {
            if (tip > 0)
            {
                lastEarningText.text = $"${basePrice:F0} (Base) + ${tip:F2} (Propina)";
            }
            else
            {
                lastEarningText.text = $"+${basePrice:F0} (Base) | (Sin propina)";
            }
            lastEarningText.gameObject.SetActive(true);
        }

        // Espera 2 segundos visible
        yield return new WaitForSeconds(3.0f);

        // Oculta el mensaje
        LimpiarFeedback();
    }

    // Activa de forma permanente el mensaje en rojo "Sin clientes"

    public void MostrarAvisoSinClientes()
    {
        if (noMoreCustomersText != null)
        {
            noMoreCustomersText.text = "Sin clientes";
            noMoreCustomersText.color = Color.red;
            noMoreCustomersText.gameObject.SetActive(true);
        }
    }

    private void UpdateTotalMoneyUI()
    {
        if (totalMoneyText != null)
        {
            totalMoneyText.text = $"Dinero Ganado: ${totalMoney:F2}";
        }
    }

    private void LimpiarFeedback()
    {

        if (satisfactionStatusText != null)
            satisfactionStatusText.gameObject.SetActive(false);

        if (lastEarningText != null)
            lastEarningText.gameObject.SetActive(false);
    }
}