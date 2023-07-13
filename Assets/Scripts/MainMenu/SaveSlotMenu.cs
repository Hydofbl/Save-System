using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveSlotMenu : MonoBehaviour
{
    [Header("Menu Navigation")]
    [SerializeField] private MainMenu mainMenu;

    [Header("Menu Buttons")]
    [SerializeField] private Button backButton;

    private SaveSlot[] saveSlots;

    private bool isLoadingGame = false;

    private void Awake()
    {
        saveSlots = GetComponentsInChildren<SaveSlot>();
    }

    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        // Disable all buttons
        DisableMenuButtons();

        // Change selected profile id
        DataPersistenceManager.Instance.ChangeSelectedProfileId(saveSlot.GetProfileId());

        if(!isLoadingGame)
        {
            // Create New Game
            DataPersistenceManager.Instance.NewGame();

            // Before loading new scene, we have to save the data
            DataPersistenceManager.Instance.SaveGame();
        }

        // Load Scene
        SceneManager.LoadSceneAsync("MarketScene");
    }

    public void OnBackButtonClicked()
    {
        mainMenu.ActivateMenu();
        DeactivateMenu();
    }

    public void ActivateMenu(bool isLoadingGame)
    {
        gameObject.SetActive(true);

        // Set saveslotmenu mode
        this.isLoadingGame = isLoadingGame;

        // Load all profiles that exist
        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.Instance.GetAllProfilesGameData();

        foreach(SaveSlot saveSlot in saveSlots) 
        {
            GameData data = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out data);
            saveSlot.SetData(data);

            if(data == null && isLoadingGame)
            {
                saveSlot.SetInteractable(false);
            }
            else
            {
                saveSlot.SetInteractable(true);
            }
        }
    }

    public void DeactivateMenu() 
    {
        gameObject.SetActive(false);
    }

    private void DisableMenuButtons()
    {
        foreach(SaveSlot saveSlot in saveSlots)
        {
            saveSlot.SetInteractable(false);
        }

        backButton.interactable = false;
    }
}
