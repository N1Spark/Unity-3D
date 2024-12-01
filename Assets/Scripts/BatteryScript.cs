using UnityEngine;

public class BatteryScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameState.Collect(gameObject.name);
        Destroy(gameObject);
    }
}
