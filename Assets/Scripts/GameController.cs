using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private GameState _gameState = GameState.Initialize;

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

    public float _forceProjectile = -0.5f;

    internal int _totalAtomCount = 0;

    internal int _atomCount = 0;

    internal uint _atomCollisionCount = 0;

    internal float _deacayedPercentage;

    /// <summary>
    /// Reference to the gameobject representing the UI
    /// </summary>
    public GameObject _ui;

    private Text _textResult;

    private Text _textStat;

    private bool _requestSimulationEnd = false;

    private float _timeEndRequested = 0;

    CylinderSpawn _scriptProjectile;

    CylinderSpawn _scriptTarget;

    ProjectileMotionController _scriptProjectileMotion;


    // Start is called before the first frame update
    void Start()
    {
        _textResult = GameObject.Find("EvaluationText").GetComponent<Text>();
        _textResult.text = "";
        _textStat = GameObject.Find("AtomCounterText").GetComponent<Text>();
        _textStat.text = "";

        _scriptProjectile = GameObject.Find("Projectile").GetComponent<CylinderSpawn>();
        _scriptTarget = GameObject.Find("Target").GetComponent<CylinderSpawn>();
        _scriptProjectileMotion = GameObject.Find("ProjectileCylinder").GetComponent<ProjectileMotionController>();
    }

    public int AtomDecayed()
    {
        _atomCount--;

        int decayedAtomCount = _totalAtomCount - _atomCount;
        _deacayedPercentage = (float)decayedAtomCount / _totalAtomCount * 100;

        return _atomCount;
    }

    public void RequestSimulationEnd()
    {
        if (_gameState == GameState.Running)
        {
            _requestSimulationEnd = true;
            _timeEndRequested = Time.time;
        }
        else
        {
            Debug.Log("Stop Requested but simulation is not running!");
        }
    }

    private void ShowScore()
    {
        _textResult.enabled = true;

        if (_deacayedPercentage < 30)
        {
            // total failure
            _textResult.text = "Bomb failed to detonate!";
        }
        else if (_deacayedPercentage < 60)
        {
            // bomb fizzled out
            _textResult.text = "Bomb fizzled out!";
        }
        else 
        {
            // Chain Reaction!
            _textResult.text = "Uncontrolled Chain Reaction!";
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (_gameState)
        {
            case GameState.Initialize:
                _scriptProjectile.ResetAtomsAndPosition();
                _scriptTarget.ResetAtomsAndPosition();
                _scriptProjectileMotion.ResetMotion();

                _totalAtomCount = _scriptTarget.NumberOfAtoms + _scriptProjectile.NumberOfAtoms;
                _atomCount = _totalAtomCount;
                _atomCollisionCount = 0;
                _deacayedPercentage = 0;

                _textResult.text = "";
                _textStat.text = "";
                _gameState = GameState.WaitForStart;
                break;

            case GameState.WaitForStart:
               break;

            case GameState.Running:
                _textResult.enabled = false;
                _textStat.enabled = true;

                int decayedAtomCount = _totalAtomCount - _atomCount;
                _textStat.text = string.Format("Decayed atoms: {0,4:#####} of {1,4:#####} ({2:##} %)", decayedAtomCount, _totalAtomCount, _deacayedPercentage);

                if (_requestSimulationEnd && (Time.time - _timeEndRequested) > 0.5)
                {
                    _requestSimulationEnd = false;
                    ShowScore();
                    _gameState = GameState.ShowScore;
                }

                Debug.Log(string.Format("Atoms alive={0}; Collisions={1}; dt={2}", _atomCount, _atomCollisionCount, Time.deltaTime));
                break;

            case GameState.ShowScore:
                break;
        }
    }

    public void Reset()
    {
        _gameState = GameState.Initialize;
    }

    public void Fire()
    {
        _gameState = GameState.Running;
    }
}
