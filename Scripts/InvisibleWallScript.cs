using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Invisible Wall builder Editor tool. Sam Redfern, 2022.
#if UNITY_EDITOR
[CustomEditor(typeof(InvisibleWallScript))]

public class InvisibleWallBuilderCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        // Enable the default inspector UI first
        base.OnInspectorGUI();

        InvisibleWallScript iwb = (InvisibleWallScript)target;
        InvisibleWallScript.InvisibleWallBuilderState state = iwb.GetState();

        if (state == InvisibleWallScript.InvisibleWallBuilderState.IDLE)
        {
            if (GUILayout.Button("Add Block(s)"))
                iwb.StartAddingBlocks();

            if (GUILayout.Button("Clear Children"))
                iwb.ClearChildren();
        }
        else
        {
            if (GUILayout.Button("Stop"))
                iwb.ResetState();

            GUILayout.BeginHorizontal();
            if (state == InvisibleWallScript.InvisibleWallBuilderState.FIRST_CLICK)
                EditorGUILayout.LabelField("Click on a surface in the scene to start adding a block..");
            else
                EditorGUILayout.LabelField("Click to show the other end of the block..");
            GUILayout.EndHorizontal();
        }
    }

    void OnEnable()
    {
        InvisibleWallScript iwb = (InvisibleWallScript)target;
        iwb.ResetState();
    }

    void OnSceneGUI()
    {
        InvisibleWallScript iwb = (InvisibleWallScript)target;

        if (!iwb.IsInIdleState())
        {
            if (Event.current.type == EventType.Layout)
            {
                // Prevents clicks in the scene from switching away from our InvisibleWallScript object
                HandleUtility.AddDefaultControl(0);
            }

            if (Event.current.type == EventType.MouseDown)
            {
                Vector2 pos = Event.current.mousePosition;
                Ray ray = HandleUtility.GUIPointToWorldRay(pos);
                RaycastHit hitInfo;

                if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, iwb.buildOnSurfaces))
                {
                    iwb.RecordSceneMouseClick(hitInfo.point);
                }
            }
        }
    }
}
#endif



public class InvisibleWallScript : MonoBehaviour
{
    public enum InvisibleWallBuilderState
{
    IDLE,
    FIRST_CLICK,
    SECOND_CLICK
}
    // Inspector settings
    public LayerMask buildOnSurfaces = ~0;
    public float wallWidth = 1f, wallHeight = 3f;
    [Range(0f, 3f)] [Tooltip("Subdivisions per metre")] public float meshResolution = 0f;

    private InvisibleWallBuilderState state = InvisibleWallBuilderState.IDLE;
    private Vector3 firstClickPos;

    public InvisibleWallBuilderState GetState()
    {
        return state;
    }

    public bool IsInIdleState()
    {
        return state == InvisibleWallBuilderState.IDLE;
    }

    public void StartAddingBlocks()
    {
        state = InvisibleWallBuilderState.FIRST_CLICK;
    }

    public void ResetState()
    {
        state = InvisibleWallBuilderState.IDLE;
    }

    public void ClearChildren()
    {
        // Only destroy children with the name "InvisibleWall"
        for (int i = 0; i < transform.childCount; i++)
        {
            string n = transform.GetChild(i).gameObject.name;
            if (n == "InvisibleBoxWall" || n == "InvisibleMeshWall")
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
                i--;
            }
        }
    }

#if UNITY_EDITOR
    public void RecordSceneMouseClick(Vector3 worldPos)
    {
        // Debug.Log("Hit at: " + worldPos);
        if (state == InvisibleWallBuilderState.FIRST_CLICK)
        {
            state = InvisibleWallBuilderState.SECOND_CLICK;
            firstClickPos = worldPos;
        }
        else if (state == InvisibleWallBuilderState.SECOND_CLICK)
        {
            AddWall(firstClickPos, worldPos);
            firstClickPos = worldPos; // Use just one click for the next one
        }

        EditorUtility.SetDirty(this); // Tell the inspector to repaint
    }

    private void AddWall(Vector3 fromPos, Vector3 toPos)
    {
        GameObject go = new GameObject();
        go.transform.SetParent(transform, false);
        Vector3 centrePos = (fromPos + toPos) / 2f;
        go.transform.position = centrePos;
        go.layer = gameObject.layer;

        if (meshResolution == 0f)
        {
            // Use a simple box collider
            go.name = "InvisibleBoxWall";
            go.transform.LookAt(toPos);
            go.transform.localScale = new Vector3(wallWidth, wallHeight, Vector3.Distance(fromPos, toPos));
            go.AddComponent<BoxCollider>();
        }
        else
        {
            // Use a more accurate mesh collider
            go.name = "InvisibleMeshWall";
            Mesh mesh = new Mesh();

            // Raycast to get the bottom vertices of the mesh
            List<Vector3> verts = new List<Vector3>();
            Vector3 diff = toPos - fromPos;
            Vector3 step = diff.normalized / meshResolution;
            Vector3 diffXZ = diff.normalized;
            diffXZ.y = 0;
            diffXZ.Normalize();
            Vector3 left = Quaternion.Euler(0, -90f, 0) * diffXZ * wallWidth / 2f;
            int numSteps = Mathf.FloorToInt(diff.magnitude * meshResolution);
            RaycastHit hitInfo;

            for (int i = 0; i <= numSteps; i++)
            {
                Vector3 vert1 = Vector3.zero;
                for (int v = 0; v < 2; v++)
                {
                    Vector3 castPos = fromPos + step * i;
                    if (i == numSteps) castPos = toPos;
                    castPos.y += wallHeight;

                    if (v == 0) castPos += left;
                    else castPos -= left;

                    int attempts = 0;
                    while (!Physics.Raycast(castPos, Vector3.down, out hitInfo, Mathf.Infinity, buildOnSurfaces))
                    {
                        castPos.y += wallHeight / 2f;
                        attempts++;
                        if (attempts > 20) break;
                    }

                    if (attempts <= 20)
                    {
                        if (v == 0) vert1 = hitInfo.point - centrePos;
                        else
                        {
                            verts.Add(vert1);
                            verts.Add(hitInfo.point - centrePos);
                        }
                    }
                }
            }

            // Add top vertices of the mesh
            int bottomNum = verts.Count;
            for (int i = 0; i < bottomNum; i++)
            {
                Vector3 vert = verts[i];
                vert.y += wallHeight;
                verts.Add(vert);
            }

            // Triangles
            List<int> tris = new List<int>();
            for (int i = 0; i < bottomNum - 2; i++)
            {
                if (i % 2 == 0)
                {
                    tris.Add(i); tris.Add(i + 1); tris.Add(i + 2);
                    tris.Add(i + bottomNum); tris.Add(i + 2 + bottomNum); tris.Add(i + 1 + bottomNum);
                    tris.Add(i); tris.Add(i + 2); tris.Add(i + bottomNum);
                    tris.Add(i + bottomNum); tris.Add(i + 2); tris.Add(i + 2 + bottomNum);
                }
                else
                {
                    tris.Add(i); tris.Add(i + 2); tris.Add(i + 1);
                    tris.Add(i + bottomNum); tris.Add(i + 1 + bottomNum); tris.Add(i + 2 + bottomNum);
                    tris.Add(i); tris.Add(i + bottomNum); tris.Add(i + 2);
                    tris.Add(i + 2); tris.Add(i + bottomNum); tris.Add(i + 2 + bottomNum);
                }
            }

            // End-cap triangles
            tris.Add(0); tris.Add(0 + bottomNum); tris.Add(1);
            tris.Add(1); tris.Add(0 + bottomNum); tris.Add(1 + bottomNum);

            int idx = bottomNum - 2;
            tris.Add(idx); tris.Add(idx + 1); tris.Add(idx + bottomNum);
            tris.Add(idx + 1); tris.Add(idx + 1 + bottomNum); tris.Add(idx + bottomNum);

            mesh.vertices = verts.ToArray();
            mesh.triangles = tris.ToArray();
            MeshCollider meshCollider = go.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = mesh;
        }
    }
#endif
}
