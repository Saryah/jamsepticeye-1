using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine.UIElements;

public class Level : MonoBehaviour
{
    [SerializeField]Collider2D levelBounds;
    LevelState curState;
    [SerializeField]Transform startPosition;
    [SerializeField] int soulAmount;
    Player player;
    Vector2 center;
    Camera cam;
    public float cameraSize;
    void Start()
    {
        levelBounds = GetComponent<Collider2D>();
        center = Utility.GetCenter(levelBounds as BoxCollider2D);
        cam = Camera.main;
    }
    public void SaveLevel()
    {
        Debug.Log("saving Level");
        LevelState prevState = curState; // gets the current level state
        
        curState = new LevelState(); // removes the current öevel state
        curState.previousState = prevState; // 
        if(curState.previousState == null)
        {
            curState.previousState = new LevelState();
        }

        curState.previousState.Save(FilteredElems(),player);

        
        //check if there is a new dynamic element, if so assigna a new ID

        // Go through all registered dynamic elements and save their positions in some struct
        // save the current state as the previous state of the level.
        // reloading will now put you in the newly saved, previous sate
    }

    public void EnterLevel(Player player)
    {
        player.GiveSouls(soulAmount);
        player.transform.position = startPosition.position;
        this.player = player;
        cam.transform.position = new Vector3(center.x,center.y,cam.transform.position.z);
        cam.orthographicSize = cameraSize;

        //SaveLevel();
    }

    private List<DynamicElement> FilteredElems()
    {

        List<DynamicElement> result = new();
        List<Collider2D> unfilteredList = new();

        Physics2D.OverlapCollider(levelBounds, unfilteredList);
        
        if(unfilteredList == null)
        {
            return new List<DynamicElement>();
        }
        foreach (Collider2D elem in unfilteredList)
        {
            DynamicElement d = elem.GetComponent<DynamicElement>();
            if (d != null)
            {
              result.Add(d);  
            }
        }

        return result;
    }

    public Player.PlayerTurnData GetPrevTurnData()
    {
        return curState.previousState.playerTurnData;
    }

    public void ReloadPrevLevelState()
    {
        //check for the previous state of the "LEvelState" class, if its null, then you are at the first saved state
        if(curState == null )
        {
            return;
        }
        if (curState != null && curState.previousState == null)
        {
            return;
        }
        curState.previousState.Load(FilteredElems());
        curState = curState.previousState;
    }

    public void ReloadLevel()
    {
        //clear Everything
    }

    public void UnloadLevel()
    {
        //delete everything spawned within the level
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

public class LevelState
{
    public LevelState previousState;
    Dictionary<int, DynamicElemInfo> assignedElemInfo = new();
    List<DynamicElement> levelStateDynamicElements = new();
    public Player.PlayerTurnData playerTurnData;
    Player player;
    public void Save(List<DynamicElement> elements,Player player)
    {
        this.player = player;
        assignedElemInfo.Clear();
        int count = elements.Count;
        Debug.Log("count" + count);
        levelStateDynamicElements = elements;
        playerTurnData.UpdateData(player.transform.position, player.playerRb.linearVelocity, player.CurSoulAmnt);

        // go through each element and check if they have an Assigned ID
        foreach (DynamicElement element in levelStateDynamicElements)
        {
            //first check if the object has been assigned an id yet
            if(!element.IsAssigned)
            {
                element.AssignId(count);
                count++;
            }
            
            // then add the objectinfo to the "Assigned ElemInfo"
            DynamicElemInfo elemInfo;
            elemInfo.position = element.transform.position;
            Debug.Log("elemId" + element.ID);
            assignedElemInfo.Add(element.ID, elemInfo);
            
        }
        Debug.Log("dynamic elems in scene" + elements.Count);
    }

    public void Load(List<DynamicElement> elements)
    {
        player.GiveSouls(playerTurnData.SoulAmount);
        player.playerRb.linearVelocity = playerTurnData.Velocity;
        player.transform.position = playerTurnData.Position;
        List<DynamicElement> filteredObjects = new ();
        foreach (DynamicElement element in elements) 
        { 
            //Update the positions of the, then existing, elements
            if (assignedElemInfo.ContainsKey(element.ID))
            {
                element.transform.position = assignedElemInfo[element.ID].position;
            }
            else
            {
                filteredObjects.Add(element);
            }
            
        }

        foreach (DynamicElement element in filteredObjects)
        {
            element.Destroy();
        }     
    }

    public void DeleteAllStates()
    {
        //recursively delete alll the previous level states
    }
    //needs a list of registered Elements with their appropriate IDS.

}

public struct DynamicElemInfo
{
    public Vector2 position;
    //if there is anything just add in here
}

