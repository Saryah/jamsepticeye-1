using UnityEngine;

public class DynamicElement : MonoBehaviour
{
    private int Id;
    public int ID => Id;

    bool assignedId;

    public bool IsAssigned => assignedId;

    public void AssignId(int ID)
    {
        Id = ID;
;        assignedId = true;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
