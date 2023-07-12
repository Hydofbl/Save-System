using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MarketManager : MonoBehaviour, IDataPersistance
{
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text HPotAmountText;
    [SerializeField] private TMP_Text MPotAmountText;

    private int moneyCount = 0;
    private int healthAmount = 100;
    private int HPotCount = 0;
    private int MPotCount = 0;

    private void Update()
    {
        moneyText.text = moneyCount.ToString();
        healthText.text = healthAmount.ToString() + "/100";
        HPotAmountText.text = HPotCount.ToString();
        MPotAmountText.text = MPotCount.ToString();
    }

    public void LoadData(GameData data)
    {
        moneyCount = data.money;
        healthAmount = data.health;
        HPotCount = data.healthPotionCount;
        MPotCount = data.manaPotionCount;
    }

    public void SaveData(ref GameData data)
    {
        data.money = moneyCount;
        data.health = healthAmount;
        data.healthPotionCount = HPotCount;
        data.manaPotionCount = MPotCount;
    }

    public void Work(int amount)
    {
        moneyCount += 500;
    }

    public void BuyHealthPotion()
    {
        if(moneyCount >= 250)
        {
            // HealthPot cost
            moneyCount -= 250;
            HPotCount++;
        }
    }

    public void BuyManaPotion()
    {
        if (moneyCount >= 250)
        {
            // ManaPot cost
            moneyCount -= 250;
            MPotCount++;
        }
    }

    public void GetDamage(int amount)
    {
        healthAmount -= amount;

        if (healthAmount < 0)
            healthAmount = 0;
    }

    public void UseHealthPotion()
    {
        if(HPotCount > 0 && healthAmount < 100)
        {
            HPotCount--;
            healthAmount += 25;

            if (healthAmount > 100)
                healthAmount = 100;
        }
    }

    public void UseManaPotion()
    {
        if (MPotCount > 0)
        { 
            MPotCount--;
        }
    }

    public void Save()
    {
        DataPersistenceManager.Instance.SaveGame();
    }

    public void Load()
    {
        DataPersistenceManager.Instance.LoadGame();
    }

    public void ExitMarket()
    {
        DataPersistenceManager.Instance.SaveGame();

        SceneManager.LoadSceneAsync("MainMenuScene");
    }
}
