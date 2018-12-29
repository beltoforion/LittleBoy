using Assets.Scripts;
using UnityEngine;


public class ProjectileMotionController : MonoBehaviour
{
    private Rigidbody _rb;

    public GameController _gameController;

    bool _stopMotion = false;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        tag = "projectile";
    }

    public void ResetMotion()
    {
        _stopMotion = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag != "initiator")
            return;

        Debug.Log(string.Format("Stopping Projectile; Hit registered: {0} ({1})", other.name, other.tag));

        _rb.velocity = Vector3.zero;
        _stopMotion = true;
        _gameController.RequestSimulationEnd();
    }

    private void FixedUpdate()
    {
        if (_gameController == null || _gameController.GameState != GameState.Running)
            return;

        if (!_stopMotion)
        {
            _rb.AddForce(new Vector3(0, 0, _gameController._forceProjectile));
        }
    }
}
