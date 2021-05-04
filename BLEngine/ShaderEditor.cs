
using OpenTK.Graphics.ES11;
using System.Collections.Generic;
using System.Linq.Expressions;
using ImGuiNET;
using System.Runtime.Remoting.Contexts;
using System.Numerics;

namespace RenderingEngine.ShaderEditor
{
    public enum ShaderNodePinType
    {
        TYPE_NULL,
        TYPE_FLOAT,
        TYPE_FLOAT2,
        TYPE_FLOAT3,
        TYPE_FLOAT4,
        TYPE_MAT3X3,
        TYPE_MAT3X4,
        TYPE_MAT4X4,
        TYPE_TEXTURE2D,
        TYPE_TEXTURE3D,
        TYPE_TEXTURECUBE
    }

    public enum ShaderOutput
    {
        OUTPUT_DEFAULT,
        OUTPUT_TRANSLUCENT,
        OUTPUT_EMISSIVEONLY,
        OUTPUT_DEBUG
    }

    public class ShaderNodePinDesc
    {
        public ShaderNode ParentNode = null;
        public ShaderNode ConnectedNode = null;
        public ShaderNodePinType JackType = ShaderNodePinType.TYPE_NULL;
        public int PinIndex = -1;
        public string Name = "NULL";
        public string Description = "NULL";
        public bool IsOutput = false;
        public bool Connected = false;

        public ShaderNodePinDesc(ShaderNode ParentNode, ShaderNode ConnectedNode, ShaderNodePinType JackType, int JackIndex, string Name, string Description, bool IsOutput)
        {
            this.ParentNode = ParentNode;
            this.ConnectedNode = ConnectedNode;
            this.JackType = JackType;
            this.PinIndex = JackIndex;
            this.Name = Name == "" || Name == null ? "NULL" : Name;
            this.Description = Description == "" || Description == null ? "NULL" : Description;
            this.IsOutput = IsOutput;

            if(ConnectedNode != null)
            {
                this.Connected = true;
            }
        }

        bool Disconnect()
        {
            if (ConnectedNode == null)
            {
                Connected = false;
                return false;
            }

            ConnectedNode = null;
            Connected = false;
            return true;
        }

        bool Connect(ShaderNodePinDesc p)
        {
            if(p == null)
            {
                Connected = false;
                return false;
            }

            ConnectedNode = p.ParentNode;
            Connected = true;
            return true;
        }
        ShaderNode GetConnectedNode()
        {
            return ConnectedNode;
        }

        ShaderNodePinType GetPinType()
        {
            return JackType;
        }

        int GetPinIndex()
        {
            return PinIndex;
        }
    }

    public class ShaderNode
    {
        public List<ShaderNodePinDesc> Pins = new List<ShaderNodePinDesc>();
        public bool CanBeDeleted = true;
        public string buffer;

        public void Draw(ShaderEditorGraph graph)
        {
            ImDrawListPtr list = ImGui.GetWindowDrawList();

            list.AddCircle(new Vector2(0.5f, 0.5f), 0.5f, 0, 8, 1.0f);
            list.AddLine(new Vector2(0.0f, 0.0f), new Vector2(10.0f, 10.0f), 1);
        }

        public ShaderNodePinDesc GetPin(int PinIndex)
        {
            return Pins[PinIndex];
        }

        public ShaderNode(ShaderEditorGraph Graph)
        {
            Pins.Add(new ShaderNodePinDesc(this, null, ShaderNodePinType.TYPE_NULL, 0, "NULL", "NULL", false));
            Pins.Add(new ShaderNodePinDesc(this, null, ShaderNodePinType.TYPE_NULL, 0, "NULL", "NULL", true));
            Graph.AddNode(this);
        }

        public void DestroyNode(ShaderEditorGraph graph)
        {
            graph.RemoveNode(this);
            this.CleanUp();
        } 

        public bool ConnectPin(ShaderNodePinDesc n, int JackIndex)
        {
            if (n == null)
            {
                return false;
            }

            Pins[JackIndex].ConnectedNode = n.ParentNode;
            return true;
        }
        public bool DisconnectPin(ShaderNodePinDesc p)
        {
            if (p == null)
            {
                return false;
            }

            p.ConnectedNode = null;
            p.Connected = false;
            return true;
        }

        public string GetNodeBuffer()
        {
            return buffer;
        }

        public bool CleanUp()
        {
            Pins = null;
            return true;
        }
    }

    public class ShaderOutputDesc
    {
        string ShaderCode;
        int NumVariables;
        bool NeedsUpdate = false;

        public ShaderOutputDesc(ShaderOutput output)
        {
            NeedsUpdate = true;
        }

    }

    public class ShaderOutputNode : ShaderNode
    {
        public ShaderOutputNode(ShaderEditorGraph graph, ShaderOutputDesc desc) : base(graph)
        {
            this.CanBeDeleted = false;
            Pins.Add(new ShaderNodePinDesc(this, null, ShaderNodePinType.TYPE_FLOAT4, 0, "Diffuse", "The Diffuse component of the shader.", false));
            Pins.Add(new ShaderNodePinDesc(this, null, ShaderNodePinType.TYPE_FLOAT4, 1, "Emissive", "The Emissive component of the shader", false));
            Pins.Add(new ShaderNodePinDesc(this, null, ShaderNodePinType.TYPE_FLOAT, 2, "Roughness", "The Roughness component of the shader.", false));
            Pins.Add(new ShaderNodePinDesc(this, null, ShaderNodePinType.TYPE_FLOAT, 3, "Metalness", "The Metalness component of the shader.", false));
            Pins.Add(new ShaderNodePinDesc(this, null, ShaderNodePinType.TYPE_FLOAT3, 4, "Normal", "The Normal component of the shader.", false));
            
        }
    }

    public class ShaderNode_Add : ShaderNode
    {
        public ShaderNode_Add(ShaderEditorGraph graph) : base(graph)
        {

        }
    }

    interface IShaderEditorGraph
    {
        void RenderGraph(float deltaTime);
        void SolveNodes();
        void AddNode(ShaderNode node);
        void RemoveNode(ShaderNode node);
        void BeginRendering();
        void EndRendering();
    }

    public class ShaderEditorGraph : IShaderEditorGraph
    {
        List<ShaderNode> NodeGraphContents = new List<ShaderNode>();
        public ShaderEditorGraph()
        {
            ShaderOutputNode output = new ShaderOutputNode(this, new ShaderOutputDesc(ShaderOutput.OUTPUT_DEFAULT));
        }
        public void RenderGraph(float DeltaTime)
        {
            BeginRendering();

            foreach(ShaderNode node in NodeGraphContents)
            {
                node.Draw(this);
            }

            EndRendering();
        }
        public void SolveNodes()
        {
            // Solve node graph here every time we update the graph :)
        }
        public void AddNode(ShaderNode node)
        {
            NodeGraphContents.Add(node);
        }
        public void RemoveNode(ShaderNode node)
        {
            NodeGraphContents.Remove(node);
        }
        public void BeginRendering()
        {
            ImGui.Begin("Shader Editor");
        }
        public void EndRendering()
        {
            ImGui.End();
        }
    }
}
