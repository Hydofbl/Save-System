using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button continueButton;

    private void Start()
    {
        // if there is a save file,
        // then continue button is interactable
        // else it is not interactable
        continueButton.interactable = DataPersistenceManager.Instance.fileDataHandler.TryLoad();
    }

    public void OnNewGameButtonClicked()
    {
        // Creates a new gameData with default values
        DataPersistenceManager.Instance.NewGame();

        // Save the newly created gameData before the new scene
        DataPersistenceManager.Instance.SaveGame();

        // When MainScene unloaded, newly created gameData will be saved.
        // Because of the SaveGame() method inside the OnSceneUnloaded()
        SceneManager.LoadSceneAsync("MarketScene");
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
}
