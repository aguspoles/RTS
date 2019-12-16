using System;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class SensorySystem
{
    public float targetLostMaxTime;
    public float fovAngle = 60;
    public float fovDistance = 10;

    internal Vector3 lastKnownPosition = Vector3.zero;

    Unit pathUnit;
    AIComponent aiComponent;

    float targetLostTimer = 0;

    public void Initialize(AIComponent aiComponent, Unit pathUnit)
    {
        this.aiComponent = aiComponent;
        this.pathUnit = pathUnit;
    }

    internal void Update()
    {
        if (aiComponent.pathUnit.target != Vector3.zero)
        {
            UpdateHasTarget();
        }
    }

    private void UpdateHasTarget()
    {
        if (IsEventSourceVisible(aiComponent.pathUnit.target))
        {
            lastKnownPosition = aiComponent.pathUnit.target;
            targetLostTimer = 0;
        }
        else
        {
            targetLostTimer += Time.deltaTime;

            if (targetLostTimer >= targetLostMaxTime)
            {
                aiComponent.currentState = AIState.IDLE;
                pathUnit.ResetPath();
                aiComponent.pathUnit.target = Vector3.zero;
                targetLostTimer = 0;
            }
        }
    }

    public bool IsEventSourceVisible(Vector3 position)
    {
        bool isVisible = false;

        Vector3 sourcePosition = position;
        Vector3 aiPosition = aiComponent.transform.position;

        aiPosition.y += 1;
        sourcePosition.y += 1;

        Vector3 vectorToSource = (sourcePosition - aiPosition);

        if (vectorToSource.sqrMagnitude < fovDistance * fovDistance)
        {
            Vector3 sourceDirection = vectorToSource.normalized;
            sourceDirection.y = 0;

            if (Vector3.Angle(aiComponent.transform.forward, sourceDirection) < (fovAngle / 2))
            {
                RaycastHit hit;
                if (Physics.Raycast(aiPosition, sourceDirection, out hit, fovDistance, ~LayerMask.GetMask("AI")))
                {
                    isVisible = hit.point == position;
                }
            }
        }

        return isVisible;
    }
}
