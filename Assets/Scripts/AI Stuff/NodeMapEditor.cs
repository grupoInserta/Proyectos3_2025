using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(NodeMap))]
public class NodeMapEditor : Editor
{
    public Color gizmoColor;
    private static bool editingNodes;
    private void OnSceneGUI()
    {
        Tools.hidden = true;
        NodeMap nodeMap = (NodeMap)target;

        if (nodeMap.nodes == null) return;

        Camera sceneCamera = SceneView.lastActiveSceneView.camera; // Get the current scene camera

        Event e = Event.current;
        bool clickedInScene = e.type == EventType.MouseDown && e.button == 0;

        // Collect label data
        List<(Vector2 guiPos, string text)> labelsToDraw = new List<(Vector2, string)>();

        for (int i = 0; i < nodeMap.nodes.Count; i++)
        {
            Node node = nodeMap.nodes[i];

            bool isPrimary = node == nodeMap.selectedPrimaryNode;
            bool isSecondary = node == nodeMap.selectedSecondaryNode;
            bool isSelected = node == nodeMap.debugNode;

            if (nodeMap.debugNode == node || editingNodes)
            {
                EditorGUI.BeginChangeCheck();
                Vector3 newPos = Handles.PositionHandle(node.position, Quaternion.identity);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(nodeMap, "Move Node");
                    node.position = newPos;
                    EditorUtility.SetDirty(nodeMap);
                }
            }

            Vector3 worldPos = node.position;
            Vector2 guiPos = HandleUtility.WorldToGUIPoint(worldPos);

            // Check if the node is within the camera's view (visible to the camera)
            Vector3 viewportPos = sceneCamera.WorldToViewportPoint(worldPos);
            bool isVisible = viewportPos.z > 0 && viewportPos.x >= 0 && viewportPos.x <= 1 && viewportPos.y >= 0 && viewportPos.y <= 1;

            if (isVisible)
            {
                labelsToDraw.Add((guiPos, $"{i}"));
            }

            foreach (Connection connection in node.connections)
            {
                Node connected = connection.to;
                if (connected != null)
                {
                    if (isSelected) Handles.color = Color.magenta;
                    else if (isPrimary) Handles.color = Color.red;
                    else if (isSecondary) Handles.color = Color.cyan;
                    else Handles.color = Color.green;

                    Handles.DrawLine(node.position, connected.position);
                }
            }

            // Draw handle button
            Vector3 buttonPos = node.position;
            if (isPrimary) Handles.color = Color.red;
            else if (isSecondary) Handles.color = Color.cyan;
            else if (isSelected) Handles.color = Color.magenta;
            else Handles.color = Color.white;

            if (Handles.Button(buttonPos, Quaternion.identity, 0.1f, 0.1f, Handles.SphereHandleCap))
            {
                Undo.RecordObject(nodeMap, "Set Debug Node");
                nodeMap.debugNode = node;
                EditorUtility.SetDirty(nodeMap);
            }
        }

        // Draw all labels AFTER all handles to make sure they appear on top
        Handles.BeginGUI();
        GUIStyle bigLabelStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 14,
            normal = { textColor = Color.black },
            alignment = TextAnchor.MiddleCenter
        };

        foreach (var (guiPos, text) in labelsToDraw)
        {
            GUI.Label(new Rect(guiPos.x - 20, guiPos.y - 10, 40, 20), text, bigLabelStyle);
        }
        Handles.EndGUI();
    }


    private void OnDisable()
    {
        Tools.hidden = false;
    }
    public void PerformAction()
    {
        Debug.Log("Handles color" + Handles.color);
        Debug.Log("Gizmo color" + gizmoColor);
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        NodeMap nodeMap = (NodeMap)target;
        GUI.backgroundColor = editingNodes ? Color.grey : Color.white;
        if (GUILayout.Button("Modify Nodes"))
        {
            editingNodes = !editingNodes;
        }
        GUI.backgroundColor = Color.white;
        if (GUILayout.Button("Add Node"))
        {
            nodeMap.AddNodeFromSelected();
            EditorUtility.SetDirty(nodeMap);
        }
        GUILayout.Space(10);

        EditorGUILayout.LabelField("Node Selection", EditorStyles.boldLabel);

        if (GUILayout.Button("Select Primary Node"))
        {
            nodeMap.SelectPrimaryNode();
            EditorUtility.SetDirty(nodeMap);
        }
        if (GUILayout.Button("Select Secondary Node"))
        {
            nodeMap.SelectSecondaryNode();
            EditorUtility.SetDirty(nodeMap);
        }
        if (GUILayout.Button("Deselect Nodes"))
        {
            nodeMap.DeselectNodes();
            EditorUtility.SetDirty(nodeMap);
        }
        if (GUILayout.Button("Connect Primary And Secondary Nodes"))
        {
            nodeMap.ConnectPrimaryAndSecondaryNodes();
            EditorUtility.SetDirty(nodeMap);
        }

        GUILayout.Space(10);


        if (GUILayout.Button("Isolate Selected Node"))
        {
            nodeMap.debugNode.Isolate(true);
            EditorUtility.SetDirty(nodeMap);
        }
        if (GUILayout.Button("Remove Selected Node"))
        {
            nodeMap.RemoveSelectedNode();
            EditorUtility.SetDirty(nodeMap);
        }

        if (GUILayout.Button("Weight All Connections"))
        {
            nodeMap.WeightAllConnections();
            EditorUtility.SetDirty(nodeMap);
        }
        if (GUILayout.Button("Isolate All Nodes"))
        {
            nodeMap.IsolateAllNodes();
            EditorUtility.SetDirty(nodeMap);
        }

        if (nodeMap.selectedPrimaryNode != null)
        {
            GUILayout.Label($"Selected Node: {nodeMap.selectedPrimaryNode.position}");
        }
    }
}
