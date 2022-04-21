using UnityEngine;
using Normal.Realtime;
using TMPro;

/// <summary>
/// Stores the player information to be synchronized
/// </summary>
public class PlayerInfo : RealtimeComponent<PlayerInfoModel>
{
    private string _playerName;
    [SerializeField] TextMeshProUGUI playerNameText;

    private void Start()
    {
        // We don't need to see our own nickname
        //playerNameText.enabled = false;
    }

    protected override void OnRealtimeModelReplaced(PlayerInfoModel previousModel, PlayerInfoModel currentModel)
    {
        if (previousModel != null)
        {
            // Unregister from events
            previousModel.playerNameDidChange -= PlayerNameChange;
        }

        if (currentModel != null)
        {
            // If this is a model that has no data set on it, populate it with the current unity component data
            if (currentModel.isFreshModel)
                currentModel.playerName = _playerName;

            // Update the mesh render to match the new model
            UpdatePlayerName();

            // Register for events so we'll know if the color changes later
            currentModel.playerNameDidChange += PlayerNameChange;
        }
    }

    private void PlayerNameChange(PlayerInfoModel model, string value)
    {
       UpdatePlayerName();
    }

    private void UpdatePlayerName()
    {
        _playerName = model.playerName;
        playerNameText.text = _playerName;
    }

    public void SetPlayerName(string name)
    {
        model.playerName = name;
    }
}
