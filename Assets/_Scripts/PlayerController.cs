using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(PolygonCollider2D))]
public class PlayerController : MonoBehaviour
{
    #region Components

    private Rigidbody2D m_rigidbody;
    private PolygonCollider2D m_collider;

    #endregion

    #region Movement

    [SerializeField] private float acceleration = 5;
    [SerializeField] private float maxSpeed = 5;
    [SerializeField] private float rotationSpeed = 5;

    private InputAction m_movementAction;

    private bool wasMovingLastFrame;

    #endregion

    #region Bullet Firing

    private InputAction m_shootAction;

    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private Transform bulletSpawn;

    #endregion

    public int lives = 5;

    void Awake()
    {
        #region Setting private vars

        m_rigidbody = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<PolygonCollider2D>();

        m_movementAction = InputSystem.actions.FindAction("Movement");
        m_shootAction = InputSystem.actions.FindAction("Shoot");
        #endregion
    }

    void Update()
    {
        #region Movement

        Vector2 movementVec = m_movementAction.ReadValue<Vector2>();

        float rotation = movementVec.x * rotationSpeed;
        m_rigidbody.angularVelocity = -rotation;

        if (movementVec.y != 0)
        {
            m_rigidbody.AddForce(transform.up * movementVec.y * acceleration * Time.deltaTime);
            m_rigidbody.linearVelocity = Vector2.ClampMagnitude(m_rigidbody.linearVelocity, maxSpeed);
        }

        #endregion

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
        else if (transform.position.x< -cameraMaxX - m_collider.bounds.extents.y && movingLeft)
        {
            transform.position = new Vector3(cameraMaxX + m_collider.bounds.extents.y, transform.position.y);
        }

        if (transform.position.y  > cameraMaxY + m_collider.bounds.extents.y && movingUp)
        {
            transform.position = new Vector3(transform.position.x , -cameraMaxY - m_collider.bounds.extents.y);
        }
        else if (transform.position.y < -cameraMaxY - m_collider.bounds.extents.y && movingDown)
        {
            transform.position = new Vector3(transform.position.x, cameraMaxY + m_collider.bounds.extents.y);
        }


        #endregion

        #region Bullet Firing

        if (m_shootAction.triggered)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, transform.rotation);
            BulletBehaviour bb = bullet.GetComponent<BulletBehaviour>();

            bb.speed = 10;
            bb.normalizedDirection = transform.up;
        }

        #endregion
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "stroid")
        {
            lives--;

            if (lives <= 0)
            {
                //game over
                Destroy(gameObject);
            }
        }
    }
}
