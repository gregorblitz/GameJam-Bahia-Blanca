using System.Collections.Generic;
using UnityEngine;

public class ShelfManager : MonoBehaviour
{
    public static ShelfManager Instance { get; private set; }

    [Header("Ranuras de la Estantería")]
    [SerializeField] private List<DrugSlot> allShelfSlots = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Si la lista está vacía busca automáticamente todos los DrugSlot hijos
        if (allShelfSlots.Count == 0)
        {
            allShelfSlots.AddRange(GetComponentsInChildren<DrugSlot>());
        }
    }

    // Intenta colocar una medicina en el primer DrugSlot libre disponible.
    public bool TryPlaceDrugInShelf(GameObject drugPrefab)
    {
        if (drugPrefab == null)
        {
            Debug.LogError("No se asignó el Prefab del medicamento.");
            return false;
        }

        // Busca el primer slot que no esté lleno
        foreach (DrugSlot slot in allShelfSlots)
        {
            if (slot != null && !slot.IsFull)
            {
                // Instancia la caja física del medicamento
                GameObject drugObj = Instantiate(drugPrefab);
                Drug drugComponent = drugObj.GetComponent<Drug>();

                if (drugComponent == null)
                {
                    Debug.LogError($"El prefab {drugPrefab.name} no contiene el componente Drug.");
                    Destroy(drugObj);
                    return false;
                }

                // Lo ubica dentro del DrugSlot usando su método nativo
                bool placed = slot.PlaceDrug(drugComponent);
                if (placed)
                {
                    Debug.Log($"Medicamento {drugComponent.Data?.DrugName} colocado en {slot.name}.");
                    return true;
                }
                else
                {
                    Destroy(drugObj);
                }
            }
        }

        Debug.LogWarning("No hay espacio libre en la estantería para colocar este medicamento.");
        return false;
    }
}
