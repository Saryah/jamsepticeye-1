using UnityEngine;

public static class Utility
{
    public static Vector2 GetCenter(BoxCollider2D collider)
    {
        if (collider == null) return Vector2.zero;

        // local center is collider.offset
        // world center = transform position + rotated offset
        return (Vector2)collider.transform.position + collider.offset;
    }
}
