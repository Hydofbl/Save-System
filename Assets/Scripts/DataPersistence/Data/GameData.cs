[System.Serializable]
public class GameData
{
    public string playerName;
    public int money;
    public int health;
    public int healthPotionCount;
    public int manaPotionCount;

    public GameData()
    {
        playerName = "user";
        money = 0;
        health = 100;
        healthPotionCount = 0;
        manaPotionCount = 0;
    }


}
