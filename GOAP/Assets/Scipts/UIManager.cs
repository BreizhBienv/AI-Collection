using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Dropdown _dropdown;
    private int _prevDropdownValue;


    private MinerUI[] _minerInfos;

    // Start is called before the first frame update
    void Start()
    {
        _minerInfos = FindObjectsOfType<MinerUI>();

        foreach (MinerUI miner in _minerInfos)
            _dropdown.options.Add(new TMP_Dropdown.OptionData(miner._name));

        _prevDropdownValue = _dropdown.value;

        _dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(_dropdown);
        });
    }

    void DropdownValueChanged(TMP_Dropdown change)
    {
        if (_prevDropdownValue > 0)
            _minerInfos[_prevDropdownValue - 1]._infos.SetActive(false);

        if (change.value > 0)
            _minerInfos[change.value - 1]._infos.SetActive(true);

        _prevDropdownValue = change.value;
    }
}
