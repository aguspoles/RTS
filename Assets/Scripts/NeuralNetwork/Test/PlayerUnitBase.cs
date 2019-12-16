using UnityEngine;
using System.Collections;

public class PlayerUnitBase : MonoBehaviour
{
    protected Genome genome;
	protected NeuralNetwork brain;
    protected GameObject collectable;
    protected float[] inputs;
    protected Unit unit;
    protected PlayerController controller;
    protected PathGrid grid;

    void Start()
    {
        unit = GetComponent<Unit>();
        controller = FindObjectOfType<PlayerController>();
        grid = FindObjectOfType<PathGrid>();
    }

    // Sets a brain to the tank
    public void SetBrain(Genome genome, NeuralNetwork brain)
    {
        this.genome = genome;
        this.brain = brain;
        inputs = new float[brain.InputsCount];
        OnReset();
    }

    // Used by the PopulationManager to set the closest collectable
    public void SetNearestCollectable(GameObject mine)
    {
        collectable = mine;
    }

    protected Vector3 GetDirToCollectable(GameObject mine)
    {
        return (mine.transform.position - this.transform.position).normalized;
    }
    
    protected bool IsCloseToMine(GameObject mine)
    {
        return (this.transform.position - collectable.transform.position).sqrMagnitude <= 2.0f;
    }

    protected void SetTarget(float leftForce, float rightForce, float dt)
    {
        // Tank position
        Vector3 pos = this.transform.position;

        // Use the outputs as the force of both tank tracks
        float rotFactor = Mathf.Clamp((rightForce - leftForce), -1.0f, 1.0f);

        // Rotate the tank as the rotation factor
        this.transform.rotation *= Quaternion.AngleAxis(rotFactor * 3 * dt, Vector3.up);

        // Move the tank in current forward direction
        pos += this.transform.forward * Mathf.Abs(rightForce + leftForce) * dt/** 0.5f * 5 * dt*/;

        // Sets current position
        unit.MoveToPosition(pos, Vector3.zero);

    }

	// Update is called once per frame
	public void Think(float dt) 
	{
        OnThink(dt);

        if(unit.targetReached)
        {
            OnTakeCollectable(collectable);
            // Move the mine to a random position in the screen
            PopulationManager.Instance.RelocateMine(collectable);
        }
	}

    protected virtual void OnThink(float dt)
    {

    }

    protected virtual void OnTakeCollectable(GameObject mine)
    {
    }

    protected virtual void OnReset()
    {

    }
}
