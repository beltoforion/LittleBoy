using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private static void SetGlobalScale(Transform transform, Vector3 globalScale)
    {
        transform.localScale = Vector3.one;
        transform.localScale = new Vector3(globalScale.x / transform.lossyScale.x, globalScale.y / transform.lossyScale.y, globalScale.z / transform.lossyScale.z);
    }

    // Start is called before the first frame update
    void Start()
    {
        var dist = _gameController._atomDist;
        var size = _gameController._atomSize;

        var v1 = Mathf.PI * Mathf.Pow(_innerRadius, 2) * _length;
        var v2 = Mathf.PI * Mathf.Pow(_outterRadius, 2) * _length;

        var volume = v2 - v1;

        Debug.Log(string.Format("Spawn Cylinder: vi={0}; vo={1}; v={2}", v1, v2, volume));

        for (float zp = -_length/2; zp <= _length/2; zp += dist)
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

                    var atom = Instantiate(_spawnee, transform);
                    atom.transform.Translate(new Vector3(xp, yp, zp));
                    SetGlobalScale(atom.transform, new Vector3(size, size, size));
                    atom.transform.parent = _physicsParent.transform;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
