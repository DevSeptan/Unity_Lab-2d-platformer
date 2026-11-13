using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Banana : MonoBehaviour
{
    [SerializeField] private BananaCounter counter;
    [SerializeField, Min(1)] private int amount = 1;

    private bool collected;

    private void Awake()
    {
        GetComponent<CircleCollider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;

        if (other.GetComponentInParent<PlayerMovement>() == null)
            return;

        if (counter == null) return;

        collected = true;
        counter.AddBanana(amount);
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}