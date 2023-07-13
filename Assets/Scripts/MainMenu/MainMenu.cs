using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Navigation")]
    [SerializeField] private SaveSlotMenu saveSlotMenu;

    [Header("Menu Buttons")]
    [SerializeField] private Button continueButton;
    [SerializeField] private Button loadButton;

    private void Start()
    {
        // if there is a save file,
        // then continue button is interactable
        // else it is not interactable
        bool isThereAnySave = DataPersistenceManager.Instance.fileDataHandler.TryLoad(DataPersistenceManager.Instance.selectedProfileId);
        continueButton.interactable = isThereAnySave;
        loadButton.interactable = isThereAnySave;
    }

    public void OnNewGameButtonClicked()
    {
        saveSlotMenu.ActivateMenu(false);
        DeactivateMenu();
    }

    public void OnLoadGameButtonClicked()
    {
        saveSlotMenu.ActivateMenu(true);
        DeactivateMenu();
    }

    public void OnContinueButtonClicked()
    {
        // After the next scene loaded, OnSceneLoaded() method will call Load() method.
        // So it gets last saved datas
        SceneManager.LoadSceneAsync("MarketScene");
    }

    public void OnExitButtonClicked()
    {
        Application.Quit();
    }

    public void ActivateMenu()
    {
        gameObject.SetActive(true);
    }

    public void DeactivateMenu() 
    {
        gameObject.SetActive(false);
    }
}
