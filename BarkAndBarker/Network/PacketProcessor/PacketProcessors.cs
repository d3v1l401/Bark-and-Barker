using Azure;
using BarkAndBarker.Session;
using BarkAndBarker.Steam;
using DC.Packet;
using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.Collections;
using BarkAndBarker.Persistence.Models;

namespace BarkAndBarker.Network.PacketProcessor
{
    internal class Helpers
    {
        public static string GetHWID(string[] hashes)
        {
            SHA512 sha = SHA512.Create();

            var final = "";
            foreach (var hash in hashes)
                final += hash + "-";

            var rawHash = sha.ComputeHash(final.ToByteArray());
            return Convert.ToBase64String(rawHash);
        }
    }
}