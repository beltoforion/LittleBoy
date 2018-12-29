using UnityEngine;
using UnityEngine.Assertions;

public class CylinderSpawn : MonoBehaviour
{
    public GameController _gameController;

    /// <summary>
    /// Outter radius of the cylinder
    /// </summary>
    public float _outterRadius;

    /// <summary>
    /// Inner radius of the cyclinder.
    /// </summary>
    public float _innerRadius;

    /// <summary>
    /// Length of the clinder.
    /// </summary>
    public float _length;

    /// <summary>
    /// Atom to spawn
    /// </summary>
    public GameObject _spawnee;

    public GameObject _physicsParent;

    private Vector3 _startPosition;

    private int _numberOfAtoms = 0;

    public int NumberOfAtoms
    {
        get
        {
            return _numberOfAtoms;
        }
    }

    private static void SetGlobalScale(Transform transform, Vector3 globalScale)
    {
        transform.localScale = Vector3.one;
        transform.localScale = new Vector3(globalScale.x / transform.lossyScale.x, globalScale.y / transform.lossyScale.y, globalScale.z / transform.lossyScale.z);
    }

    /// <summary>
    /// Reset the cylinder to its start position and repopulate it
    /// </summary>
    public void ResetAtomsAndPosition()
    {
        if (_physicsParent == null)
            return;

        foreach(Transform child in _physicsParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        _numberOfAtoms = 0;
        Spawn();

        _physicsParent.transform.position = _startPosition;
    }

    private void Spawn()
    {
        Assert.IsNotNull(_physicsParent);

        var dist = _gameController._atomDist;
        var size = _gameController._atomSize;

        var v1 = Mathf.PI * Mathf.Pow(_innerRadius, 2) * _length;
        var v2 = Mathf.PI * Mathf.Pow(_outterRadius, 2) * _length;

        var volume = v2 - v1;

        Debug.Log(string.Format("Spawn Cylinder: vi={0}; vo={1}; v={2}", v1, v2, volume));

        for (float zp = -_length / 2; zp <= _length / 2; zp += dist)
        {
            for (float yp = -_outterRadius; yp <= _outterRadius; yp += dist)
            {
                for (float xp = -_outterRadius; xp <= _outterRadius; xp += dist)
                {
                    var r = Mathf.Sqrt(xp * xp + yp * yp);
                    if (r > _outterRadius)
                        continue;

                    if (r < _innerRadius)
                        continue;

                    ++_numberOfAtoms;

                    var atom = Instantiate(_spawnee, _physicsParent.transform);
                    atom.transform.Translate(new Vector3(xp, zp, yp)); // <- order mixed up because the cylinders are rotated!
                    SetGlobalScale(atom.transform, new Vector3(size, size, size));
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = _physicsParent.transform.position;
//        Spawn();
    }
}
