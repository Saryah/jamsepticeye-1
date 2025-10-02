using System;
using UnityEngine;

public class Soul : MonoBehaviour
{
    protected Rigidbody2D rb;
    Transform teleportObject;
    public SoulType soulType;
    public Action onTeleport;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Throw(Vector2 force, Transform teleportObject, Action onTeleport = null)
    {
        rb.linearVelocity = force;
        this.teleportObject = teleportObject;
        this.onTeleport += onTeleport;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object's layer is included in the LayerMask
        if (((1 << other.gameObject.layer) & soulType.Hittable) != 0)
        {
            soulType.ActivateSpecialEffect();
            Teleport();
            Destroy(this.gameObject);
        }
        else
        {
           // some other thing happens

        }
    }

    private void Teleport()
    {
        //some fancy animation shit potentially
        teleportObject.transform.position = transform.position;
        onTeleport?.Invoke();
    }
}
