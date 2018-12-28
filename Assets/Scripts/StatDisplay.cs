using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class StatDisplay : MonoBehaviour
{
    private GameController _setupScript;

    public Text _text;

    void Start()
    {
        _setupScript = GetComponentInParent<GameController>();
        Assert.IsNotNull(_setupScript, "Setup Script not found on parent!");
    }

    // Update is called once per frame
    void Update()
    {
        uint decayedAtomCount = _setupScript._totalAtomCount - _setupScript._atomCount;
        float perc = (float)decayedAtomCount / _setupScript._totalAtomCount * 100;
        _text.text = string.Format("Decayed atoms: {0,4:#####} of {1,4:#####} ({2:##} %)", decayedAtomCount, _setupScript._totalAtomCount, perc);
    }
}
