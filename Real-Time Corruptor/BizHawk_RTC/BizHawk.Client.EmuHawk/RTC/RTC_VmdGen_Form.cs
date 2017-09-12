﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RTC
{
    public partial class RTC_VmdGen_Form : Form
    {

        long currentDomainSize = 0;

        public RTC_VmdGen_Form()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            this.AutoSize = false;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            RTC_Core.coreForm.RefreshDomainsAndKeepSelected();

            cbSelectedMemoryDomain.Items.Clear();
            cbSelectedMemoryDomain.Items.AddRange(RTC_MemoryDomains.MemoryInterfaces.Keys.Where(it => !it.Contains("[V]")).ToArray());

            cbSelectedMemoryDomain.SelectedIndex = 0;
        }

        private void cbSelectedEngine_SelectedIndexChanged(object sender, EventArgs e)
        {



            if (string.IsNullOrWhiteSpace(cbSelectedMemoryDomain.SelectedItem?.ToString()) || !RTC_MemoryDomains.MemoryInterfaces.ContainsKey(cbSelectedMemoryDomain.SelectedItem.ToString()))
            {
                cbSelectedMemoryDomain.Items.Clear();
                return;
            }

            MemoryInterface mi = RTC_MemoryDomains.MemoryInterfaces[cbSelectedMemoryDomain.SelectedItem.ToString()];

            lbDomainSizeValue.Text = mi.Size.ToString();
            lbWordSizeValue.Text = $"{mi.WordSize*8} bits";
            lbEndianTypeValue.Text = (mi.BigEndian ? "Big" : "Little");

            currentDomainSize = Convert.ToInt64(mi.Size);
        }

        public int SafeStringToInt(string input)
        {
            if (input.ToUpper().Contains("0X"))
                return int.Parse(input.Substring(2), NumberStyles.HexNumber);
            else
                return Convert.ToInt32(input);
        }

        private void btnGenerateVMD_Click(object sender, EventArgs e) => GenerateVMD();
        private bool GenerateVMD()
        {
            if (string.IsNullOrWhiteSpace(cbSelectedMemoryDomain.SelectedItem?.ToString()) || !RTC_MemoryDomains.MemoryInterfaces.ContainsKey(cbSelectedMemoryDomain.SelectedItem.ToString()))
            {
                cbSelectedMemoryDomain.Items.Clear();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(tbVmdName.Text) && RTC_MemoryDomains.VmdPool.ContainsKey($"[V]{tbVmdName.Text}"))
            {
                MessageBox.Show("There is already a VMD with this name in the VMD Pool");
                return false;
            }

            MemoryInterface mi = RTC_MemoryDomains.MemoryInterfaces[cbSelectedMemoryDomain.SelectedItem.ToString()];
            VirtualMemoryDomain VMD = new VirtualMemoryDomain();
            VmdPrototype proto = new VmdPrototype();

            proto.GenDomain = cbSelectedMemoryDomain.SelectedItem.ToString();

            if (string.IsNullOrWhiteSpace(tbVmdName.Text))
                proto.VmdName = RTC_Core.GetRandomKey();
            else
                proto.VmdName = tbVmdName.Text;


            proto.BigEndian = mi.BigEndian;
            proto.WordSize = mi.WordSize;


            if (cbUsePointerSpacer.Checked && nmPointerSpacer.Value > 1)
                proto.PointerSpacer = Convert.ToInt32(nmPointerSpacer.Value);
            else
                proto.PointerSpacer = 1;


            foreach (string line in tbCustomAddresses.Lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string trimmedLine = line.Trim();

                bool remove = false;

                if (trimmedLine[0] == '-')
                {
                    remove = true;
                    trimmedLine = trimmedLine.Substring(1);
                }

                string[] lineParts = trimmedLine.Split('-');

                if (lineParts.Length > 1)
                {
                    int start = SafeStringToInt(lineParts[0]);
                    int end = SafeStringToInt(lineParts[1]);

                    if (end >= currentDomainSize)
                        end = Convert.ToInt32(currentDomainSize - 1);

                    if (remove)
                        proto.removeRanges.Add(new int[] { start, end });
                    else
                        proto.addRanges.Add(new int[] { start, end });
                    
                }
                else
                {
                    int address = SafeStringToInt(lineParts[0]);

                    if (address < currentDomainSize)
                    {
                        if (remove)
                            proto.removeSingles.Add(address);
                        else
                            proto.addSingles.Add(address);
                    }
                }


            }

            if (proto.addRanges.Count == 0 && proto.addSingles.Count == 0)
            {
                //No add range was specified, use entire domain
                proto.addRanges.Add(new int[] { 0, (currentDomainSize > int.MaxValue ? int.MaxValue : Convert.ToInt32(currentDomainSize)) });
            }


            VMD = proto.Generate();


            if (VMD.PointerAddresses.Count == 0)
            {
                MessageBox.Show("The resulting VMD had no pointers so the operation got cancelled.");
                return false;
            }

            RTC_MemoryDomains.AddVMD(VMD);
            
            tbVmdName.Text = "";
            cbSelectedMemoryDomain.SelectedIndex = -1;
            cbSelectedMemoryDomain.Items.Clear();

            currentDomainSize = 0;

            nmPointerSpacer.Value = 2;
            cbUsePointerSpacer.Checked = false;

            tbCustomAddresses.Text = "";

            lbDomainSizeValue.Text = "######";
            lbEndianTypeValue.Text = "######";
            lbWordSizeValue.Text = "######";

            //send to vmd pool menu
            RTC_Core.vmdPoolForm.RefreshVMDs();
            RTC_Core.coreForm.cbMemoryDomainTool.SelectedIndex = 1;

            GC.Collect();
            GC.WaitForPendingFinalizers();

            return true;
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
@"VMD Generator instructions help and examples
-----------------------------------------------
Adding an address range:
50-100
Adding a single address:
55

Removing an address range:
-60-110
Removing a single address:
-66

> If no initial range is specified,
the removals will be done on the entire range.

> Ranges are exclusive, meaning that the last
address is excluded from the range.

> Single added addresses will bypass removal ranges

> Single addresses aren't affected by the
pointer spacer parameter

> add 0x in front of addresses to use Hexadecimal");
        }
    }
}
