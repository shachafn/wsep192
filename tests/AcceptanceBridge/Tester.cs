using NUnit.Framework;
using System;
using ATBridge;
namespace Tests
{

    public static class Tester
    {
        public static bool _groismanConnected = false;
        public static bool _groismanRegistered = false;
        public static bool _initalized = false;
        public static Guid AdminGuid = Guid.Empty;
        public static Guid _groismanShop = Guid.Empty;
        public static Guid galaxyGuid = Guid.Empty;
        public static Guid GroismanGuid = Guid.Empty;
        public static Guid BenGuid = Guid.Empty; //Ben is groiser's worker lol
        public static readonly Guid GuestGuid = new Guid("695D0341-3E62-4046-B337-2486443F311B");
        public static ProxyBridge PBridge = new ProxyBridge();
    }
}