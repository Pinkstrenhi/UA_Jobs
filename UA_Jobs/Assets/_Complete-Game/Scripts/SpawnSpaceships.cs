using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;

public class SpawnSpaceships : MonoBehaviour
{
    [SerializeField] private GameObject enemyShips;
    [SerializeField] private int spaceshipsAmount;
    [SerializeField] private float enemyShipsSpacing;
    private Vector3 enemyShipsPosition;
   //private Done_Mover doneMover; //da o erro

    private void Awake()
    {
        for (var i = 0; i < spaceshipsAmount; i++)
        {
            enemyShipsPosition = new Vector3(enemyShipsSpacing * i, 0, 12);
            var enemys = Instantiate(enemyShips,enemyShipsPosition, Quaternion.identity);
          // doneMover.Spaceships.Add(enemys.transform);
        }
    }
}
