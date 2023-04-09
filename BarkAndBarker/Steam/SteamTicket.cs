﻿using SteamKit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BarkAndBarker.Steam
{
    // All credits to Demonofpower: https://github.com/Demonofpower/SteamTicketDecrypt
    public class DlcDetails
    {
        public uint AppID { get; set; }
        public List<uint> Licenses { get; set; }
    }

    public class AppTicketDetails
    {
        public byte[] AuthTicket { get; set; }
        public string GcToken { get; set; }
        public DateTime TokenGenerated { get; set; }
        public IPAddress SessionExternalIP { get; set; }
        public uint ClientConnectionTime { get; set; }
        public uint ClientConnectionCount { get; set; }
        public uint Version { get; set; }
        public SteamID SteamID { get; set; }
        public uint AppID { get; set; }
        public IPAddress OwnershipTicketExternalIP { get; set; }
        public IPAddress OwnershipTicketInternalIP { get; set; }
        public uint OwnershipFlags { get; set; }
        public DateTime OwnershipTicketGenerated { get; set; }
        public DateTime OwnershipTicketExpires { get; set; }
        public List<uint> Licenses { get; set; }
        public List<DlcDetails> Dlc { get; set; }
        public byte[] Signature { get; set; }
        public bool IsExpired { get; set; }
        public bool HasValidSignature { get; set; }
        public bool IsValid { get; set; }
    }
    public class SteamTicket
    {
        public static AppTicketDetails ParseAppTicket(byte[] ticket)
        {
            using var ms = new MemoryStream(ticket);
            using var ticketReader = new BinaryReader(ms, System.Text.Encoding.UTF8, true);

            var details = new AppTicketDetails();

            try
            {
                uint initialLength = ticketReader.ReadUInt32();
                if (initialLength == 20)
                {
                    //details.AuthTicket = ticketReader.ReadBytes(52);
                    //details.AuthTicket = ticketReader.ReadBytes(48);

                    details.GcToken = ticketReader.ReadUInt64().ToString();
                    ticketReader.BaseStream.Seek(8, SeekOrigin.Current);
                    details.TokenGenerated = DateTimeOffset.FromUnixTimeSeconds(ticketReader.ReadUInt32()).DateTime;

                    if (ticketReader.ReadUInt32() != 24)
                    {
                        return null;
                    }

                    ticketReader.BaseStream.Seek(8, SeekOrigin.Current);
                    details.SessionExternalIP = new IPAddress(ticketReader.ReadUInt32());
                    ticketReader.BaseStream.Seek(4, SeekOrigin.Current);
                    details.ClientConnectionTime = ticketReader.ReadUInt32();
                    details.ClientConnectionCount = ticketReader.ReadUInt32();

                    if (ticketReader.ReadUInt32() + ms.Position != ms.Length)
                    {
                        return null;
                    }
                }
                else
                {
                    ms.Seek(-4, SeekOrigin.Current);
                }

                int ownershipTicketOffset = (int)ms.Position;
                int ownershipTicketLength = ticketReader.ReadInt32();
                if (ownershipTicketOffset + ownershipTicketLength != ms.Length &&
                    ownershipTicketOffset + ownershipTicketLength + 128 != ms.Length)
                {
                    return null;
                }

                details.Version = ticketReader.ReadUInt32();
                details.SteamID = new SteamID(ticketReader.ReadUInt64());
                details.AppID = ticketReader.ReadUInt32();
                details.OwnershipTicketExternalIP = new IPAddress(ticketReader.ReadUInt32());
                details.OwnershipTicketInternalIP = new IPAddress(ticketReader.ReadUInt32());
                details.OwnershipFlags = ticketReader.ReadUInt32();
                details.OwnershipTicketGenerated = DateTimeOffset.FromUnixTimeSeconds(ticketReader.ReadUInt32()).DateTime;
                details.OwnershipTicketExpires = DateTimeOffset.FromUnixTimeSeconds(ticketReader.ReadUInt32()).DateTime;
                details.Licenses = new List<uint>();

                int licenseCount = ticketReader.ReadUInt16();
                for (int i = 0; i < licenseCount; i++)
                {
                    details.Licenses.Add(ticketReader.ReadUInt32());
                }

                details.Dlc = new List<DlcDetails>();

                int dlcCount = ticketReader.ReadUInt16();
                for (int i = 0; i < dlcCount; i++)
                {
                    var dlc = new DlcDetails
                    {
                        AppID = ticketReader.ReadUInt32(),
                        Licenses = new List<uint>()
                    };

                    licenseCount = ticketReader.ReadUInt16();

                    for (int j = 0; j < licenseCount; j++)
                    {
                        dlc.Licenses.Add(ticketReader.ReadUInt32());
                    }

                    details.Dlc.Add(dlc);
                }

                ticketReader.ReadUInt16();

                if (ms.Position + 128 == ms.Length)
                {
                    details.Signature = ticketReader.ReadBytes(128);
                }

                DateTime currentDate = DateTime.Now;
                details.IsExpired = details.OwnershipTicketExpires < currentDate;
                //details.HasValidSignature = details.Signature != null && SteamCrypto.VerifySignature(ms.ToArray(),
                //    details.Signature, ownershipTicketOffset, ownershipTicketLength);
                //details.IsValid = !details.IsExpired && details.HasValidSignature);
                //if (!details.HasValidSignature && !allowInvalidSignature)
                //{
                //    throw new Exception("Missing or invalid signature");
                //}
            }
            catch
            {
                return null; // not a valid ticket
            }

            return details;
        }
    }
}
