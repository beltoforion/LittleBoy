using UnityEngine;
using UnityEngine.UI;

public class StatDisplay : MonoBehaviour
{
    public GameController _gameController;

    public Text _text;

    // Update is called once per frame
    void Update()
    {
        uint decayedAtomCount = _gameController._totalAtomCount - _gameController._atomCount;
        float perc = (float)decayedAtomCount / _gameController._totalAtomCount * 100;
        _text.text = string.Format("Decayed atoms: {0,4:#####} of {1,4:#####} ({2:##} %)", decayedAtomCount, _gameController._totalAtomCount, perc);
    }
}
