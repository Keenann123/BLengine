﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace RenderingEngine
{
    class Player : Entity
    {
        public Player() : base(new Vector3(0, 0, 0), new Quaternion(new Vector3(0, 0, 0)), new Vector3(1, 1, 1))
        {

        }
    }
}