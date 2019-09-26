﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using STROOP.Controls.Map.Trackers;

namespace STROOP.Map3
{
    public class Map3Gui
    {
        // Major Controls
        public GLControl GLControl;
        public Map3TrackerFlowLayoutPanel flowLayoutPanelMap3Trackers;



        // Options - CheckBoxes
        public CheckBox checkBoxMap3OptionsTrackMario;
        public CheckBox checkBoxMap3OptionsTrackHolp;
        public CheckBox checkBoxMap3OptionsTrackCamera;
        public CheckBox checkBoxMap3OptionsTrackFloorTri;
        public CheckBox checkBoxMap3OptionsTrackCeilingTri;
        public CheckBox checkBoxMap3OptionsTrackCellGridlines;
        public CheckBox checkBoxMap3OptionsTrackCurrentCell;
        public CheckBox checkBoxMap3OptionsTrackUnitGridlines;
        public CheckBox checkBoxMap3OptionsTrackCurrentUnit;
        public CheckBox checkBoxMap3OptionsEnablePuView;

        // Options - Buttons
        public Button buttonMap3OptionsAddNewTracker;
        public Button buttonMap3OptionsClearAllTrackers;
        public Button buttonMap3OptionsTrackAllObjects;
        public Button buttonMap3OptionsTrackMarkedObjects;

        // Options - ComboBoxes
        public ComboBox comboBoxMap3OptionsLevel;
        public ComboBox comboBoxMap3OptionsBackground;



        // Controllers - GroupBoxes
        public GroupBox groupBoxMap3ControllersScale;
        public GroupBox groupBoxMap3ControllersCenter;
        public GroupBox groupBoxMap3ControllersAngle;

        // Controllers - Scale - Left
        public RadioButton radioButtonMap3ControllersScaleCourseDefault;
        public RadioButton radioButtonMap3ControllersScaleMaxCourseSize;
        public RadioButton radioButtonMap3ControllersScaleCustom;
        public BetterTextbox textBoxMap3ControllersScaleCustom;

        // Controllers - Scale - Right
        public BetterTextbox textBoxMap3ControllersScaleChange;
        public Button buttonMap3ControllersScaleMinus;
        public Button buttonMap3ControllersScalePlus;

        // Controllers - Center - Left
        public RadioButton radioButtonMap3ControllersCenterBestFit;
        public RadioButton radioButtonMap3ControllersCenterOrigin;
        public RadioButton radioButtonMap3ControllersCenterCustom;
        public BetterTextbox textBoxMap3ControllersCenterCustom;

        // Controllers - Center - Right
        public BetterTextbox textBoxMap3ControllersCenterChange;
        public Button buttonMap3ControllersCenterUp;
        public Button buttonMap3ControllersCenterUpRight;
        public Button buttonMap3ControllersCenterRight;
        public Button buttonMap3ControllersCenterDownRight;
        public Button buttonMap3ControllersCenterDown;
        public Button buttonMap3ControllersCenterDownLeft;
        public Button buttonMap3ControllersCenterLeft;
        public Button buttonMap3ControllersCenterUpLeft;





        // Controllers - Angle - Left
        public RadioButton radioButtonMap3ControllersAngle0;
        public RadioButton radioButtonMap3ControllersAngle16384;
        public RadioButton radioButtonMap3ControllersAngle32768;
        public RadioButton radioButtonMap3ControllersAngle49152;
        public RadioButton radioButtonMap3ControllersAngleCustom;
        public BetterTextbox textBoxMap3ControllersAngleCustom;

        // Controllers - Angle - Right
        public BetterTextbox textBoxMap3ControllersAngleChange;
        public Button buttonMap3ControllersAngleCCW;
        public Button buttonMap3ControllersAngleCW;



        // Data
        public Label labelMap3DataMapName;
        public Label labelMap3DataMapSubName;
        public Label labelMap3DataPuCoordinateValues;
        public Label labelMap3DataQpuCoordinateValues;
        public Label labelMap3DataId;
        public Label labelMap3DataYNorm;
    }
}