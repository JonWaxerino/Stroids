using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class BulletBehaviour : MonoBehaviour
{
    public float speed;
    public Vector2 normalizedDirection;

    private Rigidbody2D m_rigidbody;
    private BoxCollider2D m_collider;

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_rigidbody.linearVelocity = normalizedDirection * speed;

        m_collider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        float cameraMaxX = Camera.main.aspect * Camera.main.orthographicSize;
        float cameraMaxY = Camera.main.orthographicSize;

        if (transform.position.x > cameraMaxX + m_collider.bounds.extents.y
            || transform.position.x < -cameraMaxX - m_collider.bounds.extents.y
            || transform.position.y > cameraMaxY + m_collider.bounds.extents.y
            || transform.position.y < -cameraMaxY - m_collider.bounds.extents.y)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // stroid collision
    }
}
