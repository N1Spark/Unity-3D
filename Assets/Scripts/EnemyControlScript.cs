using UnityEngine;

public class EnemyControlScript : MonoBehaviour
{
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();   
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(Vector3.back * Time.deltaTime);
    }
}
