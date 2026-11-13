using TMPro;
using UnityEngine;

public class BananaCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text counterText;

    public int Count { get; private set; }

    private void Start()
    {
        UpdateText();
    }

	public void AddBanana(int amount = 1)
	{
	Count += amount;
	UpdateText();
	}
    

    public bool TrySpend(int amount)
    {
        if (amount < 0 || Count < amount)
            return false;

        Count -= amount;
        UpdateText();
        return true;
    }

    private void UpdateText()
    {
        if (counterText != null)
            counterText.text = $"Banana: {Count}";
    }
}