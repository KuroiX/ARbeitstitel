using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAligner : MonoBehaviour
{
    [SerializeField] private Transform tower1, tower2;
    [SerializeField] private GameObject wallPrefab;

    private const float wallLength = 0.16f, towerOffset = 0.05f;
    private int maxNumberOfWalls = 20;

    private List<GameObject> placedWalls = new List<GameObject>();

    public void addTower(Transform tower)
    {
        if(tower1 == null)
        {
            tower1 = tower;
        } else if(tower2 == null)
        {
            tower2 = tower;
        }
    }

    private void Update()
    {
        if(tower1 != null && tower2 != null)
        {
            alignWall();
        }
    }

    private void alignWall()
    {
        float distance = Vector3.Distance(tower1.position, tower2.position);
        int numberOfNeededWalls = (int)((distance - 2 * towerOffset + 0.5f * wallLength) / wallLength);

        if(numberOfNeededWalls > maxNumberOfWalls)
        {
            //too many walls needed, don't place them
            setActiveFromTo(0, placedWalls.Count, false);
            return; 
        }

        setActiveFromTo(0, Mathf.Min(placedWalls.Count, numberOfNeededWalls), true);
        if (numberOfNeededWalls > placedWalls.Count)
        {
            //produce more walls
            while (placedWalls.Count < numberOfNeededWalls)
            {
                placedWalls.Add(Instantiate(wallPrefab));
            }
        } else
        {
            //disable wall overhead
            setActiveFromTo(numberOfNeededWalls, placedWalls.Count, false);
        }

        int index = 0;
        float offset = (distance - numberOfNeededWalls * wallLength) * 0.5f;
        Vector3 direction = (tower2.position - tower1.position).normalized;
        foreach (GameObject wall in placedWalls)
        {
            if(index >= numberOfNeededWalls)
            {
                break;
            }

            wall.transform.SetPositionAndRotation(tower1.position + (offset + (0.5f + index++) * wallLength) * direction, Quaternion.LookRotation(direction));
        }
    }

    private void setActiveFromTo(int startIndex, int endIndex, bool active)
    {
        for(int i=startIndex; i<endIndex; i++)
        {
            placedWalls[i].SetActive(active);
        }
    }
}
