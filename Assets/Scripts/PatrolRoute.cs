using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PatrolPoint
{
    public Transform patrolPoint;
    public float waitForSeconds = 0;
    public enum Direction {none, oben, oben_rechts, rechts, rechts_unten, unten , unten_links, links, links_oben };
    public Direction viewDirection;
}

public class PatrolRoute : MonoBehaviour
{
    [SerializeField] private FieldOfView fieldOfView;
    [SerializeField] private Transform spriteTransform;
    public float moveSpeed = 0.2f;
    [SerializeField]private GameObject exclamationMark;
    public PatrolPoint[] patrolRoute;

    private Vector3[] routeVectors;
    private int routeIndex = -1;
    private bool onPatrol = true;

    void Start()
    {
        routeVectors = new Vector3[patrolRoute.Length];
        int i = 0;
        foreach(PatrolPoint point in patrolRoute)
        {
            routeVectors[i] = point.patrolPoint.position;
            i++;
        }
        GoToNextPoint();
    }

    void LateUpdate()
    {
        if (onPatrol && patrolRoute.Length != 0)
        {
            transform.position = transform.position + transform.forward * moveSpeed * Time.fixedDeltaTime;
            if (Vector3.Distance(transform.position, routeVectors[routeIndex]) < 0.02)
            {
                if (patrolRoute[routeIndex].viewDirection != PatrolPoint.Direction.none)
                {
                    transform.rotation = GetViewDirection(patrolRoute[routeIndex].viewDirection);
                    spriteTransform.localRotation = Quaternion.Euler(Vector3.zero);
                    fieldOfView.SetViewDirection(spriteTransform.transform.up);
                }
                Invoke("ShowExclamationMark", patrolRoute[routeIndex].waitForSeconds - 1);
                Invoke("GoToNextPoint", patrolRoute[routeIndex].waitForSeconds);
                onPatrol = false;
            }
        }
    }

    private void ShowExclamationMark()
    {
        exclamationMark.SetActive(true);
    }

    private void GoToNextPoint()
    {
        exclamationMark.SetActive(false);

        if (patrolRoute.Length == 0)
            return;

        routeIndex++;
        if (routeIndex >= patrolRoute.Length)
            routeIndex = 0;

        transform.LookAt(routeVectors[routeIndex], Vector3.forward);
        spriteTransform.localRotation = Quaternion.Euler(90, 0, 0);
        fieldOfView.SetViewDirection(transform.forward);
        onPatrol = true;

    }

    private Quaternion GetViewDirection(PatrolPoint.Direction lookAt)
    {
        switch(lookAt)
        {
            case PatrolPoint.Direction.oben:
                return Quaternion.Euler(0, 0, 0);
            case PatrolPoint.Direction.oben_rechts:
                return Quaternion.Euler(0, 0, -45);
            case PatrolPoint.Direction.rechts:
                return Quaternion.Euler(0, 0, -90);
            case PatrolPoint.Direction.rechts_unten:
                return Quaternion.Euler(0, 0, -135);
            case PatrolPoint.Direction.unten:
                return Quaternion.Euler(0, 0, 180);
            case PatrolPoint.Direction.unten_links:
                return Quaternion.Euler(0, 0, 135);
            case PatrolPoint.Direction.links:
                return Quaternion.Euler(0, 0, 90);
            case PatrolPoint.Direction.links_oben:
                return Quaternion.Euler(0, 0, 45);
        }
        return Quaternion.Euler(0, 0, 0);

    }

}
