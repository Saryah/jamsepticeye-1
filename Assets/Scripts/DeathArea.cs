using UnityEngine;

public class DeathArea : MonoBehaviour,InteractsWithPlayer
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlayerEnter(Player player)
    {
        player.Respawn();
    }

    public void OnSoulEnter(Soul s)
    {
        s.DestroySoul();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Player p = col.gameObject.GetComponent<Player>();
        if (p != null)
        {
            OnPlayerEnter(p);
        }
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        Soul s = col.gameObject.GetComponent<Soul>();
        if (s != null)
        {
            Debug.Log("soul Entered");
            OnSoulEnter(s);
        }
    }
}
