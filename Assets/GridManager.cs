using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject gridCellPrefab;
    public int gridSize = 8; // Default size, can be adjusted in Unity Editor
    public GameObject explosionPrefab; // Assign an explosion effect prefab
    private ParticleSystem[,] gridCellParticleSystems; // 2D array to hold references to the particle systems

    public Vector3 explosionPosOffset = new Vector3(0, 0, -1);

    private void Start()
    {
        gridCellParticleSystems = new ParticleSystem[gridSize, gridSize];
        GenerateGrid();
    }
    void GenerateGrid()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector3 position = new Vector3(x, y, 0); // Adjust for 2D or 3D based on your project
                GameObject cell = Instantiate(gridCellPrefab, position, Quaternion.identity, transform);
                ParticleSystem ps = cell.GetComponentInChildren<ParticleSystem>();
                if (ps != null)
                {
                    gridCellParticleSystems[x, y] = ps;
                }
                else
                {
                    Debug.LogError("Grid cell prefab does not contain a Particle System component.");
                }
            }
        }
    }

    public void TriggerExplosion(Vector2 gridPosition)
    {
        int x = (int)gridPosition.x;
        int y = (int)gridPosition.y;

        if (x >= 0 && y >= 0 && x < gridSize && y < gridSize)
        {
            ParticleSystem ps = gridCellParticleSystems[x, y];
            if (ps != null)
            {
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear); // Stop current emission and clear to prevent overlap
                ps.Play(); // Start the explosion effect
            }
        }
        else
        {
            Debug.LogError("Grid position out of bounds.");
        }
    }

    private Vector3 GridPositionToWorldPosition(Vector2 gridPosition)
    {
        // Transform grid position to world position
        // Adjust this based on your grid setup and scaling
        return new Vector3(gridPosition.x, 0, gridPosition.y);
    }

}
