using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{
    [SerializeField] private PlayerInventory player;
    [SerializeField] private SpriteRenderer doorRenderer;
    [SerializeField] private Sprite openDoorSprite;
    [SerializeField] private Collider2D doorCollider;

    [SerializeField] private string nextSceneName = "Level2";
    [SerializeField, Min(0f)] private float openDelay = 0.4f;
    [SerializeField, Min(0f)] private float interactionDistance = 2f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private bool isLoading;

    private void Update()
    {
        if (isLoading || player == null || !Input.GetKeyDown(interactKey))
            return;

        Vector2 offset = player.transform.position - transform.position;

        if (offset.sqrMagnitude > interactionDistance * interactionDistance)
            return;

        if (!player.HasKey)
        {
            Debug.Log("Потрібен ключ!", this);
            return;
        }

        if (string.IsNullOrWhiteSpace(nextSceneName) ||
            !Application.CanStreamedLevelBeLoaded(nextSceneName))
        {
            Debug.LogError($"Сцена «{nextSceneName}» недоступна для завантаження.", this);
            return;
        }

        StartCoroutine(OpenAndLoadScene());
    }

    private IEnumerator OpenAndLoadScene()
    {
        isLoading = true;

        if (doorRenderer != null && openDoorSprite != null)
            doorRenderer.sprite = openDoorSprite;

        if (doorCollider != null)
            doorCollider.enabled = false;

        yield return new WaitForSecondsRealtime(openDelay);

        yield return SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Single);
    }
}