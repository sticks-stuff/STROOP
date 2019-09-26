﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using STROOP.Structs;
using STROOP.Utilities;
using System.Xml.Linq;
using STROOP.Structs.Configurations;
using System.Drawing.Drawing2D;
using STROOP.Interfaces;
using STROOP.Controls.Map.Objects;
using STROOP.Controls.Map.Semaphores;

namespace STROOP.Map3
{
    public partial class Map3Tracker : UserControl
    {
        private readonly List<Map3Object> _mapObjectList;
        private readonly List<Map3Semaphore> _semaphoreList;

        private static readonly Image ImageEyeOpen = Properties.Resources.image_eye_open2;
        private static readonly Image ImageEyeClosed = Properties.Resources.image_eye_closed2;

        public bool IsVisible;
        private MapTrackerVisibilityType _currentVisiblityType;

        public Map3Tracker(Map3Object mapObj, List<Map3Semaphore> semaphoreList = null)
            : this(new List<Map3Object>() { mapObj }, semaphoreList)
        {
        }

        public Map3Tracker(
            List<Map3Object> mapObjectList,
            List<Map3Semaphore> semaphoreList = null)
        {
            if (mapObjectList.Count < 1) throw new ArgumentOutOfRangeException();
            semaphoreList = semaphoreList ?? new List<Map3Semaphore>();

            InitializeComponent();

            _mapObjectList = new List<Map3Object>(mapObjectList);
            _semaphoreList = new List<Map3Semaphore>(semaphoreList);

            IsVisible = true;
            _currentVisiblityType = MapTrackerVisibilityType.VisibleWhenLoaded;

            tableLayoutPanel.BorderWidth = 2;
            tableLayoutPanel.ShowBorder = true;

            comboBoxVisibilityType.DataSource = Enum.GetValues(typeof(MapTrackerVisibilityType));
            comboBoxVisibilityType.SelectedItem = MapTrackerVisibilityType.VisibleWhenLoaded;

            comboBoxOrderType.DataSource = Enum.GetValues(typeof(MapTrackerOrderType));
            comboBoxOrderType.SelectedItem = MapTrackerOrderType.OrderByY;

            SetSize(null);
            SetOpacity(null);
            SetColor(null);

            textBoxSize.AddEnterAction(() => textBoxSize_EnterAction());
            textBoxOpacity.AddEnterAction(() => textBoxOpacity_EnterAction());
            colorSelector.AddColorChangeAction((Color color) => SetColor(color));

            InitializeCogContextMenuStrip();

            UpdateControl();

            /*
            MapObjectList.ForEach(obj =>
            {
                obj.Tracker = this;
                obj.Tracked = true;
                obj.Shown = true;
                obj.Opacity = 1;
            });

            UpdateName(MapObjectList.FirstOrDefault()?.Name);
            UpdateImage(MapObjectList.FirstOrDefault()?.BitmapImage);
            UpdateBackColor(MapObjectList.FirstOrDefault()?.BackColor);
            SetRotates(MapObjectList.FirstOrDefault()?.Rotates);
            SetColor(MapObjectList.FirstOrDefault()?.MyColor);

            comboBoxVisibilityType.SelectedValueChanged += (sender, e) =>
                SetVisibilityType((MapTrackerVisibilityType)comboBoxVisibilityType.SelectedItem);
            colorSelector.AddColorChangeAction((Color color) => SetColor(color));

            SetSize(MapObjectList.FirstOrDefault()?.DefaultSize);
            SetOpacity(MapObjectList.FirstOrDefault()?.DefaultOpacity, true);
            */
        }

        private void InitializeCogContextMenuStrip()
        {
            ToolStripMenuItem itemHitboxCylinder = new ToolStripMenuItem("Add Tracker for Hitbox Cylinder");
            itemHitboxCylinder.Click += (sender, e) =>
            {
                List<Map3Object> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (Map3Object)new Map3HitboxCylinderObject(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                Map3Tracker tracker = new Map3Tracker(newMapObjs);
                Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemTangibilitySphere = new ToolStripMenuItem("Add Tracker for Tangibility Sphere");
            itemTangibilitySphere.Click += (sender, e) =>
            {
                List<Map3Object> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (Map3Object)new Map3TangibilitySphereObject(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                Map3Tracker tracker = new Map3Tracker(newMapObjs);
                Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemDrawDistanceSphere = new ToolStripMenuItem("Add Tracker for Draw Distance Sphere");
            itemDrawDistanceSphere.Click += (sender, e) =>
            {
                List<Map3Object> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (Map3Object)new Map3DrawDistanceSphereObject(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                Map3Tracker tracker = new Map3Tracker(newMapObjs);
                Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemResizableCylinder = new ToolStripMenuItem("Add Tracker for Resizable Cylinder");
            itemResizableCylinder.Click += (sender, e) =>
            {
                List<Map3Object> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    return (Map3Object)new Map3ResizableCylinderObject(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                Map3Tracker tracker = new Map3Tracker(newMapObjs);
                Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemResizableSphere = new ToolStripMenuItem("Add Tracker for Resizable Sphere");
            itemResizableSphere.Click += (sender, e) =>
            {
                List<Map3Object> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    return (Map3Object)new Map3ResizableSphereObject(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                Map3Tracker tracker = new Map3Tracker(newMapObjs);
                Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemHome = new ToolStripMenuItem("Add Tracker for Home");
            itemHome.Click += (sender, e) =>
            {
                List<Map3Object> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObject()) return null;
                    return (Map3Object)new Map3HomeObject(posAngle.GetObjAddress());
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                Map3Tracker tracker = new Map3Tracker(newMapObjs);
                Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemResizableCylinderForHome = new ToolStripMenuItem("Add Tracker for Resizable Cylinder for Home");
            itemResizableCylinderForHome.Click += (sender, e) =>
            {
                List<Map3Object> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObject()) return null;
                    PositionAngle homePosAngle = PositionAngle.ObjHome(posAngle.GetObjAddress());
                    return (Map3Object)new Map3ResizableCylinderObject(homePosAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                Map3Tracker tracker = new Map3Tracker(newMapObjs);
                Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemResizableSphereForHome = new ToolStripMenuItem("Add Tracker for Resizable Sphere for Home");
            itemResizableSphereForHome.Click += (sender, e) =>
            {
                List<Map3Object> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObject()) return null;
                    PositionAngle homePosAngle = PositionAngle.ObjHome(posAngle.GetObjAddress());
                    return (Map3Object)new Map3ResizableSphereObject(homePosAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                Map3Tracker tracker = new Map3Tracker(newMapObjs);
                Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemFloorTriangles = new ToolStripMenuItem("Add Tracker for Floor Triangles");
            ToolStripMenuItem itemWallTriangles = new ToolStripMenuItem("Add Tracker for Wall Triangles");
            ToolStripMenuItem itemCeilingTriangles = new ToolStripMenuItem("Add Tracker for Ceiling Triangles");

            pictureBoxCog.ContextMenuStrip = new ContextMenuStrip();
            pictureBoxCog.ContextMenuStrip.Items.Add(itemHitboxCylinder);
            pictureBoxCog.ContextMenuStrip.Items.Add(itemTangibilitySphere);
            pictureBoxCog.ContextMenuStrip.Items.Add(itemDrawDistanceSphere);
            pictureBoxCog.ContextMenuStrip.Items.Add(itemResizableCylinder);
            pictureBoxCog.ContextMenuStrip.Items.Add(itemResizableSphere);
            pictureBoxCog.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            pictureBoxCog.ContextMenuStrip.Items.Add(itemHome);
            pictureBoxCog.ContextMenuStrip.Items.Add(itemResizableCylinderForHome);
            pictureBoxCog.ContextMenuStrip.Items.Add(itemResizableSphereForHome);
            pictureBoxCog.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            pictureBoxCog.ContextMenuStrip.Items.Add(itemFloorTriangles);
            pictureBoxCog.ContextMenuStrip.Items.Add(itemWallTriangles);
            pictureBoxCog.ContextMenuStrip.Items.Add(itemCeilingTriangles);
            pictureBoxCog.Click += (se, ev) => pictureBoxCog.ContextMenuStrip.Show(Cursor.Position);
        }

        private void MapTracker_Load(object sender, EventArgs e)
        {
            /*
            comboBoxDisplayType.DataSource = Enum.GetValues(typeof(MapTrackerDisplayType));

            ControlUtilities.AddContextMenuStripFunctions(
                pictureBoxCog,
                new List<string>()
                {
                    "Hitbox Cylinder",
                    "Tangibility Radius",
                    "Draw Distance Radius",
                },
                new List<Action>()
                {
                    () => { },
                    () => { },
                    () => { },
                });
            pictureBoxCog.Click += (se, ev) => pictureBoxCog.ContextMenuStrip.Show(Cursor.Position);
            */
        }

        public List<Map3Object> GetMapObjectsToDisplay()
        {
            return _mapObjectList.FindAll(mapObj => mapObj.ShouldDisplay(
                (MapTrackerVisibilityType)comboBoxVisibilityType.SelectedItem));
        }

        public MapTrackerOrderType GetOrderType()
        {
            return (MapTrackerOrderType) comboBoxOrderType.SelectedItem;
        }

        private void trackBarSize_ValueChanged(object sender, EventArgs e)
        {
            SetSize(trackBarSize.Value);
        }

        private void textBoxSize_EnterAction()
        {
            SetSize(ParsingUtilities.ParseFloatNullable(textBoxSize.Text));
        }

        /** null if controls should be refreshed */
        private void SetSize(float? sizeNullable)
        {
            bool updateMapObjs = sizeNullable != null;
            float size = sizeNullable ?? _mapObjectList[0].Size;
            if (updateMapObjs)
            {
                _mapObjectList.ForEach(mapObj => mapObj.Size = size);
            }
            ControlUtilities.SetTrackBarValueCapped(trackBarSize, size);
            textBoxSize.Text = size.ToString();
        }

        private void trackBarOpacity_ValueChanged(object sender, EventArgs e)
        {
            SetOpacity((byte)trackBarOpacity.Value);
        }

        private void textBoxOpacity_EnterAction()
        {
            SetOpacity(ParsingUtilities.ParseByteNullable(textBoxOpacity.Text));
        }

        /** null if controls should be refreshed */
        private void SetOpacity(byte? opacityNullable)
        {
            bool updateMapObjs = opacityNullable != null;
            byte opacity = opacityNullable ?? _mapObjectList[0].OpacityByte;
            if (updateMapObjs)
            {
                _mapObjectList.ForEach(mapObj => mapObj.OpacityByte = opacity);
            }
            ControlUtilities.SetTrackBarValueCapped(trackBarOpacity, opacity);
            textBoxOpacity.Text = opacity.ToString();
        }

        private void checkBoxRotates_CheckedChanged(object sender, EventArgs e)
        {
            /*
            SetRotates(checkBoxRotates.Checked);
            */
        }

        public void SetRotates(bool? rotates)
        {
            /*
            if (!rotates.HasValue) return;
            checkBoxRotates.Checked = rotates.Value;
            MapObjectList.ForEach(obj =>
            {
                obj.Rotates = rotates.Value;
            });
            */
        }

        /** null if controls should be refreshed */
        public void SetColor(Color? colorNullable)
        {
            bool updateMapObjs = colorNullable != null;
            Color color = colorNullable ?? _mapObjectList[0].Color;
            if (updateMapObjs)
            {
                _mapObjectList.ForEach(mapObj => mapObj.Color = color);
            }
            colorSelector.SelectedColor = color;
        }

        public void SetVisibilityType(MapTrackerVisibilityType visibilityType)
        {
            /*
            comboBoxVisibilityType.SelectedItem = visibilityType;
            MapObjectList.ForEach(obj =>
            {
                obj.VisibilityType = visibilityType;
            });
            */
        }

        private void pictureBoxRedX_Click(object sender, EventArgs e)
        {
            Config.Map3Gui.flowLayoutPanelMap3Trackers.RemoveControl(this);
        }

        private void pictureBoxEye_Click(object sender, EventArgs e)
        {
            IsVisible = !IsVisible;
            pictureBoxEye.BackgroundImage = IsVisible ? ImageEyeOpen : ImageEyeClosed;
        }

        private void pictureBoxUpArrow_Click(object sender, EventArgs e)
        {
            Config.Map3Gui.flowLayoutPanelMap3Trackers.MoveUpControl(this);
        }

        private void pictureBoxDownArrow_Click(object sender, EventArgs e)
        {
            Config.Map3Gui.flowLayoutPanelMap3Trackers.MoveDownControl(this);
        }

        public void UpdateName(string name)
        {
            /*
            textBoxName.Text = name ?? "(Unknown)";
            */
        }

        public void UpdateImage(Bitmap image)
        {
            /*
            pictureBoxPicture.Image = image == null ? null : new Bitmap(image);
            */
        }

        public void UpdateBackColor(Color? colorNullable)
        {
            /*
            Color color = colorNullable ?? ObjectSlotsConfig.VacantSlotColor;
            panelPicture.BackColor = color;
            pictureBoxPicture.BackColor = color.Lighten(0.7);
            */
        }

        public void UpdateControl()
        {
            textBoxName.Text = string.Join(", ", _mapObjectList.ConvertAll(obj => obj.GetName()));
            pictureBoxPicture.Image = _mapObjectList[0].GetImage(); // TODO fix this

            MapTrackerVisibilityType currentVisibilityType = (MapTrackerVisibilityType)comboBoxVisibilityType.SelectedValue;
            if (currentVisibilityType != _currentVisiblityType)
            {
                if (currentVisibilityType == MapTrackerVisibilityType.VisibleWhenThisBhvrIsLoaded)
                {
                    foreach (Map3Object mapObj in _mapObjectList)
                    {
                        mapObj.NotifyStoreBehaviorCritera();
                    }
                }
                _currentVisiblityType = currentVisibilityType;
            }

            if (_semaphoreList.Any(semaphore => !semaphore.IsUsed))
            {
                Config.Map3Gui.flowLayoutPanelMap3Trackers.RemoveControl(this);
            }
        }

        public void CleanUp()
        {
            _semaphoreList.ForEach(semaphore => semaphore.IsUsed = false);
        }
    }
}
