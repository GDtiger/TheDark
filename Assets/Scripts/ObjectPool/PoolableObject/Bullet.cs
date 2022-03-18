using UnityEngine;
public class Bullet : PoolableObject
{
    public float speed = 5f;

    void Update()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime);
    }
}