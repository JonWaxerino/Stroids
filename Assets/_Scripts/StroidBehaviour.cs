using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(PolygonCollider2D))]
public class StroidBehaviour : MonoBehaviour
{
    public float speed;

    private Rigidbody2D m_rigidbody;
    private PolygonCollider2D m_collider;

    public StroidType type;

    [SerializeField] private GameObject MediumStroid;
    [SerializeField] private GameObject SmallStroid;

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<PolygonCollider2D>();

        switch (type)
        {
            case StroidType.Large:
                m_rigidbody.angularVelocity = 45;
                speed = 2;
                break;
            case StroidType.Medium:
                m_rigidbody.angularVelocity = 90;
                speed = 4;
                break;
            case StroidType.Small:
                m_rigidbody.angularVelocity = 180;
                speed = 6;
                break;
        }


        m_rigidbody.linearVelocity = transform.up * speed;
    }

    void Update()
    {
        #region Screen Wrapping

        bool movingUp, movingDown, movingLeft, movingRight;
        movingUp = Mathf.Sign(m_rigidbody.linearVelocity.y) > 0;
        movingDown = Mathf.Sign(m_rigidbody.linearVelocity.y) < 0;
        movingRight = Mathf.Sign(m_rigidbody.linearVelocity.x) > 0;
        movingLeft = Mathf.Sign(m_rigidbody.linearVelocity.x) < 0;

        float cameraMaxX = Camera.main.aspect * Camera.main.orthographicSize;
        float cameraMaxY = Camera.main.orthographicSize;

        if (transform.position.x > cameraMaxX + m_collider.bounds.extents.y && movingRight)
        {
            transform.position = new Vector3(-cameraMaxX - m_collider.bounds.extents.y, transform.position.y);
        }
        else if (transform.position.x < -cameraMaxX - m_collider.bounds.extents.y && movingLeft)
        {
            transform.position = new Vector3(cameraMaxX + m_collider.bounds.extents.y, transform.position.y);
        }

        if (transform.position.y > cameraMaxY + m_collider.bounds.extents.y && movingUp)
        {
            transform.position = new Vector3(transform.position.x, -cameraMaxY - m_collider.bounds.extents.y);
        }
        else if (transform.position.y < -cameraMaxY - m_collider.bounds.extents.y && movingDown)
        {
            transform.position = new Vector3(transform.position.x, cameraMaxY + m_collider.bounds.extents.y);
        }


        #endregion
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "bullet")
        {
            switch (type)
            {
                case StroidType.Large:
                    for (int i = 0; i < 3; i++)
                    {
                        GameObject newStroid = Instantiate(MediumStroid);
                        newStroid.transform.position = transform.position;
                        newStroid.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f));

                        StroidBehaviour behaviour = newStroid.GetComponent<StroidBehaviour>();
                        behaviour.type = StroidType.Medium;

                        UIManager.AddPoints(100);
                    }
                    
                    break;
                case StroidType.Medium:
                    for (int i = 0; i < 2; i++)
                    {
                        GameObject newStroid = Instantiate(SmallStroid);
                        newStroid.transform.position = transform.position;
                        newStroid.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f));

                        StroidBehaviour behaviour = newStroid.GetComponent<StroidBehaviour>();
                        behaviour.type = StroidType.Small;

                        UIManager.AddPoints(50);
                    }
                    break;
                case StroidType.Small:
                    UIManager.AddPoints(25);
                    break;
            }
            Destroy(collider.gameObject);
            Destroy(gameObject);
        }
    }
}

public enum StroidType
{
    Large,
    Medium,
    Small
}
