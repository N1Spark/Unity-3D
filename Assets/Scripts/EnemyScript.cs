using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private Transform control;
    void Start()
    {
        control = transform.Find("Control");
    }

    void Update()
    {
        this.transform.position = control.position;
        control.localPosition = Vector3.zero;
    }
}
