using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    public static GameManager Instance => instance;
    public Player player;

    Level curLevel;

    [SerializeField]List<Level> levels;
    int curLevelId;
    public Level CurLevel => curLevel;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

       
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<Player>();
        curLevelId = 0;
        EnterLevel(curLevelId);
    }

    public void EnterLevel(int id)
    {
        levels[id].EnterLevel(player);
        curLevel = levels[id];
    }

    public void ResetLevelToLastSavedState()
    {
        curLevel.ReloadPrevLevelState();
    }
    public void SaveLevelState()
    {
        curLevel.SaveLevel();
    }

    public void FinishLevel()
    {
        curLevelId++;
        EnterLevel(curLevelId);
    }
   
}
