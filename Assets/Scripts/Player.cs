using NUnit.Framework;
using UnityEngine;

public class Player : MonoBehaviour
{
    public struct PlayerTurnData
    {
        Vector2 position;
        Vector2 velocity;
        int soulAmount;
        public Vector2 Position => position;
        public Vector2 Velocity => velocity;

        public int SoulAmount => soulAmount;

        public void UpdateData(Vector2 position, Vector2 vel,int soulAmount)
        {
            velocity = vel;
            this.position = position;
            this.soulAmount = soulAmount;
        }

        
    }
    Rigidbody2D rb;
    public Rigidbody2D playerRb => rb;
    [SerializeField] float maxStrength;
    [SerializeField] GameObject throwObject;
    [SerializeField] GameObject corpse;
    [SerializeField] Transform throwPoint;
    [SerializeField] float dragMultiplier;
    Vector2 throwDirection;
    Rigidbody2D soulRb;
    [SerializeField]int soulAmount;
    public int CurSoulAmnt => soulAmount;

    LineRenderer lineRenderer;
    Camera cam;
    Soul thrownSoul;

    

    Vector2 initPosition;
    bool windingThrow;
    void OnEnable()
    {
       cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void GiveSouls(int soulAmount)
    {
        this.soulAmount = soulAmount;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && thrownSoul == null && soulAmount > 0)
        {
            InitiateCharge(cam.ScreenToWorldPoint(Input.mousePosition));

        }

        if(windingThrow)
        {
            WindThrow();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            GameManager.Instance.CurLevel.ReloadPrevLevelState();
        }
    }

    public void Respawn()
    {
       
    }

    public Vector2[] Plot(Rigidbody2D rb,Vector2 pos,Vector2 vel,int steps)
    {
        Vector2[] results = new Vector2[steps];

        float timestep = Time.fixedDeltaTime / Physics2D.velocityIterations;
        Vector2 gravityAccel = Physics2D.gravity * rb.gravityScale * timestep * timestep;

        float drag = 1f - timestep * rb.linearDamping;
        Vector2 moveStep = vel * timestep;

        for (int i = 0; i < steps; i++)
        {
            moveStep += gravityAccel;
            moveStep *= drag;
            pos += moveStep;
            results[i] = pos;
        }
        return results;
    }


    void WindThrow()
    {
        //visually show the person winding the throw
        Vector2 curPos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 force = ((initPosition - curPos).normalized) * Mathf.Min(Mathf.Abs(Vector2.Distance(initPosition, curPos)) * dragMultiplier, maxStrength);
        soulRb.position = throwPoint.position;

        Vector2[] trajectory = Plot(soulRb, throwPoint.position, force, 800);
        lineRenderer.positionCount = trajectory.Length;
        Vector3[] positions = new Vector3[trajectory.Length];
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = trajectory[i];
        }

        lineRenderer.SetPositions(positions);


        if (Input.GetMouseButtonUp(0))
        {
            soulRb.position = throwPoint.position;
            soulRb.bodyType = RigidbodyType2D.Dynamic;
            curPos = cam.ScreenToWorldPoint(Input.mousePosition);
            force = ((initPosition - curPos).normalized) * Mathf.Min(Mathf.Abs(Vector2.Distance(initPosition, curPos))*dragMultiplier, maxStrength);
            lineRenderer.enabled = false;
            
            Throw(force);
        }
    }

    void Throw(Vector2 force)
    {
        Debug.Log("force" + force);

        //prevTurnData.UpdateData(transform.position, rb.linearVelocity,soulAmount);
        GameManager.Instance.SaveLevelState();
        soulAmount--;
        thrownSoul.Throw(force, this.gameObject.transform,OnTeleport);
       

        windingThrow = false;
    }

    public void UndoLastMove()
    {

    }
    private void OnTeleport()
    {
        //set the previous pos to be whatever it has to be
        thrownSoul = null;
        Instantiate(corpse, GameManager.Instance.CurLevel.GetPrevTurnData().Position, Quaternion.identity);

    }
    void InitiateCharge(Vector2 initPos)
    {
        initPosition = initPos;
        windingThrow = true;
        lineRenderer.enabled = true;
        GameObject copy = Instantiate(throwObject, throwPoint.transform.position, Quaternion.identity);
        thrownSoul = copy.GetComponent<Soul>();
        soulRb = copy.GetComponent<Rigidbody2D>();
        soulRb.bodyType = RigidbodyType2D.Static;
    }
}


