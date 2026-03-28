using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    private int enemiesAlive = 0;
    public int enemiesPerRound = 3;

    void Start() => StartRound();

    void StartRound()
    {
        for (int i = 0; i < enemiesPerRound; i++)
        {
            Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            enemiesAlive++;
        }
    }

    public void EnemyDied()
    {
        enemiesAlive--;
        if (enemiesAlive <= 0)
            RoundComplete();
    }

    void RoundComplete()
    {
        Debug.Log("Ronda completa, aparece la máscara");
        // Aquí llamas tu sistema de diálogo
    }
}
