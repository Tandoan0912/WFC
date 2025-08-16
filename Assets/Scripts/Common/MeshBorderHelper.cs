using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public static class MeshBorderHelper
{
    public static List<Vector3> GetBorderVertices(Mesh mesh)
    {
        if (mesh == null) return new List<Vector3>();

        // 1. Extract and count all edges
        Dictionary<Tuple<int, int>, int> edgeCounts = new Dictionary<Tuple<int, int>, int>();
        int[] triangles = mesh.triangles;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            int v1 = triangles[i];
            int v2 = triangles[i + 1];
            int v3 = triangles[i + 2];

            AddEdge(edgeCounts, v1, v2);
            AddEdge(edgeCounts, v2, v3);
            AddEdge(edgeCounts, v3, v1);
        }

        // 2. Identify boundary edges (count of 1) and collect their vertices
        HashSet<int> borderVertexIndices = new HashSet<int>();
        foreach (var entry in edgeCounts)
        {
            if (entry.Value == 1) // This is a border edge
            {
                borderVertexIndices.Add(entry.Key.Item1);
                borderVertexIndices.Add(entry.Key.Item2);
            }
        }

        // 3. Retrieve vertex positions
        Vector3[] vertices = mesh.vertices;
        List<Vector3> borderVertices = new List<Vector3>();
        foreach (int index in borderVertexIndices)
        {
            borderVertices.Add(vertices[index]);
        }

        return borderVertices;
    }

    private static void AddEdge(Dictionary<Tuple<int, int>, int> edgeCounts, int vA, int vB)
    {
        // Ensure consistent order for edge key
        Tuple<int, int> edge = (vA < vB) ? Tuple.Create(vA, vB) : Tuple.Create(vB, vA);

        if (edgeCounts.ContainsKey(edge))
        {
            edgeCounts[edge]++;
        }
        else
        {
            edgeCounts[edge] = 1;
        }
    }
}