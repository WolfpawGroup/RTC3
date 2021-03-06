﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizHawk.Client.Common;
using BizHawk.Emulation.Common;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using BizHawk.Client.EmuHawk;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace RTC
{
    public static class RTC_FreezeEngine
    {

        //The freeze engine is very similar to the Hellgenie and shares common functions with it. See RTC_HellgenieEngine.cs for cheat-related methods.
        
        public static BlastCheat GenerateUnit(string _domain, long _address)
        {
            try
            {
                MemoryDomainProxy mdp = RTC_MemoryDomains.getProxy(_domain, _address);
                BizHawk.Client.Common.DisplayType _displaytype = BizHawk.Client.Common.DisplayType.Unsigned;

                byte[] _value;
                if (RTC_Core.CustomPrecision == -1)
                    _value = new byte[mdp.WordSize];
                else
                    _value = new byte[RTC_Core.CustomPrecision];

                long safeAddress = _address - (_address % _value.Length);

                for (int i = 0; i < _value.Length; i++)
                    _value[i] = 0;
                    //_value[i] = mdp.PeekByte(safeAddress + i);

                return new BlastCheat(_domain, safeAddress, _displaytype, mdp.BigEndian, _value, true, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong in the RTC Freeze Engine. \n" +
                                "This is not a BizHawk error so you should probably send a screenshot of this to the devs\n\n" +
                                ex.ToString());
                return null;
            }
        }


    }
}
