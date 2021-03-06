﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MDStudio.Properties;

namespace MDStudio
{
    public partial class ProfilerView : Form
    {
        const int s_maxResults = 100;

        private MainForm m_mainForm;

        enum Columns
        {
            Address,
            HitCount,
            CyclesPerInstr,
            TotalCycles,
            PercentageCost,
            Filename,
            Line
        };

        public ProfilerView(MainForm mainForm)
        {
            m_mainForm = mainForm;
            InitializeComponent();
        }

        public void SetResults(List<MainForm.ProfilerEntry> results)
        {
            dataGrid.DataSource = results;
            dataGrid.Columns[(int)Columns.Address].HeaderText = "Address";
            dataGrid.Columns[(int)Columns.HitCount].HeaderText = "Hit Count";
            dataGrid.Columns[(int)Columns.CyclesPerInstr].HeaderText = "Cycles Per Hit";
            dataGrid.Columns[(int)Columns.TotalCycles].HeaderText = "Total Cycles";
            dataGrid.Columns[(int)Columns.PercentageCost].HeaderText = "Cost %";
            dataGrid.Columns[(int)Columns.Filename].HeaderText = "Filename";
            dataGrid.Columns[(int)Columns.Line].HeaderText = "Line";

            dataGrid.Columns[(int)Columns.Address].DefaultCellStyle.Format = "X08";
            dataGrid.Columns[(int)Columns.PercentageCost].DefaultCellStyle.Format = "P4";
        }

        private void dataGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(dataGrid.SelectedCells.Count > 0)
            {
                string filename = (string)dataGrid.Rows[dataGrid.SelectedCells[0].RowIndex].Cells[(int)Columns.Filename].Value;
                int line = (int)dataGrid.Rows[dataGrid.SelectedCells[0].RowIndex].Cells[(int)Columns.Line].Value;
                m_mainForm.GoTo(filename, line);
            }
        }

        private void ProfilerView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void ProfilerView_VisibleChanged(object sender, EventArgs e)
        {
            m_mainForm.UpdateViewProfiler(Visible);
        }

        private void ProfilerView_Load(object sender, EventArgs e)
        {
            if (Settings.Default.ProfilerWindowPosition != null)
            {
                this.Location = Settings.Default.ProfilerWindowPosition;
            }

            if (Settings.Default.ProfilerWindowSize != null)
            {
                this.Size = Settings.Default.ProfilerWindowSize;
            }
        }

        private void ProfilerView_Move(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                Settings.Default.ProfilerWindowPosition = this.Location;
            }
        }

        private void ProfilerView_Resize(object sender, EventArgs e)
        {
            Settings.Default.ProfilerWindowSize = this.Size;
        }
    }
}
