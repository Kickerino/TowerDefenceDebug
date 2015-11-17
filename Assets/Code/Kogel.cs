using UnityEngine;
using System.Collections;

public class Kogel : MonoBehaviour
{
    //snelheid van de kogel
    public float Speed = 5;

    void Update()
    {
        Vector3 movementVector = this.transform.right * (Speed * Time.deltaTime);
        this.transform.position += movementVector;
        Invoke("killme", 5f);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.GetComponent<Enemy>() != null)
        {
            Debug.Log("Enemy hit!");
        }
    }

    void killme()
    {
        Destroy(this.gameObject);
    }
}
