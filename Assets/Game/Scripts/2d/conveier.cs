using UnityEngine;

public class Conveyor : MonoBehaviour
{
    public float speed = 2f;
    public Vector2 direction = Vector2.right;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.rigidbody != null)
        {
            collision.rigidbody.linearVelocity = direction * speed;
        }
    }
}