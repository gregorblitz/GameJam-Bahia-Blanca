using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private Drug heldDrug;
     [SerializeField] private AudioClip[] playlist;
      [SerializeField] private AudioSource audioSource;

    [SerializeField] private Transform handSlot;

    public Drug HeldDrug => heldDrug;

    public bool IsEmpty => heldDrug == null;

    public bool PickUp(Drug drug)
    {
        if (drug == null || !IsEmpty)
            return false;

        heldDrug = drug;

        drug.transform.SetParent(handSlot);
        drug.transform.localPosition = Vector3.zero;
        drug.transform.localRotation = Quaternion.identity;

       
        audioSource.clip = playlist[Random.Range(0, playlist.Length -1)];
        audioSource.Play();

        return true;
    }

    public Drug Drop()
    {
        if (heldDrug == null)
            return null;

        Drug drug = heldDrug;

        heldDrug = null;

        drug.transform.SetParent(null);
        
        audioSource.clip = playlist[5];
        audioSource.Play();

        return drug;
    }
}