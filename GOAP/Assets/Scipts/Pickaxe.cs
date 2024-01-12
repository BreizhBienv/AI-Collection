using UnityEngine;

public class Pickaxe : MonoBehaviour
{
    Rigidbody _rb;
    BoxCollider _boxCollider;

    [SerializeField] private Vector3 _impulse;
    [SerializeField] private float _impulseMagnitude;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<BoxCollider>();

        OnAvailable();

        _impulse *= _impulseMagnitude;
    }

    public void OnAvailable()
    {
        World.Instance.RegisterPickaxe(this);

        _rb.isKinematic = false;
        _rb.detectCollisions = true;

        _boxCollider.enabled = true;
    }

    public void OnUnavailable()
    {
        World.Instance.UnregisterPickaxe(this);

        _rb.isKinematic = true;
        _rb.detectCollisions = false;

        _boxCollider.enabled = false;
    }

    public void Impulse()
    {
        _rb.AddForce(_impulse, ForceMode.Impulse);
    }
}
