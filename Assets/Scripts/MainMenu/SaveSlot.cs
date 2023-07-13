using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private string profileId = "";

    [Header("Content")]
    [SerializeField] private GameObject noDataPanel;
    [SerializeField] private GameObject hasDataPanel;
    [SerializeField] private TMP_Text profileIdText;
    [SerializeField] private TMP_Text playerNameText;

    private Button saveSlotButton;

    private void Awake()
    {
        saveSlotButton = GetComponent<Button>();
    }

    public void SetData(GameData data)
    {
        if(profileId != null && data != null) 
        {
            noDataPanel.SetActive(false);
            hasDataPanel.SetActive(true);

            playerNameText.text = data.playerName;
            profileIdText.text = profileId;
        }
        else
        {
            hasDataPanel.SetActive(false);
            noDataPanel.SetActive(true);
        }
    }

    public string GetProfileId()
    {
        return profileId;
    }

    public void SetInteractable(bool interactable) 
    {
        saveSlotButton.interactable = interactable;
    }
}
