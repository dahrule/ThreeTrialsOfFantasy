using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using System;

public class PlayerInfo : RealtimeComponent<PlayerInfoModel>
{
    private string playerName;
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
                currentModel.playerName = playerName
                    ;

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
        playerName = model.playerName;
    }

    public void SetPlayerName(string name)
    {
        model.playerName = name;
    }
}
