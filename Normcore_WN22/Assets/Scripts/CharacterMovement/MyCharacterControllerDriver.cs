using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Updates the character controller position each frame to match the player's head movement.
/// </summary>
public class MyCharacterControllerDriver:CharacterControllerDriver
{
    // Update is called once per frame
    void Update()
    {
        UpdateCharacterController();
    }

    protected override void UpdateCharacterController()
    {
        base.UpdateCharacterController();

    }
}
