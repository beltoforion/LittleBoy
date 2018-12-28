using Assets.Scripts;
using UnityEngine;


public class ProjectileMotionController : MonoBehaviour
{
    public float _force;

    private Rigidbody _rb;

    public GameController _gameController;


    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        tag = "projectile";
    }

    public void Reset()
    {
        _force = 0;
        _rb.velocity = Vector3.zero;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag != "initiator")
            return;

        Reset();
    }

    private void FixedUpdate()
    {
        //if (_gameController.GameState == GameState.Idle)
        //    return;

        _rb.AddForce(new Vector3(0, 0, _force));
    }
}
