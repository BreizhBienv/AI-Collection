using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MinerUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] public string _name;
    [SerializeField] public  GameObject _infos;
    [SerializeField] private TextMeshProUGUI _nameSection;
    [SerializeField] private TextMeshProUGUI _states;
    [SerializeField] private TextMeshProUGUI _values;

    private MinerAgent _miner;

    private Camera _mainCam;
    [SerializeField] private GameObject _floatingGO;
    [SerializeField] private TextMeshPro _floatingText;


    private void Start()
    {
        _miner = GetComponent<MinerAgent>();

        SetName();

        _mainCam = Camera.main;
    }

    private void Update()
    {
        UpdateInfos();
        UpdateNameRotation();
    }

    private void SetName()
    {
        _nameSection.text = _name;
        _floatingText.text = _name;
    }

    private void UpdateInfos()
    {
        Dictionary<EWorldState, bool> mergedWorldState = _miner.MergeWorldStates();

        string states = null, values = null;
        foreach (var ws in mergedWorldState)
        {
            states += ws.Key.ToString() + "\n";
            values += ws.Value.ToString() + "\n";
        }

        _states.text = states;
        _values.text = values;
    }

    private void UpdateNameRotation()
    {
        _floatingText.transform.rotation = 
            Quaternion.LookRotation(
                _floatingText.transform.position - _mainCam.transform.position);
    }
}
