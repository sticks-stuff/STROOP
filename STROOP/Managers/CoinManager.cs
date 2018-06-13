﻿using STROOP.Controls;
using STROOP.Models;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace STROOP.Managers
{
    public class CoinManager
    {
        private readonly DataGridView _dataGridViewCoin;

        private readonly ListBox _listBoxCoinObjects;

        private readonly BetterTextbox _textBoxCoinHSpeedScale;
        private readonly BetterTextbox _textBoxCoinVSpeedScale;
        private readonly BetterTextbox _textBoxCoinVSpeedOffset;
        private readonly BetterTextbox _textBoxCoinParamOrder;

        private readonly Label _labelCoinHSpeedRange;
        private readonly Label _labelCoinVSpeedRange;

        private readonly BetterTextbox _textBoxCoinFilterHSpeedMin;
        private readonly BetterTextbox _textBoxCoinFilterHSpeedMax;
        private readonly BetterTextbox _textBoxCoinFilterVSpeedMin;
        private readonly BetterTextbox _textBoxCoinFilterVSpeedMax;
        private readonly BetterTextbox _textBoxCoinFilterAngleMin;
        private readonly BetterTextbox _textBoxCoinFilterAngleMax;

        private readonly BetterTextbox _textBoxCoinStartingRng;

        private readonly Button _buttonCoinClear;
        private readonly Button _buttonCoinCalculate;

        public CoinManager(TabPage tabControl)
        {
            // set controls

            SplitContainer splitContainerCoin = tabControl.Controls["splitContainerCoin"] as SplitContainer;

            _dataGridViewCoin = splitContainerCoin.Panel2.Controls["dataGridViewCoin"] as DataGridView;

            _listBoxCoinObjects = splitContainerCoin.Panel1.Controls["listBoxCoinObjects"] as ListBox;

            _textBoxCoinHSpeedScale = splitContainerCoin.Panel1.Controls["textBoxCoinHSpeedScale"] as BetterTextbox;
            _textBoxCoinVSpeedScale = splitContainerCoin.Panel1.Controls["textBoxCoinVSpeedScale"] as BetterTextbox;
            _textBoxCoinVSpeedOffset = splitContainerCoin.Panel1.Controls["textBoxCoinVSpeedOffset"] as BetterTextbox;
            _textBoxCoinParamOrder = splitContainerCoin.Panel1.Controls["textBoxCoinParamOrder"] as BetterTextbox;

            _labelCoinHSpeedRange = splitContainerCoin.Panel1.Controls["labelCoinHSpeedRange"] as Label;
            _labelCoinVSpeedRange = splitContainerCoin.Panel1.Controls["labelCoinVSpeedRange"] as Label;

            GroupBox groupBoxCoinFilter = splitContainerCoin.Panel1.Controls["groupBoxCoinFilter"] as GroupBox;
            _textBoxCoinFilterHSpeedMin = groupBoxCoinFilter.Controls["textBoxCoinFilterHSpeedMin"] as BetterTextbox;
            _textBoxCoinFilterHSpeedMax = groupBoxCoinFilter.Controls["textBoxCoinFilterHSpeedMax"] as BetterTextbox;
            _textBoxCoinFilterVSpeedMin = groupBoxCoinFilter.Controls["textBoxCoinFilterVSpeedMin"] as BetterTextbox;
            _textBoxCoinFilterVSpeedMax = groupBoxCoinFilter.Controls["textBoxCoinFilterVSpeedMax"] as BetterTextbox;
            _textBoxCoinFilterAngleMin = groupBoxCoinFilter.Controls["textBoxCoinFilterAngleMin"] as BetterTextbox;
            _textBoxCoinFilterAngleMax = groupBoxCoinFilter.Controls["textBoxCoinFilterAngleMax"] as BetterTextbox;

            _textBoxCoinStartingRng = splitContainerCoin.Panel1.Controls["textBoxCoinStartingRng"] as BetterTextbox;

            _buttonCoinClear = splitContainerCoin.Panel1.Controls["buttonCoinClear"] as Button;
            _buttonCoinCalculate = splitContainerCoin.Panel1.Controls["buttonCoinCalculate"] as Button;

            // initialize controls

            _listBoxCoinObjects.DataSource = CoinObject.GetCoinObjects();
            _listBoxCoinObjects.ClearSelected();
            _listBoxCoinObjects.SelectedValueChanged += (sender, e) => ListBoxSelectionChange();

            _buttonCoinCalculate.Click += (sender, e) => CalculateCoinTrajectories();

        }

        private void ListBoxSelectionChange()
        {
            CoinObject coinObject = _listBoxCoinObjects.SelectedItem as CoinObject;
            _textBoxCoinHSpeedScale.Text = coinObject.HSpeedScale.ToString();
            _textBoxCoinVSpeedScale.Text = coinObject.VSpeedScale.ToString();
            _textBoxCoinVSpeedOffset.Text = coinObject.VSpeedOffset.ToString();
        }

        private void CalculateCoinTrajectories()
        {
            List<CoinTrajectory> coinTrajectories = Enumerable.Range(0, 11).ToList().ConvertAll(
                num => CoinObject.Bobomb.CalculateCoinTrajectory(num));


        }

        public void Update(bool updateView)
        {
            if (!updateView) return;

            double? hSpeedScaleNullable = ParsingUtilities.ParseDoubleNullable(_textBoxCoinHSpeedScale.Text);
            if (hSpeedScaleNullable.HasValue)
            {
                double hSpeedScale = hSpeedScaleNullable.Value;
                double hSpeedMin = 0;
                double hSpeedMax = hSpeedScale;
                _labelCoinHSpeedRange.Text = String.Format("HSpeed Range: [{0},{1})", hSpeedMin, hSpeedMax);
            }
            else
            {
                _labelCoinHSpeedRange.Text = "HSpeed Range:";
            }

            double? vSpeedScaleNullable = ParsingUtilities.ParseDoubleNullable(_textBoxCoinVSpeedScale.Text);
            double? vSpeedOffsetNullable = ParsingUtilities.ParseDoubleNullable(_textBoxCoinVSpeedOffset.Text);
            if (vSpeedScaleNullable.HasValue && vSpeedOffsetNullable.HasValue)
            {
                double vSpeedScale = vSpeedScaleNullable.Value;
                double vSpeedOffset = vSpeedOffsetNullable.Value;
                double vSpeedMin = vSpeedOffset;
                double vSpeedMax = vSpeedScale + vSpeedOffset;
                _labelCoinVSpeedRange.Text = String.Format("VSpeed Range: [{0},{1})", vSpeedMin, vSpeedMax);
            }
            else
            {
                _labelCoinVSpeedRange.Text = "VSpeed Range:";
            }
        }
    }
}
