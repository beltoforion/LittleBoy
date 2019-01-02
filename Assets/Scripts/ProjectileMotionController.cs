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
    }

    public void ResetMotion()
    {
        _stopMotion = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag != "projectile_aware")
            return;

        Debug.Log(string.Format("Stopping Projectile; Hit registered: {0} ({1})", other.name, other.tag));

        // Stop the projectile
        _rb.velocity = Vector3.zero;
        _stopMotion = true;

        // trigger the initiator
        var ic = other.GetComponent<InitiatorController>();
        if (ic != null)
            ic.FireNeutrons();

        _gameController.RequestSimulationEnd();
    }

    private bool _fire = false;

    public void Fire()
    {
        _fire = true;
    }

    private void FixedUpdate()
    {
        if (_gameController == null || _gameController.GameState != GameState.Running)
            return;

        if (!_stopMotion && _fire)
        {
            _rb.AddForce(new Vector3(0, 0, _gameController._forceProjectile), ForceMode.Impulse);
            _fire = false;
        }
    }
}
