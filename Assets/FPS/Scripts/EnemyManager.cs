using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{
    PlayerCharacterController m_PlayerController;

    public static Dictionary<int, EnemyController> enemies = new Dictionary<int, EnemyController>();
    //public int numberOfEnemiesTotal { get; private set; }

    public List<int> numberOfEnemiesLevel;
    public int numberOfEnemiesTotal { get; private set; }

    public int numberOfEnemiesRemaining => enemies.Count;
    
    public UnityAction<EnemyController, int> onRemoveEnemy;

    public GameObject enemyType;
    //public int enemyCount;
    public int xMin;
    public int xMax;
    // The height should stay the same since we are on a level ground
    //public int yMin;
    //public int yMax;
    public int yPos;
    public int zMin;
    public int zMax;

    int xPos;
    //int yPos;
    int zPos;
    int counter;
    int id_;

    private void Awake()
    {
        
        numberOfEnemiesTotal = numberOfEnemiesLevel[PlayerLevel.level - 1];
        Debug.Log(numberOfEnemiesTotal);
        counter = 0;
        id_ = 1;
        StartCoroutine(EnemyDrop());

        m_PlayerController = FindObjectOfType<PlayerCharacterController>();
        DebugUtility.HandleErrorIfNullFindObject<PlayerCharacterController, EnemyManager>(m_PlayerController, this);

        enemies = new Dictionary<int, EnemyController>();
    }

    public void RegisterEnemy(EnemyController enemy)
    {
        enemies.Add(id_, enemy);
        id_ += 1;
        //Debug.Log("Enemies added" + id_);
        //numberOfEnemiesTotal++;
    }

    IEnumerator EnemyDrop()
    {
        while (counter < numberOfEnemiesTotal)
        {
            xPos = Random.Range(xMin, xMax);
            zPos = Random.Range(zMin, zMax);
            var enemy = Instantiate(enemyType, new Vector3(xPos, yPos, zPos), Quaternion.identity);
            // Decrease drop rate to 0
            enemy.GetComponent<EnemyController>().dropRate = 0;

            // Increase enemy detection
            enemy.GetComponentInChildren<DetectionModule>().detectionRange = 500;

            yield return new WaitForSeconds(0.1f);
            counter += 1;
        }
    }


    public void UnregisterEnemy(EnemyController enemyKilled)
    {
        int enemiesRemainingNotification = numberOfEnemiesRemaining - 1;

        if (onRemoveEnemy != null)
        {
            onRemoveEnemy.Invoke(enemyKilled, enemiesRemainingNotification);
        }

        // removes the enemy from the list, so that we can keep track of how many are left on the map
        id_ -= 1;
        enemies.Remove(id_);
        //Debug.Log("Enemy Removed" + id_);
        //Debug.Log("Enemies left" + (numberOfEnemiesRemaining - 1));
    }
}
