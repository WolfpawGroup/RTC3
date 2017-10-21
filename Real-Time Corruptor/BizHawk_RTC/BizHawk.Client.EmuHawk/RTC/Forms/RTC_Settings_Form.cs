﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTC
{
    public partial class RTC_Settings_Form : Form
    {
        public RTC_Settings_Form()
        {
            InitializeComponent();
        }

        private void nmGameProtectionDelay_ValueChanged(object sender, KeyPressEventArgs e) => UpdateGameProtectionDelay();
        private void nmGameProtectionDelay_ValueChanged(object sender, KeyEventArgs e) => UpdateGameProtectionDelay();
        private void nmGameProtectionDelay_ValueChanged(object sender, EventArgs e) => UpdateGameProtectionDelay();
        public void UpdateGameProtectionDelay()
        {
            RTC_GameProtection.BackupInterval = Convert.ToInt32(RTC_Core.sForm.nmGameProtectionDelay.Value);
            if (RTC_GameProtection.isRunning)
                RTC_GameProtection.Reset();

        }

        private void cbCrashSoundEffect_SelectedIndexChanged(object sender, EventArgs e)
        {
            RTC_NetCoreSettings.rotateRandomSounds = false;
            RTC_NetCoreSettings.isClassicCrash = false;

            switch (cbCrashSoundEffect.SelectedIndex)
            {
                case 0:
                    RTC_NetCoreSettings.isClassicCrash = true;
                    RTC_NetCoreSettings.simpleSound = new SoundPlayer("RTC\\ASSETS\\crash.wav");
                    break;
                case 1:
                    RTC_NetCoreSettings.simpleSound = new SoundPlayer("RTC\\ASSETS\\quack.wav");
                    break;

                case 2:
                    RTC_NetCoreSettings.simpleSound = null;
                    break;
                case 3:
                    RTC_NetCoreSettings.rotateRandomSounds = true;
                    break;
            }

        }

        public void cbNetCoreCommandTimeout_SelectedIndexChanged(object sender, EventArgs e)
        {
            string setting = cbNetCoreCommandTimeout.SelectedItem.ToString().ToUpper();
            RTC_NetCoreSettings.changeNetCoreSettings(setting);
            RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.AGGRESSIVENESS) { objectValue = setting }, true);

        }

        private void btnOpenOnlineWiki_Click(object sender, EventArgs e)
        {
            Process.Start("https://corrupt.wiki/");
        }

        private void btnChangeRTCColor_Click(object sender, EventArgs e)
        {
            RTC_Core.SelectRTCColor();
        }

        private void btnStartAutoKillSwitch_Click(object sender, EventArgs e)
        {
            Process.Start("AutoKillSwitch.exe");
        }

        private void btnRtcFactoryClean_Click(object sender, EventArgs e)
        {
            Process.Start($"FactoryClean{(RTC_Core.isStandalone ? "DETACHED" : "ATTACHED")}.bat");
        }

        private void RTC_Settings_Form_Load(object sender, EventArgs e)
        {
            if(RTC_Core.isStandalone)
            {
                pnAttachedModeSettings.Controls.Clear();
                lbAttachedModeSettings.ForeColor = Color.Gray;
            }
            else
            {
                pnDetachedModeSettings.Controls.Clear();
                lbDetachedModeSettings.ForeColor = Color.Gray;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RTC_Core.coreForm.showPanelForm(RTC_Core.coreForm.previousForm, false);
        }

        private void cbDisableBizhawkOSD_CheckedChanged(object sender, EventArgs e)
        {

            if (cbDisableBizhawkOSD.Checked)
                RTC_Params.RemoveParam("ENABLE_BIZHAWK_OSD");
            else
                RTC_Params.SetParam("ENABLE_BIZHAWK_OSD");

            RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.BIZHAWK_SET_OSDDISABLED) { objectValue = cbDisableBizhawkOSD.Checked });
        }

        private void cbAllowCrossCoreCorruption_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAllowCrossCoreCorruption.Checked)
                RTC_Params.SetParam("ALLOW_CROSS_CORE_CORRUPTION");
            else
                RTC_Params.RemoveParam("ALLOW_CROSS_CORE_CORRUPTION");

            RTC_Core.AllowCrossCoreCorruption = cbAllowCrossCoreCorruption.Checked;
        }

        private void btnImportKeyBindings_Click(object sender, EventArgs e)
        {

            try
            {
                RTC_Core.StopSound();

                if (RTC_Core.bizhawkDir.Contains("\\VERSIONS\\"))
                {
                    var bizhawkFolder = new DirectoryInfo(RTC_Core.bizhawkDir);
                    var LauncherVersFolder = bizhawkFolder.Parent.Parent;

                    var versions = LauncherVersFolder.GetDirectories().Reverse().ToArray();

                    var prevVersion = versions[1].Name;

                    var dr = MessageBox.Show(
                        "RTC Launcher detected,\n" +
                        $"Do you want to import Controller/Hotkey bindings from version {prevVersion}"
                        , $"Import config from previous version ?", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (dr == DialogResult.Yes)
                        Stockpile.LoadBizhawkKeyBindsFromIni(versions[1].FullName + "\\BizHawk\\config.ini");
                    else
                        Stockpile.LoadBizhawkKeyBindsFromIni();
                }
                else
                    Stockpile.LoadBizhawkKeyBindsFromIni();
            }
            finally
            {
                RTC_Core.StartSound();
            }

        }
    }
}
