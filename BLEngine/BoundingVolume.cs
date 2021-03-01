using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderingEngine
{
    class BoundingVolume
    {
        public float X, Y, Z, Width, Height, Length;

        BoundingVolume(float x, float y, float z, float length, float width, float height)
        {
            X = x;
            Y = y;
            Z = z;
            Length = length; // X
            Width = width; // Y
            Height = height; // Z

        }

        BoundingVolume(Vector3 position, Vector3 bounds)
        {
            X = position.X;
            Y = position.Y;
            Z = position.Z;
            Length = bounds.X;
            Width = bounds.Y;
            Height = bounds.Z;
        }
    }
}
