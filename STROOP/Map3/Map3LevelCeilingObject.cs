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
    public class Map3LevelCeilingObject : Map3TriangleObject
    {
        private List<uint> _triAddressList;
        private bool _removeCurrentTri;

        public Map3LevelCeilingObject()
            : base()
        {
            _triAddressList = TriangleUtilities.GetLevelTriangles()
                .FindAll(tri => tri.IsCeiling())
                .ConvertAll(tri => tri.Address);
            _removeCurrentTri = false;

            Opacity = 0.5;
            Color = Color.Red;
        }

        protected override List<List<(float x, float z)>> GetVertexLists()
        {
            return _triAddressList.ConvertAll(address => new TriangleDataModel(address))
                .ConvertAll(tri => tri.Get2DVertices());
        }

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                ToolStripMenuItem itemReset = new ToolStripMenuItem("Reset");
                itemReset.Click += (sender, e) =>
                {
                    _triAddressList = TriangleUtilities.GetLevelTriangles()
                        .FindAll(tri => tri.IsCeiling())
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
                uint currentTri = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.CeilingTriangleOffset);
                _triAddressList.Remove(currentTri);
            }
        }

        public override string GetName()
        {
            return "Level Ceiling Tris";
        }

        public override Image GetImage()
        {
            return Config.ObjectAssociations.TriangleCeilingImage;
        }
    }
}