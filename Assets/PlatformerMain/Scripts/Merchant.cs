using UnityEngine;

public class Merchant : MonoBehaviour
{
    [Header("Ссылки")]
    [SerializeField] private PlayerInventory player;
    [SerializeField] private BananaCounter bananaCounter;

    [Header("Торговля")]
    [SerializeField, Min(1)] private int keyPrice = 5;
    [SerializeField, Min(0f)] private float interactionDistance = 2f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private void Update()
    {
        if (player == null || bananaCounter == null)
            return;

        float distanceSquared =
            (player.transform.position - transform.position).sqrMagnitude;

        if (distanceSquared > interactionDistance * interactionDistance)
            return;

        if (Input.GetKeyDown(interactKey))
            TryBuyKey();
    }

    private void TryBuyKey()
    {
        if (player.HasKey)
        {
            Debug.Log("У тебя уже есть ключ.");
            return;
        }

        if (!bananaCounter.TrySpend(keyPrice))
        {
            Debug.Log($"Недостаточно бананов. Нужно: {keyPrice}.");
            return;
        }

        player.GiveKey();
        Debug.Log("Ключ куплен!");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}