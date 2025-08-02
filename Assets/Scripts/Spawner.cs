using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private GameObject _startPoint;
    [SerializeField] private float _repeatRate = 1f;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 5;

    private ObjectPool<GameObject> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<GameObject>(
        // createFunc определяет что нужно сделать при создании нового объекта _pool
        createFunc: () => Instantiate(_prefab),
        // actionOnGet определяет действие при взятии свободного объекта из пула
        actionOnGet: (obj) => ActionOnGet(obj),
        // actionOnRelease определяет действие при возвращении объекта в пул
        actionOnRelease: (obj) => obj.SetActive(false),
        // actionOnDestroy определяет действие при удалении объекта из пула 
        actionOnDestroy: (obj) => Destroy(obj),
        // collectionCheck определяет надо ли проверять коллекцию при возвращении объекта в пул - работает только в редакторе
        collectionCheck: true,
        // defaultCapacity определяет размер пула по умолчанию
        defaultCapacity: _poolCapacity,
        // maxSize определяет максимальный размер пула
        maxSize: _poolMaxSize);
    }

    private void ActionOnGet(GameObject obj)
    {
        obj.transform.position = _startPoint.transform.position + new Vector3(0, 25, 0);
        obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        obj.SetActive(true);
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetSphere), 0f, _repeatRate);
    }

    private void GetSphere()
    {
        _pool.Get();
    }

    private void OnTriggerEnter(Collider other)
    {
        _pool.Release(other.gameObject);
    }
}