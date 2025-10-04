using UnityEngine;



public interface InteractsWithPlayer
{
    public abstract void OnPlayerEnter(Player player);

    public void OnSoulEnter(Soul soul);
}
public class Corpse : DynamicElement,InteractsWithPlayer
{
    [SerializeField] float bounciness;
    private enum Orientation {Horizontal, Vertical };
    private Orientation orientation;
    Vector2 prevFrameVel;

    
   public void OnSoulEnter(Soul s)
    {
        s.Bounce();
    }
    public void OnPlayerEnter(Player player)
    {
        Rigidbody2D playerRb = player.playerRb;
        if(orientation == Orientation.Horizontal)
        {
            if (player.transform.position.y > transform.position.y)
            {
                Debug.Log("bounce" + playerRb.linearVelocity.y );
                //playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, -playerRb.linearVelocity.y * bounciness);
                Debug.Log("bounce" + playerRb.linearVelocity.y);
            }
        }
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        orientation = Orientation.Horizontal;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Player p = col.gameObject.GetComponent<Player>();
        if(p!= null)
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
