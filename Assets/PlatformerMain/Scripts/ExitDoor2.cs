using System.Collections;
using UnityEngine;

public class Level2ExitDoor : MonoBehaviour
{
    [Header("Посилання")]
    [SerializeField] private PlayerInventory player;
    [SerializeField] private SpriteRenderer doorRenderer;
    [SerializeField] private Sprite openDoorSprite;
    [SerializeField] private Collider2D doorCollider;
    [SerializeField] private GameObject victoryPanel;

    [Header("Взаємодія")]
    [SerializeField, Min(0f)] private float interactionDistance = 2f;
    [SerializeField, Min(0f)] private float openDelay = 0.4f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private bool isOpening;

    private void Update()
    {
        if (isOpening || player == null || Time.timeScale == 0f ||
            !Input.GetKeyDown(interactKey))
            return;

        Vector2 offset = player.transform.position - transform.position;

        if (offset.sqrMagnitude > interactionDistance * interactionDistance)
            return;

        if (!player.HasKey)
        {
            Debug.Log("Потрібен ключ!", this);
            return;
        }

        if (victoryPanel == null)
        {
            Debug.LogError("Признач панель у полі Victory Panel.", this);
            return;
        }

        StartCoroutine(OpenAndWin());
    }

    private IEnumerator OpenAndWin()
    {
        isOpening = true;

        if (doorRenderer != null && openDoorSprite != null)
            doorRenderer.sprite = openDoorSprite;

        if (doorCollider != null)
            doorCollider.enabled = false;

        // Якщо гравець програє під час відкриття, відлік зупиниться.
        yield return new WaitForSeconds(openDelay);

        victoryPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}