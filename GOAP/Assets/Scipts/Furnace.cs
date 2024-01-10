using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furnace : MonoBehaviour
{
    [Header("Visual")]
    public Transform _craftingVisual;
    public Transform _ironVisual;
    public Transform _progressBarRoot;
    public Transform _progressBar;

    [Header("Furnace Params")]
    public int _oreCost = 2;
    public int _maxQueued = 3;
    [SerializeField] private float _craftTime = 5;

    private int     _craftQueue = 0;
    private int     _barCrafted = 0;
    private float   _craftTimer = 0.0f;
    private bool    _isCrafting = false;

    private void Start()
    {
        World.Instance.RegisterFurnace(this);
        _oreCost = Utils.oreNeededToCraft;
    }

    private void OnDestroy()
    {
        World.Instance.UnregisterFurnace(this);
    }

    public bool CanCraft(int pOreAmount = 2)
    {
        return pOreAmount >= _oreCost && _craftQueue < _maxQueued;
    }

    public int TryCraft(int pOreAmount)
    {
        if (!CanCraft(pOreAmount))
        {
            World.Instance.UpdateWorldState_AvailableFurnace();
            return pOreAmount;
        }

        if (_isCrafting)
        {
            _craftQueue++;

            return pOreAmount - _oreCost;
        }

        StartCoroutine(Craft());
        return pOreAmount - _oreCost;
    }

    IEnumerator Craft()
    {
        _craftingVisual.gameObject.SetActive(true);
        _progressBarRoot.gameObject.SetActive(true);

        _isCrafting = true;
        _craftTimer = 0;

        while (_isCrafting)
        {
            Vector3 progressScale = _progressBar.localScale;
            progressScale.z = _craftTimer / _craftTime;
            _progressBar.localScale = progressScale;

            if (_craftTimer < _craftTime)
            {
                _craftTimer += Time.deltaTime;
                yield return null;
                continue;
            }

            _barCrafted++;
            _ironVisual.gameObject.SetActive(true);
            _craftTimer = 0;

            if (_craftQueue > 0)
            {
                _craftQueue--;
            }
            else
            {
                _isCrafting = false;
                _craftingVisual.gameObject.SetActive(false);
                _progressBarRoot.gameObject.SetActive(false);
            }

            World.Instance.UpdateWorldState_AvailableFurnace();
            World.Instance.UpdateWorldState_AvailableIngot();

            yield return null;
        }
    }

    public bool CanPickUp()
    {
        return _barCrafted > 0;
    }

    public bool TryPickUp()
    {
        if (CanPickUp())
        {
            _barCrafted--;

            if (_barCrafted == 0)
                _ironVisual.gameObject.SetActive(false);

            World.Instance.UpdateWorldState_AvailableIngot();
            return true;
        }
        return false;
    }
}
