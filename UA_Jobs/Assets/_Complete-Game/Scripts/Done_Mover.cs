using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Profiling;
using UnityEngine.Jobs;

public class Done_Mover : MonoBehaviour
{
	public List<Transform> Spaceships;
	public Transform[] SpaceshipsArray;
	private TransformAccessArray transformAccessArray;
	public Transform playerTransform;
	public float speed;
	public bool UseJobs;
	
	[SerializeField] private GameObject enemyShips;
	[SerializeField] private int spaceshipsAmount;
	[SerializeField] private float enemyShipsSpacing;
	private Vector3 enemyShipsPosition;

	private static readonly ProfilerMarker ProfilerWithJobs = 
		new ProfilerMarker(ProfilerCategory.Scripts, "JobsTester.WithJobs");
	private static readonly ProfilerMarker ProfilerNoJobs = 
		new ProfilerMarker(ProfilerCategory.Scripts, "JobsTester.NoJobs");
	public struct JobsMover : IJobParallelForTransform
	{
		public Transform playerTransform;
		public float speed;
		public void Execute(int index, TransformAccess transform)
		{
			transform.position = Vector3.MoveTowards(transform.position, 
				playerTransform.position, speed * Time.deltaTime);
		}
	}

	private void Start()
	{
		//não cria a nave
	
		for (var i = 0; i < spaceshipsAmount; i++)
		{
			enemyShipsPosition = new Vector3(enemyShipsSpacing * i, 0, 12);
			var enemys = Instantiate(enemyShips,enemyShipsPosition, Quaternion.identity);
			Spaceships.Add(enemys.transform);
		}
		SpaceshipsArray = Spaceships.ToArray();
		transformAccessArray = new TransformAccessArray(SpaceshipsArray, 4);
	}

	private void Update()
	{
		if (UseJobs)
		{
			using (ProfilerWithJobs.Auto())
			{
				var spaceshipJobs = new JobsMover()
				{
					playerTransform = playerTransform,
					speed = speed
				};
				var jobHandle = spaceshipJobs.Schedule(transformAccessArray);
				jobHandle.Complete();
			}
		}
		else
		{
			using (ProfilerNoJobs.Auto())
			{
				transform.position = Vector3.MoveTowards(transform.position, 
					playerTransform.position, speed * Time.deltaTime);
			}
		}
	}

	private void OnDestroy()
	{
		transformAccessArray.Dispose();
	}
}
