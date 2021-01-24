using UnityEngine;
using UnityEngine.Networking;


public class CharacterSelectUI : MonoBehaviour
{
    public static CharacterSelectUI instance;

    [SerializeField] private GameObject _loginPanel = null;
    [SerializeField] private GameObject _selectPanel = null;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one instance of CharacterSelectUI found!");
            return;
        }
        instance = this;
    }

    public void OpenPanel()
    {
        _loginPanel.SetActive(false);
        _selectPanel.SetActive(true);
    }

    public void SelectCharacter(NetworkIdentity characterIdentity)
    {
        CharacterSelect.Instance.SelectCharacter(characterIdentity.assetId);
    }
}