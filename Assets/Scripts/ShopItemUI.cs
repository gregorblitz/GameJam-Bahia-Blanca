using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemUI : MonoBehaviour
{
    [Header("Datos")]
    [SerializeField] private DrugData drugData;
    [SerializeField] private GameObject drugPhysicalPrefab; // Prefab con Drug, SpriteRenderer y BoxCollider2D

    [Header("UI")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Button buyButton;

    public void Setup(DrugData data, GameObject prefab, Sprite iconSprite)
    {
        drugData = data;
        drugPhysicalPrefab = prefab;

        if (nameText != null && data != null)
            nameText.text = data.DrugName;

        if (costText != null && data != null)
            costText.text = $"${data.Cost:F0}";

        if (iconImage != null && iconSprite != null)
            iconImage.sprite = iconSprite;

        if (buyButton != null)
        {
            //buyButton.onClick.RemoveAllListeners();
            //buyButton.onClick.AddListener(OnBuyClicked);
        }
    }

    private void Start()
    {
        if (buyButton != null)
            //buyButton.onClick.AddListener(OnBuyClicked);

        ActualizarTextos();
    }

    public void ActualizarTextos()
    {
        if (drugData != null)
        {
            if (nameText != null) nameText.text = drugData.DrugName;
            if (costText != null) costText.text = $"${drugData.Cost:F0}";
        }
    }

    public void OnBuyClicked()
    {
        Debug.LogWarning("OnBuyClicked method invoked.");
        if (drugData == null || drugPhysicalPrefab == null)
        {
            Debug.LogWarning("Faltan datos de DrugData o Prefab en este botón.");
            return;
        }

        // Verifica si hay dinero suficiente
        if (EconomyUI.Instance != null && EconomyUI.Instance.TotalMoney < drugData.Cost)
        {
            Debug.LogWarning("Dinero insuficiente.");
            return;
        }

        // 2. Verifica si hay espacio en la estantería y lo coloca
        if (ShelfManager.Instance != null)
        {
            bool pudoColocarse = ShelfManager.Instance.TryPlaceDrugInShelf(drugPhysicalPrefab);

            if (pudoColocarse)
            {
                // Descuentas el dinero solo si se colocó exitosamente
                EconomyUI.Instance.TrySpendMoney(drugData.Cost);
            }
        }
    }
}