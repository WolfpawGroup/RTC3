﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using BizHawk.Client.Common;
using BizHawk.Emulation.Common;
using BizHawk.Client.EmuHawk;
using System.Windows.Forms;
using System.Threading;
using BizHawk.Emulation.Cores.Nintendo.N64;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.IO.Compression;

namespace RTC
{

    public static class RTC_MemoryDomains
    {
		
		public static volatile MemoryDomainRTCInterface MDRI = new MemoryDomainRTCInterface();
		public static volatile Dictionary<string,MemoryInterface> MemoryInterfaces = new Dictionary<string, MemoryInterface>();
        public static volatile Dictionary<string, MemoryInterface> VmdPool = new Dictionary<string, MemoryInterface>();

        public static string MainDomain = null;
		public static bool BigEndian { get; set; }
		public static int DataSize { get; set; }
		public static WatchSize WatchSize
		{
			get
			{
				return (WatchSize)DataSize;
			}
		}

		public static string[] SelectedDomains = new string[] { };
		public static string[] lastSelectedDomains = new string[] { };


        public static string[] getSelectedDomains()
        {
            return SelectedDomains;
        }

        public static void UpdateSelectedDomains(string[] _domains, bool sync = false)
        {

			SelectedDomains = _domains;
			lastSelectedDomains = _domains;

			if (RTC_Core.isStandalone)
				RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_DOMAIN_SETSELECTEDDOMAINS) { objectValue = SelectedDomains }, sync);

			Console.WriteLine($"{RTC_Core.RemoteRTC?.expectedSide} -> Selected {_domains.Count().ToString()} domains \n{string.Join(" | ", _domains)}");

		}

		public static void ClearSelectedDomains()
		{
			lastSelectedDomains = SelectedDomains;

			SelectedDomains = new string[] { };

			if (RTC_Core.isStandalone)
				RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_DOMAIN_SETSELECTEDDOMAINS) { objectValue = SelectedDomains });

			Console.WriteLine($"{RTC_Core.RemoteRTC?.expectedSide} -> Cleared selected domains");
		}

        public static string[] GetBlacklistedDomains()
        {
            // Returns the list of Domains that can't be rewinded.

            List<string> DomainBlacklist = new List<string>();

			string SystemName;
			
			if(RTC_Core.isStandalone)
				SystemName = (string)RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_DOMAIN_SYSTEM), true);
			else
				SystemName = Global.Game.System.ToString().ToUpper();

			switch (SystemName)
            {

                case "NES":     //Nintendo Entertainment system

                    DomainBlacklist.Add("System Bus");
                    DomainBlacklist.Add("PRG ROM");
					DomainBlacklist.Add("PALRAM"); //Color Memory (Useless and disgusting)
					DomainBlacklist.Add("CHR VROM"); //Cartridge
                    DomainBlacklist.Add("Battery RAM"); //Cartridge Save Data
                    break;


                case "GB":      //Gameboy
                case "GBC":     //Gameboy Color
                    DomainBlacklist.Add("ROM"); //Cartridge
                    DomainBlacklist.Add("System Bus");
                    break;

                case "SNES":    //Super Nintendo

                    DomainBlacklist.Add("CARTROM"); //Cartridge
                    DomainBlacklist.Add("CARTRAM"); //Cartridge Save data
                    DomainBlacklist.Add("APURAM"); //SPC700 memory
                    DomainBlacklist.Add("CGRAM"); //Color Memory (Useless and disgusting)
					DomainBlacklist.Add("System Bus"); // maxvalue is not representative of chip (goes ridiculously high)
					break;

                case "N64":     //Nintendo 64
                    DomainBlacklist.Add("System Bus");
                    DomainBlacklist.Add("PI Register");
                    DomainBlacklist.Add("EEPROM");
                    DomainBlacklist.Add("ROM");
                    DomainBlacklist.Add("SI Register");
					DomainBlacklist.Add("VI Register");
					DomainBlacklist.Add("RI Register");
					DomainBlacklist.Add("AI Register");
					break;

                case "PCE":     //PC Engine / Turbo Grafx
                    DomainBlacklist.Add("ROM");
                    break;


                case "GBA":     //Gameboy Advance
                    DomainBlacklist.Add("OAM");
                    DomainBlacklist.Add("BIOS");
                    DomainBlacklist.Add("PALRAM");
                    DomainBlacklist.Add("ROM");
                    break;

                case "SG":      //Sega SG-1000
                    //everything okay
                    break;

                case "SMS":     //Sega Master System
                    DomainBlacklist.Add("System Bus"); // the game cartridge appears to be on the system bus
                    break;

                case "GG":      //Sega GameGear
                    //everything okay
                    break;

                case "GEN":     //Sega Genesis and CD
                    DomainBlacklist.Add("MD CART");
                    break;


                case "PSX":     //Sony Playstation 1
                    DomainBlacklist.Add("MainRAM");
                    DomainBlacklist.Add("BiosROM");
                    DomainBlacklist.Add("PIOMem");
                    break;

                case "A26":     //Atari 2600
                    break;

                case "A78":     //Atari 7800
                    DomainBlacklist.Add("BIOS ROM");
                    DomainBlacklist.Add("HSC ROM");
                    break;

                case "LYNX":    //Atari Lynx
                    DomainBlacklist.Add("Save RAM");
                    DomainBlacklist.Add("Cart B");
                    DomainBlacklist.Add("Cart A");
                    break;


                case "INTV":    //Intellivision

                case "PCECD":   //related to PC-Engine / Turbo Grafx
                case "SGX":     //related to PC-Engine / Turbo Grafx
                case "TI83":    //Ti-83 Calculator
                case "WSWAN":   //Wonderswan
                case "C64":     //Commodore 64
                case "Coleco":  //Colecovision
                case "SGB":     //Super Gameboy
                case "SAT":     //Sega Saturn
                case "DGB": 
                    MessageBox.Show("WARNING: The selected system appears to be supported by Bizhawk Emulator.\n " +
                    "However, no corruption Template is available yet for this system.\n " +
                    "You'll have to manually select the Memory Domains to corrupt.");
                    break;

                    //TODO: Add more domains for cores like gamegear, atari, turbo graphx
            }


			return DomainBlacklist.ToArray();

        }


        public static void RefreshDomains(bool clearSelected = true)
        {

			if (Global.Emulator is NullEmulator)
				return;

			object[] returns;

			if (!RTC_Core.isStandalone)
				returns = (object[])getInterfaces();
			else
				returns = (object[])RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_DOMAIN_GETDOMAINS), true);

			if (returns == null)
			{
				Console.WriteLine($"{RTC_Core.RemoteRTC.expectedSide} -> RefreshDomains() FAILED");
				return;
			}

			if(clearSelected)
				ClearSelectedDomains();

			if (RTC_Core.isStandalone)
			{
				MemoryInterfaces.Clear();

				foreach (MemoryInterface mi in (MemoryInterface[])returns[0])
					MemoryInterfaces.Add(mi.ToString(), mi);

				MainDomain = (string)returns[1];
				DataSize = MemoryInterfaces[MainDomain].WordSize;
				BigEndian = MemoryInterfaces[MainDomain].BigEndian;

			}

		}

		public static object getInterfaces()
		{
			Console.WriteLine($"{RTC_Core.RemoteRTC?.expectedSide.ToString()} -> getInterfaces()");

			MemoryInterfaces.Clear();

			ServiceInjector.UpdateServices(Global.Emulator.ServiceProvider, MDRI);

			foreach (MemoryDomain _domain in MDRI.MemoryDomains)
				MemoryInterfaces.Add(_domain.ToString(), new MemoryDomainProxy(_domain));

			MainDomain = MDRI.MemoryDomains.MainMemory.ToString();
			DataSize = (MemoryInterfaces[MainDomain] as MemoryDomainProxy).md.WordSize;
			BigEndian = (MemoryInterfaces[MainDomain] as MemoryDomainProxy).md.EndianType == MemoryDomain.Endian.Big;

			//RefreshDomains();

            /*
            if(VmdPool.Count > 0)
                foreach (string VmdKey in VmdPool.Keys)
                    MemoryInterfaces.Add(VmdKey, VmdPool[VmdKey]);
            */

            return new object[] { MemoryInterfaces.Values.ToArray(), MainDomain };
		}

        public static void Clear()
        {

            MemoryInterfaces.Clear();

			lastSelectedDomains = SelectedDomains;
			SelectedDomains = new string[] { };

			if(RTC_Core.coreForm != null)
				RTC_Core.coreForm.lbMemoryDomains.Items.Clear();

        }

        public static MemoryDomainProxy getProxy(string _domain, long _address)
        {
			if (MemoryInterfaces.Count == 0)
				RefreshDomains();

            if (MemoryInterfaces.ContainsKey(_domain))
            {
                MemoryInterface mi = MemoryInterfaces[_domain];
                return (MemoryDomainProxy)mi;
            }
            else if(VmdPool.ContainsKey(_domain))
            {
                MemoryInterface mi = VmdPool[_domain];
                var vmd = (mi as VirtualMemoryDomain);
                return getProxy(vmd.getRealDomain(_address), vmd.getRealAddress(_address));
            }
            else
                return null;
        }

        public static MemoryInterface getInterface(string _domain)
        {
			if (MemoryInterfaces.Count == 0)
				RefreshDomains();

            if (MemoryInterfaces.ContainsKey(_domain))
				return MemoryInterfaces[_domain];

            if (VmdPool.ContainsKey(_domain))
                return VmdPool[_domain];

			return null;
        }

		public static void UnFreezeAddress(long address)
		{
			if (address >= 0)
			{
				// TODO: can't unfreeze address 0??
				Global.CheatList.RemoveRange(
					Global.CheatList.Where(x => x.Contains(address)).ToList());
			}

		}

		public static void FreezeAddress(long address, string freezename = "")
		{
			if (address >= 0)
			{
				var watch = Watch.GenerateWatch(
					getProxy(MainDomain, address).md,
					address,
					WatchSize,
					BizHawk.Client.Common.DisplayType.Hex,
					BigEndian,
					//RTC_HIJACK : change string.empty to freezename
					freezename);

				Global.CheatList.Add(new Cheat(
					watch,
					watch.Value));
			}
		}

        public static long getRealAddress(string domain, long address)
        {
            if (domain.Contains("[V]"))
            {
                MemoryInterface mi = VmdPool[domain];
                var vmd = (mi as VirtualMemoryDomain);
                return vmd.getRealAddress(address);
            }
            else
                return address;
        }

        public static string getRealDomain(string domain, long address)
        {
            if (domain.Contains("[V]"))
            {
                MemoryInterface mi = VmdPool[domain];
                var vmd = (mi as VirtualMemoryDomain);
                return vmd.getRealDomain(address);
            }
            else
                return domain;
        }

        public static void GenerateVmdFromStashkey(StashKey sk)
        {
            var proto = new VmdPrototype(sk.BlastLayer);
            AddVMD(proto);

            RTC_Core.vmdPoolForm.RefreshVMDs();
        }

        public static void AddVMD(VmdPrototype proto) => AddVMD(proto.Generate());
        public static void AddVMD(VirtualMemoryDomain VMD)
        {
            RTC_MemoryDomains.VmdPool[VMD.ToString()] = VMD;

            if (RTC_Core.isStandalone)
            {
                RTC_RPC.SendToKillSwitch("FREEZE");
                RTC_NetCore.HugeOperationStart();
                RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_DOMAIN_VMD_ADD) { objectValue = VMD.proto }, true);
                RTC_RPC.SendToKillSwitch("UNFREEZE");
                RTC_NetCore.HugeOperationEnd();
            }

            if(!RTC_Hooks.isRemoteRTC)
                RTC_Core.coreForm.RefreshDomainsAndKeepSelected();
        }

        public static void RemoveVMD(VirtualMemoryDomain VMD) => RemoveVMD(VMD.ToString());
        public static void RemoveVMD(string vmdName)
        {
            if (RTC_MemoryDomains.VmdPool.ContainsKey(vmdName))
            {
                RTC_MemoryDomains.VmdPool.Remove(vmdName);
                RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_DOMAIN_VMD_REMOVE) { objectValue = vmdName }, true);
            }

            if (!RTC_Hooks.isRemoteRTC)
                RTC_Core.coreForm.RefreshDomainsAndKeepSelected();
        }

        public static void RenameVMD(VirtualMemoryDomain VMD) => RenameVMD(VMD.ToString());
        public static void RenameVMD(string vmdName)
        {
            if (!RTC_MemoryDomains.VmdPool.ContainsKey(vmdName))
                return;

            RTC_Core.StopSound();
            string Name = "";
            string value = "";
                if (RTC_Extensions.getInputBox("BlastLayer to VMD", "Enter the new VMD name:", ref value) == DialogResult.OK)
                {
                    Name = value.Trim();
                    RTC_Core.StartSound();
                }
                else
                {
                    RTC_Core.StartSound();
                    return;
                }

            if (string.IsNullOrWhiteSpace(Name))
                Name = RTC_Core.GetRandomKey();

            VirtualMemoryDomain VMD = (VirtualMemoryDomain)RTC_MemoryDomains.VmdPool[vmdName];

            RemoveVMD(VMD);
            VMD.name = Name;
            VMD.proto.VmdName = Name;
            AddVMD(VMD);

        }

        public static byte[] getDomainData(string domain)
        {
            MemoryInterface mi;

            if (domain.Contains("[V]"))
            {
                mi = MemoryInterfaces[domain];
            }
            else
            {
                mi = VmdPool[domain];
            }

            return mi.getDump();
        }
    }

    [Serializable()]
    public abstract class MemoryInterface
    {
        public abstract long Size { get; set; }
        public int WordSize { get; set; }
        public string name { get; set; }
        public bool BigEndian { get; set; }

        public abstract byte[] getDump();
        public abstract byte[] PeekBytes(long startAddress, long endAddress);
        public abstract byte PeekByte(long address);
        public abstract void PokeByte(long address, byte value);

        
    }

    [XmlInclude(typeof(BlastLayer))]
    [XmlInclude(typeof(BlastCheat))]
    [XmlInclude(typeof(BlastByte))]
    [XmlInclude(typeof(BlastPipe))]
    [XmlInclude(typeof(BlastVector))]
    [XmlInclude(typeof(BlastUnit))]
    [Serializable()]
    public class VmdPrototype
    {
        public string VmdName { get; set; }
        public string GenDomain { get; set; }
        public bool BigEndian { get; set; }
        public int WordSize { get; set; }
        public int PointerSpacer { get; set; }

        public List<int> addSingles = new List<int>();
        public List<int> removeSingles = new List<int>();

        public List<int[]> addRanges = new List<int[]>();
        public List<int[]> removeRanges = new List<int[]>();

        public BlastLayer SuppliedBlastLayer = null;

        public VmdPrototype()
        {
        }

        public VmdPrototype(BlastLayer bl)
        {
            VmdName = RTC_Core.GetRandomKey();
            GenDomain = "Hybrid";

            BlastUnit bu = bl.Layer[0];
            var mi = RTC_MemoryDomains.getInterface(bu.Domain);
            BigEndian = mi.BigEndian;
            WordSize = mi.WordSize;
            SuppliedBlastLayer = bl;
        }

        public VirtualMemoryDomain Generate()
        {
            VirtualMemoryDomain VMD = new VirtualMemoryDomain();

            VMD.proto = this;
            VMD.name = VmdName;
            VMD.BigEndian = BigEndian;
            VMD.WordSize = WordSize;

            if (SuppliedBlastLayer != null)
            {
                VMD.AddFromBlastLayer(SuppliedBlastLayer);
                return VMD;
            }

            int addressCount = 0;

            foreach (int[] range in addRanges)
            {
                int start = range[0];
                int end = range[1];

                for (int i = start; i < end; i++)
                {
                    if (!isAddressInRanges(i, removeSingles, removeRanges))
                        if (PointerSpacer == 1 || addressCount % PointerSpacer == 0)
                        {
                            //VMD.MemoryPointers.Add(new Tuple<string, long>(Domain, i));
                            VMD.PointerDomains.Add(GenDomain);
                            VMD.PointerAddresses.Add(i);
                        }
                    addressCount++;
                }
            }

            foreach (int single in addSingles)
            {
                //VMD.MemoryPointers.Add(new Tuple<string, long>(Domain, single));
                VMD.PointerDomains.Add(GenDomain);
                VMD.PointerAddresses.Add(single);
                addressCount++;
            }

            return VMD;
        }

        public bool isAddressInRanges(int Address, List<int> Singles, List<int[]> Ranges)
        {
            if (Singles.Contains(Address))
                return true;

            foreach (int[] range in Ranges)
            {
                int start = range[0];
                int end = range[1];

                if (Address >= start && Address < end)
                    return true;
            }

            return false;
        }

    }

    
    [Serializable()]
    public class VirtualMemoryDomain : MemoryInterface
    {
        //public List<MemoryPointer> MemoryPointers = new List<MemoryPointer>();
        //public List<Tuple<string, long>> MemoryPointers = new List<Tuple<string, long>>();
        public List<string> PointerDomains = new List<string>();
        public List<long> PointerAddresses = new List<long>();
        public VmdPrototype proto;

        public override long Size { get { return PointerDomains.Count; } set { } }

        public void AddFromBlastLayer(BlastLayer bl)
        {
            if (bl == null)
                return;

            foreach(BlastUnit bu in bl.Layer)
            {
                //MemoryPointers.Add(new MemoryPointer(bu.Domain, bu.Address));
                //MemoryPointers.Add(new Tuple<string, long>(bu.Domain, bu.Address));
                PointerDomains.Add(bu.Domain);
                PointerAddresses.Add(bu.Address);
            }
        }

        public string getRealDomain(long address)
        {
            if (address < 0 || address >= PointerDomains.Count)
                return null;

            return PointerDomains[(int)address];
        }

        public long getRealAddress(long address)
        {
            if (address < 0 || address >= PointerAddresses.Count)
                return 0;

            return PointerAddresses[(int)address];
        }

        public byte[] ToData()
        {
            VirtualMemoryDomain VMD = this;

            using (MemoryStream serialized = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(serialized, VMD);

                using (MemoryStream input = new MemoryStream(serialized.ToArray()))
                using (MemoryStream output = new MemoryStream())
                {

                    using (GZipStream zip = new GZipStream(output, CompressionMode.Compress))
                    {
                        input.CopyTo(zip);
                    }

                    return output.ToArray();
                }
            }
        }

        public static VirtualMemoryDomain FromData(byte[] data)
        {
            using (MemoryStream input = new MemoryStream(data))
            using (MemoryStream output = new MemoryStream())
            {
                using (GZipStream zip = new GZipStream(input, CompressionMode.Decompress))
                {
                    zip.CopyTo(output);
                }

                var binaryFormatter = new BinaryFormatter();

                using (MemoryStream serialized = new MemoryStream(output.ToArray()))
                {
                    VirtualMemoryDomain VMD = (VirtualMemoryDomain)binaryFormatter.Deserialize(serialized);
                    return VMD;
                }
            }

        }

        public override string ToString()
        {
            return "[V]" + name;
            //Virtual Memory Domains always start with [V]
        }

        public override byte[] getDump()
        {
            return PeekBytes(0, Size);
        }

        public override byte[] PeekBytes(long startAdress, long endAddress)
        {
            //endAddress is exclusive
            List<byte> data = new List<byte>();
            for (long i = startAdress; i < endAddress; i++)
                data.Add(PeekByte(i));

            return data.ToArray();
        }

        public override byte PeekByte(long address)
        {
            string targetDomain = getRealDomain(address);
            long targetAddress = getRealAddress(address);

            var mdp = RTC_MemoryDomains.getProxy(targetDomain, targetAddress);

            if(mdp == null)
                return 0;

            return mdp.PeekByte(targetAddress);
        }

        public override void PokeByte(long address, byte value)
        {
            string targetDomain = getRealDomain(address);
            long targetAddress = getRealAddress(address);

            var mdp = RTC_MemoryDomains.getProxy(targetDomain, targetAddress);

            if (mdp == null)
                return;

            mdp.PokeByte(targetAddress, value);
        }
    }

    /*
    [Serializable()]
    [XmlType("MP")]
    public class MemoryPointer
    {
        [XmlElement("D")]
        public string DomainData { get; set; }
        [XmlIgnore]
        public string Domain { get { return (IsEnabled ? DomainData : ""); }}
        [XmlElement("A")]
        public long AddressData { get; set; }
        [XmlIgnore]
        public long Address { get { return (IsEnabled ? AddressData : 0); } }
        [XmlElement("E")]
        public bool IsEnabled { get; set; }

        public MemoryPointer(string _domain, long _address)
        {
            DomainData = _domain;
            AddressData = _address;
            IsEnabled = true;
        }

        public MemoryPointer()
        {
        }

        public override string ToString()
        {
            return $"[{(IsEnabled ? "x" : " ")}] {DomainData}({AddressData})";

            //Gives: [x] ROM(123)
        }

        

    }
    */

    [Serializable()]
	public class MemoryDomainProxy : MemoryInterface
    {

		[NonSerialized]
		public MemoryDomain md = null;

		//public long Size;
		//public int WordSize;
		//public string name;
		//public bool BigEndian;

        public override long Size { get; set; }

		public MemoryDomainProxy(MemoryDomain _md)
		{
			md = _md;
			Size = md.Size;

            name = md.ToString();

            if (Global.Emulator is N64 && !(Global.Emulator as N64).UsingExpansionSlot && name == "RDRAM")
				Size = Size / 2;

			WordSize = md.WordSize;
			name = md.ToString();
			BigEndian = _md.EndianType == MemoryDomain.Endian.Big;
		}

		public override string ToString()
		{
			return name;
		}

		public void Detach()
		{
			md = null;
		}

		public void Reattach()
		{
			md = RTC_MemoryDomains.MDRI.MemoryDomains.FirstOrDefault(it => it.ToString() == name);
			Size = md.Size;
			WordSize = md.WordSize;
			name = md.ToString();
			BigEndian = md.EndianType == MemoryDomain.Endian.Big;
		}

        public override byte[] getDump()
        {
            return PeekBytes(0, Size);
        }

        public override byte[] PeekBytes(long startAdress, long endAddress)
        {
            //endAddress is exclusive
            List<byte> data = new List<byte>();
            for (long i = startAdress; i < endAddress; i++)
                data.Add(PeekByte(i));

            return data.ToArray();
        }


        public override byte PeekByte(long address)
		{
			if (md == null)
				return (byte)RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_DOMAIN_PEEKBYTE) { objectValue = new object[] { name, address } }, true);
			else
				return md.PeekByte(address);
		}

		public override void PokeByte(long address, byte value)
		{
			if (md == null)
				RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_DOMAIN_POKEBYTE) { objectValue = new object[] { name, address, value } });
			else
				md.PokeByte(address, value);
		}
	}

	public class MemoryDomainRTCInterface
	{
		[RequiredService]
		public IMemoryDomains MemoryDomains { get; set; }

		[RequiredService]
		private IEmulator Emulator { get; set; }

	}
}
