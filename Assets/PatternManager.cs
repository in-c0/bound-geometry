using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PatternManager : MonoBehaviour
{
    public GridManager gridManager;
    public float beatInterval = 1.0f;
    private int beatCount = 0;

    private void Start()
    {
        StartCoroutine(GeneratePatternRoutine());
    }

    private IEnumerator GeneratePatternRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(beatInterval);

            // Select random points in one quadrant
            List<Vector2> initialPattern = GenerateRandomPatternInQuadrant();
            
            // Mirror the pattern across all quadrants
            List<Vector2> fullPattern = MirrorPatternToAllQuadrants(initialPattern);
            
            foreach (Vector2 position in fullPattern)
            {
                gridManager.TriggerExplosion(position);
            }

            beatCount++;
        }
    }

    // Generates a simple pattern by selecting random points in the first quadrant
    private List<Vector2> GenerateRandomPatternInQuadrant()
    {
        List<Vector2> pattern = new List<Vector2>();
        int pointsToGenerate = Random.Range(1, 5); // Generate 1 to 4 points for simplicity
        
        for (int i = 0; i < pointsToGenerate; i++)
        {
            float x = Random.Range(0, gridManager.gridSize / 2);
            float y = Random.Range(0, gridManager.gridSize / 2);
            Vector2 newPoint = new Vector2(x, y);
            if (!pattern.Contains(newPoint))
            {
                pattern.Add(newPoint);
            }
        }
        return pattern;
    }

    // Mirrors the given pattern across all four quadrants
    private List<Vector2> MirrorPatternToAllQuadrants(List<Vector2> initialPattern)
    {
        List<Vector2> fullPattern = new List<Vector2>(initialPattern);
        foreach (Vector2 point in initialPattern)
        {
            // Mirror horizontally
            fullPattern.Add(new Vector2(gridManager.gridSize - 1 - point.x, point.y));
            // Mirror vertically
            fullPattern.Add(new Vector2(point.x, gridManager.gridSize - 1 - point.y));
            // Mirror both horizontally and vertically
            fullPattern.Add(new Vector2(gridManager.gridSize - 1 - point.x, gridManager.gridSize - 1 - point.y));
        }
        return fullPattern;
    }
}
