using UnityEngine;
using UnityEngine.Assertions;
using System;

public class Spawn : MonoBehaviour
{

    public Transform _spawnPos;

    public GameObject _spawnee;

    public float _spawnRadius;

    public float _deltaRad = 0.5f;

    public float _atomRadius = 0.02f;

    private GameController _setupScript;

    // Use this for initialization
    void Start()
    {
        _setupScript = GetComponentInParent<GameController>();
        Assert.IsNotNull(_setupScript, "Setup Script not found on parent!");

        _spawnRadius = gameObject.transform.localScale.x / 2;
        _spawnPos = transform;

        int ct = 0;
        for (float r = _deltaRad; r <= _spawnRadius; r += _deltaRad)
        {
            int num = 4 * (int)(4 * (float)Math.PI * r * r);
            FibonacciSphere(num, r);
            ct += num;
        }

        Debug.Log(string.Format("Anzahl: {0}", ct));
    }

    private void FibonacciSphere(int points, float scale, bool randomize = true)
    {
        float offset = 2f / points;
        float increment = (float)Math.PI * (3 - (float)Math.Sqrt(5f));
        float rnd = 1;

        if (randomize)
        {
            var r = new System.Random();
            rnd = (float)r.NextDouble() * points;
        }

        for (int i = 0; i < points; ++i)
        {
            float y = ((i * offset) - 1) + (offset / 2);
            float r = (float)Math.Sqrt(1 - Math.Pow(y, 2));

            float phi = ((i + rnd) % points) * increment;

            float x = (float)Math.Cos(phi) * r;
            float z = (float)Math.Sin(phi) * r;

            var atom = Instantiate(_spawnee, transform);
            atom.transform.Translate(new Vector3(x, y, z) * scale);
            atom.transform.localScale = new Vector3(_atomRadius, _atomRadius, _atomRadius);
        }
    }

    // Update is called once per frame
    void Update()
    {}
}

