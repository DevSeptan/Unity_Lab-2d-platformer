using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private bool hasKey;
    [SerializeField] private GameObject keyIcon;

    public bool HasKey => hasKey;

    private void Awake()
    {
        UpdateKeyIcon();
    }

    public void GiveKey()
    {
        hasKey = true;
        UpdateKeyIcon();
    }

    private void UpdateKeyIcon()
    {
        if (keyIcon != null)
            keyIcon.SetActive(hasKey);
    }
}