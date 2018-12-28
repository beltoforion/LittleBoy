using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts;


public class GameController : MonoBehaviour
{
    private GameState _gameState = GameState.Idle;

    public GameState GameState
    {
        get { return _gameState; }
        set { _gameState = value; }
    }

    public float _atomSize;

    public float _atomDist;

    public float _massRatio;

    public float _decayProbability = 0.001f;

    public float _neutronPathLength = 2;

    internal uint _totalAtomCount = 0;

    internal uint _atomCount = 0;

    internal uint _atomCollisionCount = 0;

    // Start is called before the first frame update
    void Start()
    {}

    public uint IncAtomCount()
    {
        _atomCount++;
        _totalAtomCount = Math.Max(_totalAtomCount, _atomCount);
        return _atomCount;
    }

    public uint DecAtomCount()
    {
        _atomCount--;
        return _atomCount;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(string.Format("Atoms alive={0}; Collisions={1}; dt={2}", _atomCount, _atomCollisionCount, Time.deltaTime));

        //if (_simulationIsOver)
        //{
        //    _simulationIsOver = false;
        //    Invoke("Restart", 1);
        //}
    }

    public void Restart()
    {
        _atomCount = 0;
        _totalAtomCount = 0;
        _atomCollisionCount = 0;
        _gameState = GameState.Running;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
