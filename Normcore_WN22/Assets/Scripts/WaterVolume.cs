
using UnityEngine;

public class WaterVolume : MonoBehaviour
{
    [Tooltip("The object that defines the surface of this water volume")]
    [SerializeField] Transform _surface;
    public Transform Surface => _surface;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (other.gameObject.TryGetComponent(out Swim component))
            {
                component.waterSurface = _surface;
            }
        }
    }

}
