using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class KeyPickup : MonoBehaviour
{
    [SerializeField] private Collider2D playerCollider;

    private CircleCollider2D keyCollider;
    private PlayerInventory inventory;

    private void Awake()
    {
        keyCollider = GetComponent<CircleCollider2D>();
        keyCollider.isTrigger = true;

        if (playerCollider != null)
            inventory = playerCollider.GetComponentInParent<PlayerInventory>();

        if (inventory == null)
        {
            Debug.LogError(
                "У Key Pickup признач колайдер гравця з PlayerInventory.", this);
            enabled = false;
        }
    }

    private void FixedUpdate()
    {
        ColliderDistance2D contact = keyCollider.Distance(playerCollider);

        if (!contact.isValid || !contact.isOverlapped)
            return;

        inventory.GiveKey();
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}