using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemyTouch : MonoBehaviour
{
    [SerializeField] private GameOver gameOver;

    private void Awake()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponentInParent<PlayerMovement>() != null)
            gameOver.Lose();
    }
}