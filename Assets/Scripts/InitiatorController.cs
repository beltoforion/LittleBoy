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

    public void FireNeutrons()
    {
        // activate neutrons
        ActivateNeutrons(true);

        // deactivate neutrons in 0.05 s
        Invoke("DeactivateNeutrons", 0.1f);
    }

    private void ActivateNeutrons(bool activate)
    {
        if (_neutronBeams == null || _neutronBeams.Count == 0)
            return;

        foreach(var cyl in _neutronBeams)
        {
            if (cyl == null)
                continue;

            var renderer = cyl.GetComponent<Renderer>();
            if (renderer == null)
                continue;

            renderer.enabled = activate;

            var collider = cyl.GetComponent<CapsuleCollider>();
            if (collider == null)
                continue;

            collider.enabled = activate;
        }
    }

    private void DeactivateNeutrons()
    {
        ActivateNeutrons(false);
    }


    void Start()
    {
        var go = GameObject.Find("GameManager");
        Assert.IsNotNull(go);

        _gameController = go.GetComponent("GameController") as GameController;
        Assert.IsNotNull(_gameController);

        _neutronBeams = new List<GameObject>();

        // Create neutron beams
        float deltaAlpha = 90 / (_neutronRings + 1);
        float deltaBeta = 360 / _neutronsPerRing;

        var neutronParent = new GameObject();
        neutronParent.name = "NeutronBeamParent";
        neutronParent.transform.parent = gameObject.transform;

        for (int n = 0; n < _neutronRings; ++n)
        {
            var alpha = 90 - (n + 1) * deltaAlpha;
            for (int i = 0; i < _neutronsPerRing; ++i)
            {
                var beta = i * deltaBeta;

                var neutronBeam = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                neutronBeam.name = "InitiatorNeutronBeam";
                neutronBeam.transform.localScale = new Vector3(_gameController._atomSize * 0.2f,
                                                            _gameController._neutronPathLength,
                                                            _gameController._atomSize * 0.2f);
                neutronBeam.transform.Rotate(new Vector3(beta, 90, alpha));
                neutronBeam.transform.position = transform.position + neutronBeam.transform.up * _gameController._neutronPathLength;
                neutronBeam.tag = "neutron";
                neutronBeam.transform.parent = neutronParent.transform;

                var renderer = neutronBeam.GetComponent<Renderer>();
                renderer.material = _matNeutron;
                renderer.enabled = false;

                var collider = neutronBeam.GetComponent<CapsuleCollider>();
                collider.isTrigger = true;
                collider.enabled = false;

                _neutronBeams.Add(neutronBeam);
            }
        }
    }
}
