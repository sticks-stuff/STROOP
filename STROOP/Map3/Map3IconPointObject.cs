﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using STROOP.Controls.Map;
using OpenTK.Graphics.OpenGL;
using STROOP.Utilities;
using STROOP.Structs.Configurations;
using STROOP.Structs;
using OpenTK;

namespace STROOP.Map3
{
    public abstract class Map3IconPointObject : Map3IconObject
    {
        public Map3IconPointObject()
            : base()
        {
        }

        public override void DrawOnControl()
        {
            UpdateImage();

            // Update map object
            (double x, double y, double z, double angle) = GetPositionAngle().GetValues();
            (float xPosPixels, float zPosPixels) = Map3Utilities.ConvertCoordsForControl((float)x, (float)z);
            float angleDegrees = Map3Utilities.ConvertAngleForControl(angle);
            SizeF size = Map3Utilities.ScaleImageSize(Image.Size, Size);
            DrawTexture(new PointF(xPosPixels, zPosPixels), size, angleDegrees);
        }
    }
}
