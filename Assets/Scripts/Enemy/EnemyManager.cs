using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Gameobject tag Enemy")]
    [SerializeField] private GameObject[] _musuh;
    public bool _winorlose;
    
    [Header("GameManager")]
    [Tooltip("Masukan Gameobject GameManager")]
    [SerializeField] private GameManager _gamemanager;

    private void Start()
    {
        _musuh = GameObject.FindGameObjectsWithTag("Enemy");
    }

    private void Update()
    {
        CheckEnemyStat();
    }

    private void CheckEnemyStat()
    {
        foreach(GameObject Enemy in _musuh)
        {
            if(Enemy !=null && Enemy.activeInHierarchy)
            {
                return;
            }

        }
        _gamemanager.Winning();
        Debug.Log("menang");

    }
}
