using System;

using UnityEngine;
using UnityEngine.Assertions;


using Random = UnityEngine.Random;



public class AtomController : MonoBehaviour
{

    enum AtomState
    {
        /// <summary>
        /// Atom is created but does not decay
        /// </summary>
        Inactive,

        /// <summary>
        /// Atom is subject to decay
        /// </summary>
        Alive,

        /// <summary>
        /// Atom will decay in the next timestep
        /// </summary>
        AboutToDecay,

        Decaying,

        /// <summary>
        /// Atom is decayed
        /// </summary>
        Decayed
    };

    ///// <summary>
    /////     Probability of a decay per second
    ///// </summary>
    private float _decayProbability;

    /// <summary>
    /// Length of a neutron Path.
    /// </summary>
    private float _neutronPathLength;

    private float _atomSize;

    private AtomState _state = AtomState.Alive;

    private long _id = 0;

    private float _elapsedTime = 0;

    private GameObject[] _neutron = { null, null };

    private GameController _setupScript;

    private static void SetGlobalScale(Transform transform, Vector3 globalScale)
    {
        transform.localScale = Vector3.one;
        transform.localScale = new Vector3(globalScale.x / transform.lossyScale.x, globalScale.y / transform.lossyScale.y, globalScale.z / transform.lossyScale.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag != "neutron")
            return;

        //        Debug.Log(string.Format("Collosion with {0}", collision.gameObject.name));
        if (_state == AtomState.Alive)
        {
            _state = AtomState.AboutToDecay;
            _setupScript._atomCollisionCount++;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag != "neutron")
            return;

        if (_state == AtomState.Alive)
        {
            _state = AtomState.AboutToDecay;
            _setupScript._atomCollisionCount++;
        }
    }

    private void Setup()
    {

    }

    void Start()
    {
        _setupScript = GetComponentInParent<GameController>();
        Assert.IsNotNull(_setupScript, "Setup Script not found on parent!");

        _decayProbability = _setupScript._decayProbability;
        _neutronPathLength = _setupScript._neutronPathLength;

        _id = _setupScript.IncAtomCount();

        const float deg2Rad = Mathf.PI / 180;

        // Scale the atom according to the scale property
        _atomSize = gameObject.transform.localScale.x;

        // Initialize directions of the two emitted neutrons
        for (int i = 0; i < _neutron.Length; ++i)
        {
            _neutron[i] = this.gameObject.transform.GetChild(i).gameObject;
            var trans = _neutron[i].transform;
            SetGlobalScale(trans, new Vector3(_atomSize * 0.2f, _neutronPathLength, _atomSize * 0.2f));

            float rotx = Random.value * 360;
            float roty = 0;
            float rotz = Random.value * 180 - 90;

            // Standardrichtungsvektor des Zylinders            
            var angles = new Vector3(rotx, roty, rotz);
            var dir = Quaternion.Euler(angles) * new Vector3(0, _neutronPathLength, 0);

            //            Debug.Log(string.Format("angles: {0}, {1}, {2}; dir: {3}, {4}, {5}", angles.x, angles.y, angles.z, dir.x,dir.y,dir.z));
            trans.eulerAngles = angles;
            trans.position += dir;
        }
    }

    private void EnableNeutrons(bool enable)
    {
        // disable atom renderer
        var rend = GetComponent<MeshRenderer>();
        rend.enabled = false;

        // activate neutron renderer
        for (int i = 0; i < _neutron.Length; ++i)
        {
            rend = _neutron[i].GetComponent<MeshRenderer>();
            rend.enabled = enable;

            var collider = _neutron[i].GetComponent<CapsuleCollider>();
            collider.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _elapsedTime += Time.deltaTime;

        // Probability of a decay in the last frame cycle
        var prob = Mathf.Pow((1 - _decayProbability), Time.deltaTime);

        switch (_state)
        {
            case AtomState.Alive:
                if (Random.value > prob)
                    _state = AtomState.AboutToDecay;
                break;

            case AtomState.AboutToDecay:
                EnableNeutrons(true);
                _state = AtomState.Decaying;
                break;

            case AtomState.Decaying:
                Destroy(this.gameObject);
                _setupScript.DecAtomCount();
                _state = AtomState.Decayed;
                break;

            case AtomState.Decayed:
            default:
                // Nothing to do here, keep state
                break;
        }
    }
}
