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
using System.Drawing.Imaging;
using STROOP.Models;
using System.Windows.Forms;

namespace STROOP.Map3
{
    public class Map3LevelWallObject : Map3WallObject
    {
        private List<uint> _triAddressList;
        private bool _removeCurrentTri;

        public Map3LevelWallObject()
            : base()
        {
            _triAddressList = TriangleUtilities.GetLevelTriangles()
                .FindAll(tri => tri.IsWall())
                .ConvertAll(tri => tri.Address);
            _removeCurrentTri = false;

            Opacity = 0.5;
            Color = Color.Green;
        }

        protected override List<(float x1, float z1, float x2, float z2, bool xProjection)> GetWallData()
        {
            return _triAddressList.ConvertAll(address => new TriangleDataModel(address))
                .ConvertAll(tri => Map3Utilities.GetWallDataFromTri(tri));
        }

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                ToolStripMenuItem itemReset = new ToolStripMenuItem("Reset");
                itemReset.Click += (sender, e) =>
                {
                    _triAddressList = TriangleUtilities.GetLevelTriangles()
                        .FindAll(tri => tri.IsWall())
                        .ConvertAll(tri => tri.Address);
                };

                ToolStripMenuItem itemRemoveCurrentTri = new ToolStripMenuItem("Remove Current Tri");
                itemRemoveCurrentTri.Click += (sender, e) =>
                {
                    _removeCurrentTri = !_removeCurrentTri;
                    itemRemoveCurrentTri.Checked = _removeCurrentTri;
                };

                ToolStripMenuItem itemShowTriData = new ToolStripMenuItem("Show Tri Data");
                itemShowTriData.Click += (sender, e) =>
                {
                    List<TriangleDataModel> tris = _triAddressList.ConvertAll(address => new TriangleDataModel(address));
                    TriangleUtilities.ShowTriangles(tris);
                };

                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(itemReset);
                _contextMenuStrip.Items.Add(itemRemoveCurrentTri);
                _contextMenuStrip.Items.Add(itemShowTriData);
            }

            return _contextMenuStrip;
        }

        public override void Update()
        {
            if (_removeCurrentTri)
            {
                uint currentTri = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.WallTriangleOffset);
                _triAddressList.Remove(currentTri);
            }
        }

        public override string GetName()
        {
            return "Level Wall Tris";
        }

        public override Image GetImage()
        {
            return Config.ObjectAssociations.TriangleWallImage;
        }
    }
}