﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Extensions;
using System.Reflection;
using SM64_Diagnostic.Managers;
using SM64_Diagnostic.Structs.Configurations;

namespace SM64_Diagnostic.Controls
{
    public class AddressHolder
    {
        public readonly uint? OffsetUS;
        public readonly uint? OffsetJP;
        public readonly uint? OffsetPAL;
        public readonly uint? OffsetDefault;

        public readonly BaseAddressType BaseAddress;
        public readonly int ByteCount;

        public bool UseAbsoluteAddressing
        {
            get
            {
                return BaseAddress == BaseAddressType.Absolute;
            }
        }

        public bool HasAdditiveBaseAddress
        {
            get
            {
                return BaseAddress != BaseAddressType.Relative && BaseAddress != BaseAddressType.Absolute && BaseAddress != BaseAddressType.Special;
            }
        }

        public uint Offset
        {
            get
            {
                switch (Config.Version)
                {
                    case Config.RomVersion.US:
                        if (OffsetUS != null) return OffsetUS.Value;
                        break;
                    case Config.RomVersion.JP:
                        if (OffsetJP != null) return OffsetJP.Value;
                        break;
                    case Config.RomVersion.PAL:
                        if (OffsetPAL != null) return OffsetPAL.Value;
                        break;
                }
                if (OffsetDefault != null) return OffsetDefault.Value;
                return 0;
            }
        }

        public List<uint> BaseAddressList
        {
            get
            {
                return VarXUtilities.GetBaseAddressListFromBaseAddressType(BaseAddress);
            }
        }

        public AddressHolder(int byteCount, BaseAddressType baseAddress,
            uint? offsetUS, uint? offsetJP, uint? offsetPAL, uint? offsetDefault)
        {
            if (offsetUS == null && offsetJP == null && offsetPAL == null && offsetDefault == null)
            {
                //TODO add this back in after var refactor
                //throw new ArgumentOutOfRangeException("Cannot instantiate Address with all null values");
            }

            ByteCount = byteCount;
            BaseAddress = baseAddress;

            OffsetUS = offsetUS;
            OffsetJP = offsetJP;
            OffsetPAL = offsetPAL;
            OffsetDefault = offsetDefault;
        }

        public uint GetRamAddress(bool addressArea = true)
        {
            uint baseAddress = BaseAddressList[0];
            var offsetedAddress = new UIntPtr(baseAddress + Offset);
            uint address;

            if (UseAbsoluteAddressing)
                address = (uint)Config.Stream.ConvertAddressEndianess(
                    new UIntPtr(offsetedAddress.ToUInt64() - (ulong)Config.Stream.ProcessMemoryOffset.ToInt64()),
                    ByteCount);
            else
                address = offsetedAddress.ToUInt32();

            return addressArea ? address | 0x80000000 : address & 0x0FFFFFFF;
        }

        public UIntPtr GetProcessAddress()
        {
            uint address = GetRamAddress(false);
            return Config.Stream.ConvertAddressEndianess(
                new UIntPtr(address + (ulong)Config.Stream.ProcessMemoryOffset.ToInt64()), ByteCount);
        }
    }
}
