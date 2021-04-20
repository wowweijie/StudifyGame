using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    // Source:
    // https://www.youtube.com/watch?v=ydjpNNA5804&ab_channel=JimmyVegas

    public GameObject enemyType;
    public int enemyCount;
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

    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
        StartCoroutine(EnemyDrop());
    }

    IEnumerator EnemyDrop()
    {
        while (counter < enemyCount)
        {
            Debug.Log(counter);
            xPos = Random.Range(xMin, xMax);
            zPos = Random.Range(zMin, zMax);
            Instantiate(enemyType, new Vector3(xPos, yPos, zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            counter += 1;
        }
    }


}
