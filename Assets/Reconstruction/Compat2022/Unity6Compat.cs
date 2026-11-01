using System.Collections.Generic;
using UnityEngine;

// Unity 6 APIs used by the recovered game code that Unity 2022.3 lacks.
public static class Unity6Compat
{
    // Unity 6 named colors (CSS values / 255).
    public static readonly Color OrangeRed = new Color32(255, 69, 0, 255);
    public static readonly Color AliceBlue = new Color32(240, 248, 255, 255);
    public static readonly Color CadetBlue = new Color32(95, 158, 160, 255);

    static readonly List<Vector2> s_Points = new List<Vector2>();

    // Unity 6 PolygonCollider2D.CreateFromSprite traces the sprite's alpha at the given detail
    // and tolerance. 2022 cannot trace at runtime, so this uses the sprite's imported physics
    // shape (generated from the same alpha outline) and falls back to the sprite mesh outline.
    public static void CreateFromSprite(this PolygonCollider2D collider, Sprite sprite, float detail = 0.25f, byte alphaTolerance = 200)
    {
        int shapes = sprite.GetPhysicsShapeCount();
        if (shapes > 0)
        {
            collider.pathCount = shapes;
            for (int i = 0; i < shapes; i++)
            {
                sprite.GetPhysicsShape(i, s_Points);
                collider.SetPath(i, s_Points);
            }
            return;
        }
        var loops = MeshOutline(sprite.vertices, sprite.triangles);
        collider.pathCount = loops.Count;
        for (int i = 0; i < loops.Count; i++) collider.SetPath(i, loops[i]);
    }

    // Boundary loops of a triangle mesh: edges used by exactly one triangle, chained.
    static List<List<Vector2>> MeshOutline(Vector2[] vertices, ushort[] triangles)
    {
        var count = new Dictionary<(int, int), int>();
        for (int t = 0; t < triangles.Length; t += 3)
            for (int e = 0; e < 3; e++)
            {
                int a = triangles[t + e], b = triangles[t + (e + 1) % 3];
                var key = a < b ? (a, b) : (b, a);
                count[key] = count.TryGetValue(key, out int n) ? n + 1 : 1;
            }
        var next = new Dictionary<int, int>();
        for (int t = 0; t < triangles.Length; t += 3)
            for (int e = 0; e < 3; e++)
            {
                int a = triangles[t + e], b = triangles[t + (e + 1) % 3];
                if (count[a < b ? (a, b) : (b, a)] == 1) next[a] = b;
            }
        var loops = new List<List<Vector2>>();
        while (next.Count > 0)
        {
            var loop = new List<Vector2>();
            int start = 0;
            foreach (var key in next.Keys) { start = key; break; }
            int current = start;
            while (next.TryGetValue(current, out int to))
            {
                loop.Add(vertices[current]);
                next.Remove(current);
                current = to;
                if (current == start) break;
            }
            if (loop.Count >= 3) loops.Add(loop);
        }
        return loops;
    }
}
