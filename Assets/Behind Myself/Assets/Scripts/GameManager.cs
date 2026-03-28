using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int coins = 0;
    public TMP_Text coinText;
    private ScriptableObjectsManager soManager;

    void Awake()
    {
        Instance = this;
    }

    public void AddCoin(int amount)
    {
        coins += amount;
        coinText.text = "Coins: " + coins;
    }

    public bool SpendCoins(int amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            coinText.text = "Coins: " + coins;
            return true;
        }
        return false;
    }

    public void GoNextNode()
    {

    }
}