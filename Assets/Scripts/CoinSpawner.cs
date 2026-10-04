using UnityEngine;
using UnityEngine.EventSystems;

public class CoinSpawner : MonoBehaviour
{
    public static CoinSpawner Instance { get; private set; }
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject bumperPrefab;

    private void Awake() { Instance = this; }
    public void SpawnRandomCoinBonus() { Instantiate(coinPrefab, transform.position, Quaternion.identity); }
    public void SpawnBumper(Vector3 position) { 
        GameObject b = Instantiate(bumperPrefab, position, Quaternion.identity); 
        b.GetComponent<Bumper>().Initialize(15f); 
    }
    private CoinType nextCoinType;
    public CoinType NextCoinType => nextCoinType;
}