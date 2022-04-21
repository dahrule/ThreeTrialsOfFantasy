using Normal.Realtime;
using TMPro;
using UnityEngine;

public class AvatarAttributesSync : RealtimeComponent<AvatarAttributesModel>
{
    [SerializeField] TextMeshProUGUI playerNameText;

    private string playerName;

    private static string[] adjectives = new string[] { "Magical", "Cool", "Nice", "Funny", "Fancy", "Glorious", "Weird", "Awesome" };

    private static string[] nouns = new string[] { "Weirdo", "Guy", "Santa Claus", "Dude", "Mr. Nice Guy", "Dumbo" };

    private bool _isSelf;

    public string characterType;
    

    public string Nickname => model.nickname;

    private void Start()
    {
        if (GetComponent<RealtimeAvatarFork>().isOwnedLocallyInHierarchy)
        {
            _isSelf = true;

            // Generate a funny random name
            playerName = adjectives[UnityEngine.Random.Range(0, adjectives.Length)] + " " + nouns[UnityEngine.Random.Range(0, nouns.Length)] + " " + characterType;

            // Assign the nickname to the model which will automatically be sent to the server and broadcast to other clients
            model.nickname = playerName;

            // We don't need to see our own nickname
            //playerNameText.enabled = false;
        }
    }

    protected override void OnRealtimeModelReplaced(AvatarAttributesModel previousModel, AvatarAttributesModel currentModel)
    {
        if (previousModel != null)
        {
            // Unregister from events
            previousModel.nicknameDidChange -= NicknameDidChange;
        }

        if (currentModel != null)
        {
            if (currentModel.isFreshModel)
            {
                currentModel.nickname = "";
            }

            UpdatePlayerName();

            currentModel.nicknameDidChange += NicknameDidChange;
        }
    }

    private void NicknameDidChange(AvatarAttributesModel model, string nickname)
    {
        UpdatePlayerName();
    }

    private void UpdatePlayerName()
    {
        // Update the UI
        playerNameText.text = model.nickname;
    }

    private void Update()
    {
        Debug.Log(characterType);
    }
}