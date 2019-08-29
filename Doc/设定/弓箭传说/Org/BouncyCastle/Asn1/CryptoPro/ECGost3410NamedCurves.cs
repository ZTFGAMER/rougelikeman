namespace Org.BouncyCastle.Asn1.CryptoPro
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Utilities.Collections;
    using System;
    using System.Collections;

    public sealed class ECGost3410NamedCurves
    {
        internal static readonly IDictionary objIds = Platform.CreateHashtable();
        internal static readonly IDictionary parameters = Platform.CreateHashtable();
        internal static readonly IDictionary names = Platform.CreateHashtable();

        static ECGost3410NamedCurves()
        {
            BigInteger q = new BigInteger("115792089237316195423570985008687907853269984665640564039457584007913129639319");
            BigInteger order = new BigInteger("115792089237316195423570985008687907853073762908499243225378155805079068850323");
            FpCurve curve = new FpCurve(q, new BigInteger("115792089237316195423570985008687907853269984665640564039457584007913129639316"), new BigInteger("166"), order, BigInteger.One);
            ECDomainParameters parameters = new ECDomainParameters(curve, curve.CreatePoint(new BigInteger("1"), new BigInteger("64033881142927202683649881450433473985931760268884941288852745803908878638612")), order);
            ECGost3410NamedCurves.parameters[CryptoProObjectIdentifiers.GostR3410x2001CryptoProA] = parameters;
            q = new BigInteger("115792089237316195423570985008687907853269984665640564039457584007913129639319");
            order = new BigInteger("115792089237316195423570985008687907853073762908499243225378155805079068850323");
            curve = new FpCurve(q, new BigInteger("115792089237316195423570985008687907853269984665640564039457584007913129639316"), new BigInteger("166"), order, BigInteger.One);
            parameters = new ECDomainParameters(curve, curve.CreatePoint(new BigInteger("1"), new BigInteger("64033881142927202683649881450433473985931760268884941288852745803908878638612")), order);
            ECGost3410NamedCurves.parameters[CryptoProObjectIdentifiers.GostR3410x2001CryptoProXchA] = parameters;
            q = new BigInteger("57896044618658097711785492504343953926634992332820282019728792003956564823193");
            order = new BigInteger("57896044618658097711785492504343953927102133160255826820068844496087732066703");
            curve = new FpCurve(q, new BigInteger("57896044618658097711785492504343953926634992332820282019728792003956564823190"), new BigInteger("28091019353058090096996979000309560759124368558014865957655842872397301267595"), order, BigInteger.One);
            parameters = new ECDomainParameters(curve, curve.CreatePoint(new BigInteger("1"), new BigInteger("28792665814854611296992347458380284135028636778229113005756334730996303888124")), order);
            ECGost3410NamedCurves.parameters[CryptoProObjectIdentifiers.GostR3410x2001CryptoProB] = parameters;
            q = new BigInteger("70390085352083305199547718019018437841079516630045180471284346843705633502619");
            order = new BigInteger("70390085352083305199547718019018437840920882647164081035322601458352298396601");
            curve = new FpCurve(q, new BigInteger("70390085352083305199547718019018437841079516630045180471284346843705633502616"), new BigInteger("32858"), order, BigInteger.One);
            parameters = new ECDomainParameters(curve, curve.CreatePoint(new BigInteger("0"), new BigInteger("29818893917731240733471273240314769927240550812383695689146495261604565990247")), order);
            ECGost3410NamedCurves.parameters[CryptoProObjectIdentifiers.GostR3410x2001CryptoProXchB] = parameters;
            q = new BigInteger("70390085352083305199547718019018437841079516630045180471284346843705633502619");
            order = new BigInteger("70390085352083305199547718019018437840920882647164081035322601458352298396601");
            curve = new FpCurve(q, new BigInteger("70390085352083305199547718019018437841079516630045180471284346843705633502616"), new BigInteger("32858"), order, BigInteger.One);
            parameters = new ECDomainParameters(curve, curve.CreatePoint(new BigInteger("0"), new BigInteger("29818893917731240733471273240314769927240550812383695689146495261604565990247")), order);
            ECGost3410NamedCurves.parameters[CryptoProObjectIdentifiers.GostR3410x2001CryptoProC] = parameters;
            objIds["GostR3410-2001-CryptoPro-A"] = CryptoProObjectIdentifiers.GostR3410x2001CryptoProA;
            objIds["GostR3410-2001-CryptoPro-B"] = CryptoProObjectIdentifiers.GostR3410x2001CryptoProB;
            objIds["GostR3410-2001-CryptoPro-C"] = CryptoProObjectIdentifiers.GostR3410x2001CryptoProC;
            objIds["GostR3410-2001-CryptoPro-XchA"] = CryptoProObjectIdentifiers.GostR3410x2001CryptoProXchA;
            objIds["GostR3410-2001-CryptoPro-XchB"] = CryptoProObjectIdentifiers.GostR3410x2001CryptoProXchB;
            names[CryptoProObjectIdentifiers.GostR3410x2001CryptoProA] = "GostR3410-2001-CryptoPro-A";
            names[CryptoProObjectIdentifiers.GostR3410x2001CryptoProB] = "GostR3410-2001-CryptoPro-B";
            names[CryptoProObjectIdentifiers.GostR3410x2001CryptoProC] = "GostR3410-2001-CryptoPro-C";
            names[CryptoProObjectIdentifiers.GostR3410x2001CryptoProXchA] = "GostR3410-2001-CryptoPro-XchA";
            names[CryptoProObjectIdentifiers.GostR3410x2001CryptoProXchB] = "GostR3410-2001-CryptoPro-XchB";
        }

        private ECGost3410NamedCurves()
        {
        }

        public static ECDomainParameters GetByName(string name)
        {
            DerObjectIdentifier identifier = (DerObjectIdentifier) objIds[name];
            if (identifier != null)
            {
                return (ECDomainParameters) parameters[identifier];
            }
            return null;
        }

        public static ECDomainParameters GetByOid(DerObjectIdentifier oid) => 
            ((ECDomainParameters) parameters[oid]);

        public static string GetName(DerObjectIdentifier oid) => 
            ((string) names[oid]);

        public static DerObjectIdentifier GetOid(string name) => 
            ((DerObjectIdentifier) objIds[name]);

        public static IEnumerable Names =>
            new EnumerableProxy(names.Values);
    }
}

