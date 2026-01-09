using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 200f;

    public void Rotate(int direction)
    {
        transform.Rotate(Vector3.forward * rotateSpeed * direction * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            collision.gameObject.SetActive(false);
        }
    }
}
