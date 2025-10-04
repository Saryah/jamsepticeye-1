using UnityEngine;

public class LevelFinishZone : MonoBehaviour,InteractsWithPlayer
{
    bool playerInteracting;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void OnPlayerEnter(Player p)
    {
        Debug.Log("playerInteracting");
        playerInteracting = true;
    }

    public void OnSoulEnter(Soul s)
    {

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Player p = col.gameObject.GetComponent<Player>();
        Debug.Log("object entered");
        if (p != null)
        {
            OnPlayerEnter(p);
        }
    }

    private void OnTriggerExit2D (Collider2D col)
    {
        Player p = col.gameObject.GetComponent<Player>();
        if (p != null)
        {
            playerInteracting = false;
        }
    }

    void Update()
    {
        if(playerInteracting && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("load new Level");
            GameManager.Instance.FinishLevel();
        }
    }
}
