using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class InitiatorController : MonoBehaviour
{
    public int _neutronRings = 3;

    public int _neutronsPerRing = 18;

    public Material _matNeutron;

    private GameController _gameController;

    private IList<GameObject> _neutronBeams;

    private bool _neutronsActive = true;

    private void ActivateNeutrons(bool activate)
    {
        foreach(var cyl in _neutronBeams)
        {
            var renderer = cyl.GetComponent<Renderer>();
            renderer.enabled = activate;

            var collider = cyl.GetComponent<CapsuleCollider>();
            collider.enabled = activate;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (_gameController == null || _gameController.GameState != Assets.Scripts.GameState.Running)
            return;

        Debug.Log(string.Format("Initiator triggered by {0}", other.name));

        if (other.name!="ProjectileCylinder")
            return;

        // activate neutrons
        ActivateNeutrons(true);

        // deactivate neutrons in 0.05 s
        Invoke("DeactivateNeutrons", 0.05f);
    }

    private void DeactivateNeutrons()
    {
        ActivateNeutrons(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        var go = GameObject.Find("GameManager");
        Assert.IsNotNull(go);

        _gameController = go.GetComponent("GameController") as GameController;
        Assert.IsNotNull(_gameController);

        // Create neutron beams
        float deltaAlpha = 90 / (_neutronRings + 1);
        float deltaBeta = 360 / _neutronsPerRing;

        for (int n = 0; n < _neutronRings; ++n)
        {
            var alpha = 90 - (n+1) * deltaAlpha;
            for (int i = 0; i < _neutronsPerRing; ++i)
            {
                var beta = i * deltaBeta;

                var cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                cylinder.name = "InitiatorNeutronBeam";
                cylinder.transform.localScale = new Vector3(_gameController._atomSize * 0.2f, 
                                                            _gameController._neutronPathLength, 
                                                            _gameController._atomSize * 0.2f);
                cylinder.transform.Rotate(new Vector3(beta, 90, alpha));
                cylinder.transform.position = transform.position + cylinder.transform.up * _gameController._neutronPathLength;
                cylinder.tag = "neutron";
                cylinder.transform.parent = gameObject.transform;

                var renderer = cylinder.GetComponent<Renderer>();
                renderer.material = _matNeutron;
                renderer.enabled = false;

                var collider = cylinder.GetComponent<CapsuleCollider>();
                //collider.isTrigger = true;
                collider.enabled = false;

//                _neutronBeams.Add(cylinder);
            }
        }
    }
}
