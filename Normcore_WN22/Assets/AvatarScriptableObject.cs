using UnityEngine;

[CreateAssetMenu(fileName = "AvatarScriptableObject", menuName = "ScriptableObjects/AvatarScriptableObject", order = 1)]
public class AvatarScriptableObject : ScriptableObject
{
    public string prefabName;
    public GameObject avatarPrefab;
    public GameObject spawnPoint;
    public MonoBehaviour[] skills; 
}