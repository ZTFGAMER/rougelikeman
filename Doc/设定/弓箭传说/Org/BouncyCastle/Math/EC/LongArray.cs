namespace Org.BouncyCastle.Math.EC
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Utilities;
    using System;
    using System.Text;

    internal class LongArray
    {
        private static readonly ushort[] INTERLEAVE2_TABLE = new ushort[] { 
            0, 1, 4, 5, 0x10, 0x11, 20, 0x15, 0x40, 0x41, 0x44, 0x45, 80, 0x51, 0x54, 0x55,
            0x100, 0x101, 260, 0x105, 0x110, 0x111, 0x114, 0x115, 320, 0x141, 0x144, 0x145, 0x150, 0x151, 340, 0x155,
            0x400, 0x401, 0x404, 0x405, 0x410, 0x411, 0x414, 0x415, 0x440, 0x441, 0x444, 0x445, 0x450, 0x451, 0x454, 0x455,
            0x500, 0x501, 0x504, 0x505, 0x510, 0x511, 0x514, 0x515, 0x540, 0x541, 0x544, 0x545, 0x550, 0x551, 0x554, 0x555,
            0x1000, 0x1001, 0x1004, 0x1005, 0x1010, 0x1011, 0x1014, 0x1015, 0x1040, 0x1041, 0x1044, 0x1045, 0x1050, 0x1051, 0x1054, 0x1055,
            0x1100, 0x1101, 0x1104, 0x1105, 0x1110, 0x1111, 0x1114, 0x1115, 0x1140, 0x1141, 0x1144, 0x1145, 0x1150, 0x1151, 0x1154, 0x1155,
            0x1400, 0x1401, 0x1404, 0x1405, 0x1410, 0x1411, 0x1414, 0x1415, 0x1440, 0x1441, 0x1444, 0x1445, 0x1450, 0x1451, 0x1454, 0x1455,
            0x1500, 0x1501, 0x1504, 0x1505, 0x1510, 0x1511, 0x1514, 0x1515, 0x1540, 0x1541, 0x1544, 0x1545, 0x1550, 0x1551, 0x1554, 0x1555,
            0x4000, 0x4001, 0x4004, 0x4005, 0x4010, 0x4011, 0x4014, 0x4015, 0x4040, 0x4041, 0x4044, 0x4045, 0x4050, 0x4051, 0x4054, 0x4055,
            0x4100, 0x4101, 0x4104, 0x4105, 0x4110, 0x4111, 0x4114, 0x4115, 0x4140, 0x4141, 0x4144, 0x4145, 0x4150, 0x4151, 0x4154, 0x4155,
            0x4400, 0x4401, 0x4404, 0x4405, 0x4410, 0x4411, 0x4414, 0x4415, 0x4440, 0x4441, 0x4444, 0x4445, 0x4450, 0x4451, 0x4454, 0x4455,
            0x4500, 0x4501, 0x4504, 0x4505, 0x4510, 0x4511, 0x4514, 0x4515, 0x4540, 0x4541, 0x4544, 0x4545, 0x4550, 0x4551, 0x4554, 0x4555,
            0x5000, 0x5001, 0x5004, 0x5005, 0x5010, 0x5011, 0x5014, 0x5015, 0x5040, 0x5041, 0x5044, 0x5045, 0x5050, 0x5051, 0x5054, 0x5055,
            0x5100, 0x5101, 0x5104, 0x5105, 0x5110, 0x5111, 0x5114, 0x5115, 0x5140, 0x5141, 0x5144, 0x5145, 0x5150, 0x5151, 0x5154, 0x5155,
            0x5400, 0x5401, 0x5404, 0x5405, 0x5410, 0x5411, 0x5414, 0x5415, 0x5440, 0x5441, 0x5444, 0x5445, 0x5450, 0x5451, 0x5454, 0x5455,
            0x5500, 0x5501, 0x5504, 0x5505, 0x5510, 0x5511, 0x5514, 0x5515, 0x5540, 0x5541, 0x5544, 0x5545, 0x5550, 0x5551, 0x5554, 0x5555
        };
        private static readonly int[] INTERLEAVE3_TABLE = new int[] { 
            0, 1, 8, 9, 0x40, 0x41, 0x48, 0x49, 0x200, 0x201, 520, 0x209, 0x240, 0x241, 0x248, 0x249,
            0x1000, 0x1001, 0x1008, 0x1009, 0x1040, 0x1041, 0x1048, 0x1049, 0x1200, 0x1201, 0x1208, 0x1209, 0x1240, 0x1241, 0x1248, 0x1249,
            0x8000, 0x8001, 0x8008, 0x8009, 0x8040, 0x8041, 0x8048, 0x8049, 0x8200, 0x8201, 0x8208, 0x8209, 0x8240, 0x8241, 0x8248, 0x8249,
            0x9000, 0x9001, 0x9008, 0x9009, 0x9040, 0x9041, 0x9048, 0x9049, 0x9200, 0x9201, 0x9208, 0x9209, 0x9240, 0x9241, 0x9248, 0x9249,
            0x40000, 0x40001, 0x40008, 0x40009, 0x40040, 0x40041, 0x40048, 0x40049, 0x40200, 0x40201, 0x40208, 0x40209, 0x40240, 0x40241, 0x40248, 0x40249,
            0x41000, 0x41001, 0x41008, 0x41009, 0x41040, 0x41041, 0x41048, 0x41049, 0x41200, 0x41201, 0x41208, 0x41209, 0x41240, 0x41241, 0x41248, 0x41249,
            0x48000, 0x48001, 0x48008, 0x48009, 0x48040, 0x48041, 0x48048, 0x48049, 0x48200, 0x48201, 0x48208, 0x48209, 0x48240, 0x48241, 0x48248, 0x48249,
            0x49000, 0x49001, 0x49008, 0x49009, 0x49040, 0x49041, 0x49048, 0x49049, 0x49200, 0x49201, 0x49208, 0x49209, 0x49240, 0x49241, 0x49248, 0x49249
        };
        private static readonly int[] INTERLEAVE4_TABLE = new int[] { 
            0, 1, 0x10, 0x11, 0x100, 0x101, 0x110, 0x111, 0x1000, 0x1001, 0x1010, 0x1011, 0x1100, 0x1101, 0x1110, 0x1111,
            0x10000, 0x10001, 0x10010, 0x10011, 0x10100, 0x10101, 0x10110, 0x10111, 0x11000, 0x11001, 0x11010, 0x11011, 0x11100, 0x11101, 0x11110, 0x11111,
            0x100000, 0x100001, 0x100010, 0x100011, 0x100100, 0x100101, 0x100110, 0x100111, 0x101000, 0x101001, 0x101010, 0x101011, 0x101100, 0x101101, 0x101110, 0x101111,
            0x110000, 0x110001, 0x110010, 0x110011, 0x110100, 0x110101, 0x110110, 0x110111, 0x111000, 0x111001, 0x111010, 0x111011, 0x111100, 0x111101, 0x111110, 0x111111,
            0x1000000, 0x1000001, 0x1000010, 0x1000011, 0x1000100, 0x1000101, 0x1000110, 0x1000111, 0x1001000, 0x1001001, 0x1001010, 0x1001011, 0x1001100, 0x1001101, 0x1001110, 0x1001111,
            0x1010000, 0x1010001, 0x1010010, 0x1010011, 0x1010100, 0x1010101, 0x1010110, 0x1010111, 0x1011000, 0x1011001, 0x1011010, 0x1011011, 0x1011100, 0x1011101, 0x1011110, 0x1011111,
            0x1100000, 0x1100001, 0x1100010, 0x1100011, 0x1100100, 0x1100101, 0x1100110, 0x1100111, 0x1101000, 0x1101001, 0x1101010, 0x1101011, 0x1101100, 0x1101101, 0x1101110, 0x1101111,
            0x1110000, 0x1110001, 0x1110010, 0x1110011, 0x1110100, 0x1110101, 0x1110110, 0x1110111, 0x1111000, 0x1111001, 0x1111010, 0x1111011, 0x1111100, 0x1111101, 0x1111110, 0x1111111,
            0x10000000, 0x10000001, 0x10000010, 0x10000011, 0x10000100, 0x10000101, 0x10000110, 0x10000111, 0x10001000, 0x10001001, 0x10001010, 0x10001011, 0x10001100, 0x10001101, 0x10001110, 0x10001111,
            0x10010000, 0x10010001, 0x10010010, 0x10010011, 0x10010100, 0x10010101, 0x10010110, 0x10010111, 0x10011000, 0x10011001, 0x10011010, 0x10011011, 0x10011100, 0x10011101, 0x10011110, 0x10011111,
            0x10100000, 0x10100001, 0x10100010, 0x10100011, 0x10100100, 0x10100101, 0x10100110, 0x10100111, 0x10101000, 0x10101001, 0x10101010, 0x10101011, 0x10101100, 0x10101101, 0x10101110, 0x10101111,
            0x10110000, 0x10110001, 0x10110010, 0x10110011, 0x10110100, 0x10110101, 0x10110110, 0x10110111, 0x10111000, 0x10111001, 0x10111010, 0x10111011, 0x10111100, 0x10111101, 0x10111110, 0x10111111,
            0x11000000, 0x11000001, 0x11000010, 0x11000011, 0x11000100, 0x11000101, 0x11000110, 0x11000111, 0x11001000, 0x11001001, 0x11001010, 0x11001011, 0x11001100, 0x11001101, 0x11001110, 0x11001111,
            0x11010000, 0x11010001, 0x11010010, 0x11010011, 0x11010100, 0x11010101, 0x11010110, 0x11010111, 0x11011000, 0x11011001, 0x11011010, 0x11011011, 0x11011100, 0x11011101, 0x11011110, 0x11011111,
            0x11100000, 0x11100001, 0x11100010, 0x11100011, 0x11100100, 0x11100101, 0x11100110, 0x11100111, 0x11101000, 0x11101001, 0x11101010, 0x11101011, 0x11101100, 0x11101101, 0x11101110, 0x11101111,
            0x11110000, 0x11110001, 0x11110010, 0x11110011, 0x11110100, 0x11110101, 0x11110110, 0x11110111, 0x11111000, 0x11111001, 0x11111010, 0x11111011, 0x11111100, 0x11111101, 0x11111110, 0x11111111
        };
        private static readonly int[] INTERLEAVE5_TABLE = new int[] { 
            0, 1, 0x20, 0x21, 0x400, 0x401, 0x420, 0x421, 0x8000, 0x8001, 0x8020, 0x8021, 0x8400, 0x8401, 0x8420, 0x8421,
            0x100000, 0x100001, 0x100020, 0x100021, 0x100400, 0x100401, 0x100420, 0x100421, 0x108000, 0x108001, 0x108020, 0x108021, 0x108400, 0x108401, 0x108420, 0x108421,
            0x2000000, 0x2000001, 0x2000020, 0x2000021, 0x2000400, 0x2000401, 0x2000420, 0x2000421, 0x2008000, 0x2008001, 0x2008020, 0x2008021, 0x2008400, 0x2008401, 0x2008420, 0x2008421,
            0x2100000, 0x2100001, 0x2100020, 0x2100021, 0x2100400, 0x2100401, 0x2100420, 0x2100421, 0x2108000, 0x2108001, 0x2108020, 0x2108021, 0x2108400, 0x2108401, 0x2108420, 0x2108421,
            0x40000000, 0x40000001, 0x40000020, 0x40000021, 0x40000400, 0x40000401, 0x40000420, 0x40000421, 0x40008000, 0x40008001, 0x40008020, 0x40008021, 0x40008400, 0x40008401, 0x40008420, 0x40008421,
            0x40100000, 0x40100001, 0x40100020, 0x40100021, 0x40100400, 0x40100401, 0x40100420, 0x40100421, 0x40108000, 0x40108001, 0x40108020, 0x40108021, 0x40108400, 0x40108401, 0x40108420, 0x40108421,
            0x42000000, 0x42000001, 0x42000020, 0x42000021, 0x42000400, 0x42000401, 0x42000420, 0x42000421, 0x42008000, 0x42008001, 0x42008020, 0x42008021, 0x42008400, 0x42008401, 0x42008420, 0x42008421,
            0x42100000, 0x42100001, 0x42100020, 0x42100021, 0x42100400, 0x42100401, 0x42100420, 0x42100421, 0x42108000, 0x42108001, 0x42108020, 0x42108021, 0x42108400, 0x42108401, 0x42108420, 0x42108421
        };
        private static readonly long[] INTERLEAVE7_TABLE = new long[] { 
            0L, 1L, 0x80L, 0x81L, 0x4000L, 0x4001L, 0x4080L, 0x4081L, 0x200000L, 0x200001L, 0x200080L, 0x200081L, 0x204000L, 0x204001L, 0x204080L, 0x204081L,
            0x10000000L, 0x10000001L, 0x10000080L, 0x10000081L, 0x10004000L, 0x10004001L, 0x10004080L, 0x10004081L, 0x10200000L, 0x10200001L, 0x10200080L, 0x10200081L, 0x10204000L, 0x10204001L, 0x10204080L, 0x10204081L,
            0x800000000L, 0x800000001L, 0x800000080L, 0x800000081L, 0x800004000L, 0x800004001L, 0x800004080L, 0x800004081L, 0x800200000L, 0x800200001L, 0x800200080L, 0x800200081L, 0x800204000L, 0x800204001L, 0x800204080L, 0x800204081L,
            0x810000000L, 0x810000001L, 0x810000080L, 0x810000081L, 0x810004000L, 0x810004001L, 0x810004080L, 0x810004081L, 0x810200000L, 0x810200001L, 0x810200080L, 0x810200081L, 0x810204000L, 0x810204001L, 0x810204080L, 0x810204081L,
            0x40000000000L, 0x40000000001L, 0x40000000080L, 0x40000000081L, 0x40000004000L, 0x40000004001L, 0x40000004080L, 0x40000004081L, 0x40000200000L, 0x40000200001L, 0x40000200080L, 0x40000200081L, 0x40000204000L, 0x40000204001L, 0x40000204080L, 0x40000204081L,
            0x40010000000L, 0x40010000001L, 0x40010000080L, 0x40010000081L, 0x40010004000L, 0x40010004001L, 0x40010004080L, 0x40010004081L, 0x40010200000L, 0x40010200001L, 0x40010200080L, 0x40010200081L, 0x40010204000L, 0x40010204001L, 0x40010204080L, 0x40010204081L,
            0x40800000000L, 0x40800000001L, 0x40800000080L, 0x40800000081L, 0x40800004000L, 0x40800004001L, 0x40800004080L, 0x40800004081L, 0x40800200000L, 0x40800200001L, 0x40800200080L, 0x40800200081L, 0x40800204000L, 0x40800204001L, 0x40800204080L, 0x40800204081L,
            0x40810000000L, 0x40810000001L, 0x40810000080L, 0x40810000081L, 0x40810004000L, 0x40810004001L, 0x40810004080L, 0x40810004081L, 0x40810200000L, 0x40810200001L, 0x40810200080L, 0x40810200081L, 0x40810204000L, 0x40810204001L, 0x40810204080L, 0x40810204081L,
            0x2000000000000L, 0x2000000000001L, 0x2000000000080L, 0x2000000000081L, 0x2000000004000L, 0x2000000004001L, 0x2000000004080L, 0x2000000004081L, 0x2000000200000L, 0x2000000200001L, 0x2000000200080L, 0x2000000200081L, 0x2000000204000L, 0x2000000204001L, 0x2000000204080L, 0x2000000204081L,
            0x2000010000000L, 0x2000010000001L, 0x2000010000080L, 0x2000010000081L, 0x2000010004000L, 0x2000010004001L, 0x2000010004080L, 0x2000010004081L, 0x2000010200000L, 0x2000010200001L, 0x2000010200080L, 0x2000010200081L, 0x2000010204000L, 0x2000010204001L, 0x2000010204080L, 0x2000010204081L,
            0x2000800000000L, 0x2000800000001L, 0x2000800000080L, 0x2000800000081L, 0x2000800004000L, 0x2000800004001L, 0x2000800004080L, 0x2000800004081L, 0x2000800200000L, 0x2000800200001L, 0x2000800200080L, 0x2000800200081L, 0x2000800204000L, 0x2000800204001L, 0x2000800204080L, 0x2000800204081L,
            0x2000810000000L, 0x2000810000001L, 0x2000810000080L, 0x2000810000081L, 0x2000810004000L, 0x2000810004001L, 0x2000810004080L, 0x2000810004081L, 0x2000810200000L, 0x2000810200001L, 0x2000810200080L, 0x2000810200081L, 0x2000810204000L, 0x2000810204001L, 0x2000810204080L, 0x2000810204081L,
            0x2040000000000L, 0x2040000000001L, 0x2040000000080L, 0x2040000000081L, 0x2040000004000L, 0x2040000004001L, 0x2040000004080L, 0x2040000004081L, 0x2040000200000L, 0x2040000200001L, 0x2040000200080L, 0x2040000200081L, 0x2040000204000L, 0x2040000204001L, 0x2040000204080L, 0x2040000204081L,
            0x2040010000000L, 0x2040010000001L, 0x2040010000080L, 0x2040010000081L, 0x2040010004000L, 0x2040010004001L, 0x2040010004080L, 0x2040010004081L, 0x2040010200000L, 0x2040010200001L, 0x2040010200080L, 0x2040010200081L, 0x2040010204000L, 0x2040010204001L, 0x2040010204080L, 0x2040010204081L,
            0x2040800000000L, 0x2040800000001L, 0x2040800000080L, 0x2040800000081L, 0x2040800004000L, 0x2040800004001L, 0x2040800004080L, 0x2040800004081L, 0x2040800200000L, 0x2040800200001L, 0x2040800200080L, 0x2040800200081L, 0x2040800204000L, 0x2040800204001L, 0x2040800204080L, 0x2040800204081L,
            0x2040810000000L, 0x2040810000001L, 0x2040810000080L, 0x2040810000081L, 0x2040810004000L, 0x2040810004001L, 0x2040810004080L, 0x2040810004081L, 0x2040810200000L, 0x2040810200001L, 0x2040810200080L, 0x2040810200081L, 0x2040810204000L, 0x2040810204001L, 0x2040810204080L, 0x2040810204081L,
            0x100000000000000L, 0x100000000000001L, 0x100000000000080L, 0x100000000000081L, 0x100000000004000L, 0x100000000004001L, 0x100000000004080L, 0x100000000004081L, 0x100000000200000L, 0x100000000200001L, 0x100000000200080L, 0x100000000200081L, 0x100000000204000L, 0x100000000204001L, 0x100000000204080L, 0x100000000204081L,
            0x100000010000000L, 0x100000010000001L, 0x100000010000080L, 0x100000010000081L, 0x100000010004000L, 0x100000010004001L, 0x100000010004080L, 0x100000010004081L, 0x100000010200000L, 0x100000010200001L, 0x100000010200080L, 0x100000010200081L, 0x100000010204000L, 0x100000010204001L, 0x100000010204080L, 0x100000010204081L,
            0x100000800000000L, 0x100000800000001L, 0x100000800000080L, 0x100000800000081L, 0x100000800004000L, 0x100000800004001L, 0x100000800004080L, 0x100000800004081L, 0x100000800200000L, 0x100000800200001L, 0x100000800200080L, 0x100000800200081L, 0x100000800204000L, 0x100000800204001L, 0x100000800204080L, 0x100000800204081L,
            0x100000810000000L, 0x100000810000001L, 0x100000810000080L, 0x100000810000081L, 0x100000810004000L, 0x100000810004001L, 0x100000810004080L, 0x100000810004081L, 0x100000810200000L, 0x100000810200001L, 0x100000810200080L, 0x100000810200081L, 0x100000810204000L, 0x100000810204001L, 0x100000810204080L, 0x100000810204081L,
            0x100040000000000L, 0x100040000000001L, 0x100040000000080L, 0x100040000000081L, 0x100040000004000L, 0x100040000004001L, 0x100040000004080L, 0x100040000004081L, 0x100040000200000L, 0x100040000200001L, 0x100040000200080L, 0x100040000200081L, 0x100040000204000L, 0x100040000204001L, 0x100040000204080L, 0x100040000204081L,
            0x100040010000000L, 0x100040010000001L, 0x100040010000080L, 0x100040010000081L, 0x100040010004000L, 0x100040010004001L, 0x100040010004080L, 0x100040010004081L, 0x100040010200000L, 0x100040010200001L, 0x100040010200080L, 0x100040010200081L, 0x100040010204000L, 0x100040010204001L, 0x100040010204080L, 0x100040010204081L,
            0x100040800000000L, 0x100040800000001L, 0x100040800000080L, 0x100040800000081L, 0x100040800004000L, 0x100040800004001L, 0x100040800004080L, 0x100040800004081L, 0x100040800200000L, 0x100040800200001L, 0x100040800200080L, 0x100040800200081L, 0x100040800204000L, 0x100040800204001L, 0x100040800204080L, 0x100040800204081L,
            0x100040810000000L, 0x100040810000001L, 0x100040810000080L, 0x100040810000081L, 0x100040810004000L, 0x100040810004001L, 0x100040810004080L, 0x100040810004081L, 0x100040810200000L, 0x100040810200001L, 0x100040810200080L, 0x100040810200081L, 0x100040810204000L, 0x100040810204001L, 0x100040810204080L, 0x100040810204081L,
            0x102000000000000L, 0x102000000000001L, 0x102000000000080L, 0x102000000000081L, 0x102000000004000L, 0x102000000004001L, 0x102000000004080L, 0x102000000004081L, 0x102000000200000L, 0x102000000200001L, 0x102000000200080L, 0x102000000200081L, 0x102000000204000L, 0x102000000204001L, 0x102000000204080L, 0x102000000204081L,
            0x102000010000000L, 0x102000010000001L, 0x102000010000080L, 0x102000010000081L, 0x102000010004000L, 0x102000010004001L, 0x102000010004080L, 0x102000010004081L, 0x102000010200000L, 0x102000010200001L, 0x102000010200080L, 0x102000010200081L, 0x102000010204000L, 0x102000010204001L, 0x102000010204080L, 0x102000010204081L,
            0x102000800000000L, 0x102000800000001L, 0x102000800000080L, 0x102000800000081L, 0x102000800004000L, 0x102000800004001L, 0x102000800004080L, 0x102000800004081L, 0x102000800200000L, 0x102000800200001L, 0x102000800200080L, 0x102000800200081L, 0x102000800204000L, 0x102000800204001L, 0x102000800204080L, 0x102000800204081L,
            0x102000810000000L, 0x102000810000001L, 0x102000810000080L, 0x102000810000081L, 0x102000810004000L, 0x102000810004001L, 0x102000810004080L, 0x102000810004081L, 0x102000810200000L, 0x102000810200001L, 0x102000810200080L, 0x102000810200081L, 0x102000810204000L, 0x102000810204001L, 0x102000810204080L, 0x102000810204081L,
            0x102040000000000L, 0x102040000000001L, 0x102040000000080L, 0x102040000000081L, 0x102040000004000L, 0x102040000004001L, 0x102040000004080L, 0x102040000004081L, 0x102040000200000L, 0x102040000200001L, 0x102040000200080L, 0x102040000200081L, 0x102040000204000L, 0x102040000204001L, 0x102040000204080L, 0x102040000204081L,
            0x102040010000000L, 0x102040010000001L, 0x102040010000080L, 0x102040010000081L, 0x102040010004000L, 0x102040010004001L, 0x102040010004080L, 0x102040010004081L, 0x102040010200000L, 0x102040010200001L, 0x102040010200080L, 0x102040010200081L, 0x102040010204000L, 0x102040010204001L, 0x102040010204080L, 0x102040010204081L,
            0x102040800000000L, 0x102040800000001L, 0x102040800000080L, 0x102040800000081L, 0x102040800004000L, 0x102040800004001L, 0x102040800004080L, 0x102040800004081L, 0x102040800200000L, 0x102040800200001L, 0x102040800200080L, 0x102040800200081L, 0x102040800204000L, 0x102040800204001L, 0x102040800204080L, 0x102040800204081L,
            0x102040810000000L, 0x102040810000001L, 0x102040810000080L, 0x102040810000081L, 0x102040810004000L, 0x102040810004001L, 0x102040810004080L, 0x102040810004081L, 0x102040810200000L, 0x102040810200001L, 0x102040810200080L, 0x102040810200081L, 0x102040810204000L, 0x102040810204001L, 0x102040810204080L, 0x102040810204081L
        };
        private const string ZEROES = "0000000000000000000000000000000000000000000000000000000000000000";
        internal static readonly byte[] BitLengths = new byte[] { 
            0, 1, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 4,
            5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
            6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
            6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8
        };
        private long[] m_ints;

        public LongArray(BigInteger bigInt)
        {
            if ((bigInt == null) || (bigInt.SignValue < 0))
            {
                throw new ArgumentException("invalid F2m field value", "bigInt");
            }
            if (bigInt.SignValue == 0)
            {
                this.m_ints = new long[1];
            }
            else
            {
                byte[] buffer = bigInt.ToByteArray();
                int length = buffer.Length;
                int num2 = 0;
                if (buffer[0] == 0)
                {
                    length--;
                    num2 = 1;
                }
                int num3 = (length + 7) / 8;
                this.m_ints = new long[num3];
                int index = num3 - 1;
                int num5 = (length % 8) + num2;
                long num6 = 0L;
                int num7 = num2;
                if (num2 < num5)
                {
                    while (num7 < num5)
                    {
                        num6 = num6 << 8;
                        uint num8 = buffer[num7];
                        num6 |= num8;
                        num7++;
                    }
                    this.m_ints[index--] = num6;
                }
                while (index >= 0)
                {
                    num6 = 0L;
                    for (int i = 0; i < 8; i++)
                    {
                        num6 = num6 << 8;
                        uint num10 = buffer[num7++];
                        num6 |= num10;
                    }
                    this.m_ints[index] = num6;
                    index--;
                }
            }
        }

        public LongArray(int intLen)
        {
            this.m_ints = new long[intLen];
        }

        public LongArray(long[] ints)
        {
            this.m_ints = ints;
        }

        public LongArray(long[] ints, int off, int len)
        {
            if ((off == 0) && (len == ints.Length))
            {
                this.m_ints = ints;
            }
            else
            {
                this.m_ints = new long[len];
                Array.Copy(ints, off, this.m_ints, 0, len);
            }
        }

        private static void Add(long[] x, int xOff, long[] y, int yOff, int count)
        {
            for (int i = 0; i < count; i++)
            {
                x[xOff + i] ^= y[yOff + i];
            }
        }

        private static void Add(long[] x, int xOff, long[] y, int yOff, long[] z, int zOff, int count)
        {
            for (int i = 0; i < count; i++)
            {
                z[zOff + i] = x[xOff + i] ^ y[yOff + i];
            }
        }

        private static void AddBoth(long[] x, int xOff, long[] y1, int y1Off, long[] y2, int y2Off, int count)
        {
            for (int i = 0; i < count; i++)
            {
                x[xOff + i] ^= y1[y1Off + i] ^ y2[y2Off + i];
            }
        }

        public LongArray AddOne()
        {
            if (this.m_ints.Length == 0)
            {
                return new LongArray(new long[] { 1L });
            }
            int newLen = Math.Max(1, this.GetUsedLength());
            long[] ints = this.ResizedInts(newLen);
            ints[0] ^= 1L;
            return new LongArray(ints);
        }

        private void AddShiftedByBitsSafe(LongArray other, int otherDegree, int bits)
        {
            int count = (otherDegree + 0x3f) >> 6;
            int xOff = bits >> 6;
            int shift = bits & 0x3f;
            if (shift == 0)
            {
                Add(this.m_ints, xOff, other.m_ints, 0, count);
            }
            else
            {
                long num4 = AddShiftedUp(this.m_ints, xOff, other.m_ints, 0, count, shift);
                if (num4 != 0L)
                {
                    this.m_ints[count + xOff] ^= num4;
                }
            }
        }

        public void AddShiftedByWords(LongArray other, int words)
        {
            int usedLength = other.GetUsedLength();
            if (usedLength != 0)
            {
                int newLen = usedLength + words;
                if (newLen > this.m_ints.Length)
                {
                    this.m_ints = this.ResizedInts(newLen);
                }
                Add(this.m_ints, words, other.m_ints, 0, usedLength);
            }
        }

        private static long AddShiftedDown(long[] x, int xOff, long[] y, int yOff, int count, int shift)
        {
            int num = 0x40 - shift;
            long num2 = 0L;
            int num3 = count;
            while (--num3 >= 0)
            {
                long num4 = y[yOff + num3];
                x[xOff + num3] ^= (num4 >> shift) | num2;
                num2 = num4 << num;
            }
            return num2;
        }

        private static long AddShiftedUp(long[] x, int xOff, long[] y, int yOff, int count, int shift)
        {
            int num = 0x40 - shift;
            long num2 = 0L;
            for (int i = 0; i < count; i++)
            {
                long num4 = y[yOff + i];
                x[xOff + i] ^= (num4 << shift) | num2;
                num2 = num4 >> num;
            }
            return num2;
        }

        private static int BitLength(long w)
        {
            int num2;
            int num4;
            int index = (int) (w >> 0x20);
            if (index == 0)
            {
                index = (int) w;
                num2 = 0;
            }
            else
            {
                num2 = 0x20;
            }
            int num3 = index >> 0x10;
            if (num3 == 0)
            {
                num3 = index >> 8;
                num4 = (num3 != 0) ? (8 + BitLengths[num3]) : BitLengths[index];
            }
            else
            {
                int num5 = num3 >> 8;
                num4 = (num5 != 0) ? (0x18 + BitLengths[num5]) : (0x10 + BitLengths[num3]);
            }
            return (num2 + num4);
        }

        public LongArray Copy() => 
            new LongArray(Arrays.Clone(this.m_ints));

        public int Degree()
        {
            long num2;
            int length = this.m_ints.Length;
            do
            {
                if (length == 0)
                {
                    return 0;
                }
                num2 = this.m_ints[--length];
            }
            while (num2 == 0L);
            return ((length << 6) + BitLength(num2));
        }

        private int DegreeFrom(int limit)
        {
            long num2;
            int num = (limit + 0x3e) >> 6;
            do
            {
                if (num == 0)
                {
                    return 0;
                }
                num2 = this.m_ints[--num];
            }
            while (num2 == 0L);
            return ((num << 6) + BitLength(num2));
        }

        private static void Distribute(long[] x, int src, int dst1, int dst2, int count)
        {
            for (int i = 0; i < count; i++)
            {
                long num2 = x[src + i];
                x[dst1 + i] ^= num2;
                x[dst2 + i] ^= num2;
            }
        }

        public virtual bool Equals(LongArray other)
        {
            if (this != other)
            {
                if (other == null)
                {
                    return false;
                }
                int usedLength = this.GetUsedLength();
                if (other.GetUsedLength() != usedLength)
                {
                    return false;
                }
                for (int i = 0; i < usedLength; i++)
                {
                    if (this.m_ints[i] != other.m_ints[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public override bool Equals(object obj) => 
            this.Equals(obj as LongArray);

        private static void FlipBit(long[] buf, int off, int n)
        {
            int num = n >> 6;
            int num2 = n & 0x3f;
            long num3 = ((long) 1L) << num2;
            buf[off + num] ^= num3;
        }

        private static void FlipVector(long[] x, int xOff, long[] y, int yOff, int yLen, int bits)
        {
            xOff += bits >> 6;
            bits &= 0x3f;
            if (bits == 0)
            {
                Add(x, xOff, y, yOff, yLen);
            }
            else
            {
                long num = AddShiftedDown(x, xOff + 1, y, yOff, yLen, 0x40 - bits);
                x[xOff] ^= num;
            }
        }

        private static void FlipWord(long[] buf, int off, int bit, long word)
        {
            int index = off + (bit >> 6);
            int num2 = bit & 0x3f;
            if (num2 == 0)
            {
                buf[index] ^= word;
            }
            else
            {
                buf[index] ^= word << num2;
                word = word >> (0x40 - num2);
                if (word != 0L)
                {
                    buf[++index] ^= word;
                }
            }
        }

        public override int GetHashCode()
        {
            int usedLength = this.GetUsedLength();
            int num2 = 1;
            for (int i = 0; i < usedLength; i++)
            {
                long num4 = this.m_ints[i];
                num2 *= 0x1f;
                num2 ^= (int) num4;
                num2 *= 0x1f;
                num2 ^= (int) (num4 >> 0x20);
            }
            return num2;
        }

        public int GetUsedLength() => 
            this.GetUsedLengthFrom(this.m_ints.Length);

        public int GetUsedLengthFrom(int from)
        {
            long[] ints = this.m_ints;
            from = Math.Min(from, ints.Length);
            if (from >= 1)
            {
                if (ints[0] != 0L)
                {
                    while (ints[--from] == 0L)
                    {
                    }
                    return (from + 1);
                }
                do
                {
                    if (ints[--from] != 0L)
                    {
                        return (from + 1);
                    }
                }
                while (from > 0);
            }
            return 0;
        }

        private static void Interleave(long[] x, int xOff, long[] z, int zOff, int count, int width)
        {
            switch (width)
            {
                case 3:
                    Interleave3(x, xOff, z, zOff, count);
                    return;

                case 5:
                    Interleave5(x, xOff, z, zOff, count);
                    return;

                case 7:
                    Interleave7(x, xOff, z, zOff, count);
                    return;
            }
            Interleave2_n(x, xOff, z, zOff, count, BitLengths[width] - 1);
        }

        private static long Interleave2_32to64(int x)
        {
            int num = INTERLEAVE2_TABLE[x & 0xff] | (INTERLEAVE2_TABLE[(x >> 8) & 0xff] << 0x10);
            int num2 = INTERLEAVE2_TABLE[(x >> 0x10) & 0xff] | (INTERLEAVE2_TABLE[x >> 0x18] << 0x10);
            return (((num2 & 0xffffffffL) << 0x20) | (num & 0xffffffffL));
        }

        private static long Interleave2_n(long x, int rounds)
        {
            while (rounds > 1)
            {
                rounds -= 2;
                x = ((Interleave4_16to64(((int) x) & 0xffff) | (Interleave4_16to64(((int) (x >> 0x10)) & 0xffff) << 1)) | (Interleave4_16to64(((int) (x >> 0x20)) & 0xffff) << 2)) | (Interleave4_16to64(((int) (x >> 0x30)) & 0xffff) << 3);
            }
            if (rounds > 0)
            {
                x = Interleave2_32to64((int) x) | (Interleave2_32to64((int) (x >> 0x20)) << 1);
            }
            return x;
        }

        private static void Interleave2_n(long[] x, int xOff, long[] z, int zOff, int count, int rounds)
        {
            for (int i = 0; i < count; i++)
            {
                z[zOff + i] = Interleave2_n(x[xOff + i], rounds);
            }
        }

        private static long Interleave3(long x)
        {
            long num = x & -9223372036854775808L;
            return (((num | Interleave3_21to63(((int) x) & 0x1fffff)) | (Interleave3_21to63(((int) (x >> 0x15)) & 0x1fffff) << 1)) | (Interleave3_21to63(((int) (x >> 0x2a)) & 0x1fffff) << 2));
        }

        private static void Interleave3(long[] x, int xOff, long[] z, int zOff, int count)
        {
            for (int i = 0; i < count; i++)
            {
                z[zOff + i] = Interleave3(x[xOff + i]);
            }
        }

        private static long Interleave3_13to65(int x)
        {
            int num = INTERLEAVE5_TABLE[x & 0x7f];
            int num2 = INTERLEAVE5_TABLE[x >> 7];
            return (((num2 & 0xffffffffL) << 0x23) | (num & 0xffffffffL));
        }

        private static long Interleave3_21to63(int x)
        {
            int num = INTERLEAVE3_TABLE[x & 0x7f];
            int num2 = INTERLEAVE3_TABLE[(x >> 7) & 0x7f];
            int num3 = INTERLEAVE3_TABLE[x >> 14];
            return ((((num3 & 0xffffffffL) << 0x2a) | ((num2 & 0xffffffffL) << 0x15)) | (num & 0xffffffffL));
        }

        private static long Interleave4_16to64(int x)
        {
            int num = INTERLEAVE4_TABLE[x & 0xff];
            int num2 = INTERLEAVE4_TABLE[x >> 8];
            return (((num2 & 0xffffffffL) << 0x20) | (num & 0xffffffffL));
        }

        private static long Interleave5(long x) => 
            ((((Interleave3_13to65(((int) x) & 0x1fff) | (Interleave3_13to65(((int) (x >> 13)) & 0x1fff) << 1)) | (Interleave3_13to65(((int) (x >> 0x1a)) & 0x1fff) << 2)) | (Interleave3_13to65(((int) (x >> 0x27)) & 0x1fff) << 3)) | (Interleave3_13to65(((int) (x >> 0x34)) & 0x1fff) << 4));

        private static void Interleave5(long[] x, int xOff, long[] z, int zOff, int count)
        {
            for (int i = 0; i < count; i++)
            {
                z[zOff + i] = Interleave5(x[xOff + i]);
            }
        }

        private static long Interleave7(long x)
        {
            long num = x & -9223372036854775808L;
            return (((((((num | INTERLEAVE7_TABLE[((int) x) & 0x1ff]) | (INTERLEAVE7_TABLE[((int) (x >> 9)) & 0x1ff] << 1)) | (INTERLEAVE7_TABLE[((int) (x >> 0x12)) & 0x1ff] << 2)) | (INTERLEAVE7_TABLE[((int) (x >> 0x1b)) & 0x1ff] << 3)) | (INTERLEAVE7_TABLE[((int) (x >> 0x24)) & 0x1ff] << 4)) | (INTERLEAVE7_TABLE[((int) (x >> 0x2d)) & 0x1ff] << 5)) | (INTERLEAVE7_TABLE[((int) (x >> 0x36)) & 0x1ff] << 6));
        }

        private static void Interleave7(long[] x, int xOff, long[] z, int zOff, int count)
        {
            for (int i = 0; i < count; i++)
            {
                z[zOff + i] = Interleave7(x[xOff + i]);
            }
        }

        public bool IsOne()
        {
            long[] ints = this.m_ints;
            if (ints[0] != 1L)
            {
                return false;
            }
            for (int i = 1; i < ints.Length; i++)
            {
                if (ints[i] != 0L)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsZero()
        {
            long[] ints = this.m_ints;
            for (int i = 0; i < ints.Length; i++)
            {
                if (ints[i] != 0L)
                {
                    return false;
                }
            }
            return true;
        }

        public LongArray ModInverse(int m, int[] ks)
        {
            int num = this.Degree();
            switch (num)
            {
                case 0:
                    throw new InvalidOperationException();

                case 1:
                    return this;
            }
            LongArray array = this.Copy();
            int intLen = (m + 0x3f) >> 6;
            LongArray array2 = new LongArray(intLen);
            ReduceBit(array2.m_ints, 0, m, m, ks);
            LongArray array3 = new LongArray(intLen);
            array3.m_ints[0] = 1L;
            LongArray array4 = new LongArray(intLen);
            int[] numArray = new int[] { num, m + 1 };
            LongArray[] arrayArray = new LongArray[] { array, array2 };
            int[] numArray3 = new int[2];
            numArray3[0] = 1;
            int[] numArray2 = numArray3;
            LongArray[] arrayArray2 = new LongArray[] { array3, array4 };
            int index = 1;
            int limit = numArray[index];
            int num5 = numArray2[index];
            int bits = limit - numArray[1 - index];
            while (true)
            {
                if (bits < 0)
                {
                    bits = -bits;
                    numArray[index] = limit;
                    numArray2[index] = num5;
                    index = 1 - index;
                    limit = numArray[index];
                    num5 = numArray2[index];
                }
                arrayArray[index].AddShiftedByBitsSafe(arrayArray[1 - index], numArray[1 - index], bits);
                int num7 = arrayArray[index].DegreeFrom(limit);
                if (num7 == 0)
                {
                    return arrayArray2[1 - index];
                }
                int otherDegree = numArray2[1 - index];
                arrayArray2[index].AddShiftedByBitsSafe(arrayArray2[1 - index], otherDegree, bits);
                otherDegree += bits;
                if (otherDegree > num5)
                {
                    num5 = otherDegree;
                }
                else if (otherDegree == num5)
                {
                    num5 = arrayArray2[index].DegreeFrom(num5);
                }
                bits += num7 - limit;
                limit = num7;
            }
        }

        public LongArray ModMultiply(LongArray other, int m, int[] ks)
        {
            int num = this.Degree();
            if (num == 0)
            {
                return this;
            }
            int num2 = other.Degree();
            if (num2 == 0)
            {
                return other;
            }
            LongArray array = this;
            LongArray array2 = other;
            if (num > num2)
            {
                array = other;
                array2 = this;
                int num3 = num;
                num = num2;
                num2 = num3;
            }
            int num4 = (num + 0x3f) >> 6;
            int bLen = (num2 + 0x3f) >> 6;
            int len = ((num + num2) + 0x3e) >> 6;
            if (num4 == 1)
            {
                long a = array.m_ints[0];
                if (a == 1L)
                {
                    return array2;
                }
                long[] c = new long[len];
                MultiplyWord(a, array2.m_ints, bLen, c, 0);
                return ReduceResult(c, 0, len, m, ks);
            }
            int count = ((num2 + 7) + 0x3f) >> 6;
            int[] numArray2 = new int[0x10];
            long[] destinationArray = new long[count << 4];
            int destinationIndex = count;
            numArray2[1] = destinationIndex;
            Array.Copy(array2.m_ints, 0, destinationArray, destinationIndex, bLen);
            for (int i = 2; i < 0x10; i++)
            {
                numArray2[i] = destinationIndex += count;
                if ((i & 1) == 0)
                {
                    ShiftUp(destinationArray, destinationIndex >> 1, destinationArray, destinationIndex, count, 1);
                }
                else
                {
                    Add(destinationArray, count, destinationArray, destinationIndex - count, destinationArray, destinationIndex, count);
                }
            }
            long[] z = new long[destinationArray.Length];
            ShiftUp(destinationArray, 0, z, 0, destinationArray.Length, 4);
            long[] ints = array.m_ints;
            long[] x = new long[len << 3];
            int num11 = 15;
            for (int j = 0; j < num4; j++)
            {
                long num13 = ints[j];
                int xOff = j;
                while (true)
                {
                    int index = ((int) num13) & num11;
                    num13 = num13 >> 4;
                    int num16 = ((int) num13) & num11;
                    AddBoth(x, xOff, destinationArray, numArray2[index], z, numArray2[num16], count);
                    num13 = num13 >> 4;
                    if (num13 == 0L)
                    {
                        break;
                    }
                    xOff += len;
                }
            }
            int length = x.Length;
            while ((length -= len) != 0)
            {
                AddShiftedUp(x, length - len, x, length, len, 8);
            }
            return ReduceResult(x, 0, len, m, ks);
        }

        public LongArray ModMultiplyAlt(LongArray other, int m, int[] ks)
        {
            int num = this.Degree();
            if (num == 0)
            {
                return this;
            }
            int num2 = other.Degree();
            if (num2 == 0)
            {
                return other;
            }
            LongArray array = this;
            LongArray array2 = other;
            if (num > num2)
            {
                array = other;
                array2 = this;
                int num3 = num;
                num = num2;
                num2 = num3;
            }
            int count = (num + 0x3f) >> 6;
            int bLen = (num2 + 0x3f) >> 6;
            int len = ((num + num2) + 0x3e) >> 6;
            if (count == 1)
            {
                long a = array.m_ints[0];
                if (a == 1L)
                {
                    return array2;
                }
                long[] c = new long[len];
                MultiplyWord(a, array2.m_ints, bLen, c, 0);
                return ReduceResult(c, 0, len, m, ks);
            }
            int width = 4;
            int shift = 0x10;
            int num10 = 0x40;
            int num11 = 8;
            int num12 = (num10 >= 0x40) ? (shift - 1) : shift;
            int num13 = ((num2 + num12) + 0x3f) >> 6;
            int num14 = num13 * num11;
            int num15 = width * num11;
            int[] numArray2 = new int[((int) 1) << width];
            int num16 = count;
            numArray2[0] = num16;
            num16 += num14;
            numArray2[1] = num16;
            for (int i = 2; i < numArray2.Length; i++)
            {
                num16 += len;
                numArray2[i] = num16;
            }
            num16 += len;
            num16++;
            long[] z = new long[num16];
            Interleave(array.m_ints, 0, z, 0, count, width);
            int destinationIndex = count;
            Array.Copy(array2.m_ints, 0, z, destinationIndex, bLen);
            for (int j = 1; j < num11; j++)
            {
                ShiftUp(z, count, z, destinationIndex += num13, num13, j);
            }
            int num20 = (((int) 1) << width) - 1;
            int num21 = 0;
            while (true)
            {
                int index = 0;
                do
                {
                    long num23 = z[index] >> num21;
                    int num24 = 0;
                    int yOff = count;
                    while (true)
                    {
                        int num26 = ((int) num23) & num20;
                        if (num26 != 0)
                        {
                            Add(z, index + numArray2[num26], z, yOff, num13);
                        }
                        if (++num24 == num11)
                        {
                            break;
                        }
                        yOff += num13;
                        num23 = num23 >> width;
                    }
                }
                while (++index < count);
                if ((num21 += num15) >= num10)
                {
                    if (num21 >= 0x40)
                    {
                        int length = numArray2.Length;
                        while (--length > 1)
                        {
                            if ((length & 1L) == 0L)
                            {
                                AddShiftedUp(z, numArray2[length >> 1], z, numArray2[length], len, shift);
                            }
                            else
                            {
                                Distribute(z, numArray2[length], numArray2[length - 1], numArray2[1], len);
                            }
                        }
                        return ReduceResult(z, numArray2[1], len, m, ks);
                    }
                    num21 = 0x40 - width;
                    num20 &= num20 << (num10 - num21);
                }
                ShiftUp(z, count, num14, num11);
            }
        }

        public LongArray ModMultiplyLD(LongArray other, int m, int[] ks)
        {
            int num = this.Degree();
            if (num == 0)
            {
                return this;
            }
            int num2 = other.Degree();
            if (num2 == 0)
            {
                return other;
            }
            LongArray array = this;
            LongArray array2 = other;
            if (num > num2)
            {
                array = other;
                array2 = this;
                int num3 = num;
                num = num2;
                num2 = num3;
            }
            int num4 = (num + 0x3f) >> 6;
            int bLen = (num2 + 0x3f) >> 6;
            int len = ((num + num2) + 0x3e) >> 6;
            if (num4 == 1)
            {
                long a = array.m_ints[0];
                if (a == 1L)
                {
                    return array2;
                }
                long[] c = new long[len];
                MultiplyWord(a, array2.m_ints, bLen, c, 0);
                return ReduceResult(c, 0, len, m, ks);
            }
            int count = ((num2 + 7) + 0x3f) >> 6;
            int[] numArray2 = new int[0x10];
            long[] destinationArray = new long[count << 4];
            int destinationIndex = count;
            numArray2[1] = destinationIndex;
            Array.Copy(array2.m_ints, 0, destinationArray, destinationIndex, bLen);
            for (int i = 2; i < 0x10; i++)
            {
                numArray2[i] = destinationIndex += count;
                if ((i & 1) == 0)
                {
                    ShiftUp(destinationArray, destinationIndex >> 1, destinationArray, destinationIndex, count, 1);
                }
                else
                {
                    Add(destinationArray, count, destinationArray, destinationIndex - count, destinationArray, destinationIndex, count);
                }
            }
            long[] z = new long[destinationArray.Length];
            ShiftUp(destinationArray, 0, z, 0, destinationArray.Length, 4);
            long[] ints = array.m_ints;
            long[] x = new long[len];
            int num11 = 15;
            for (int j = 0x38; j >= 0; j -= 8)
            {
                for (int n = 1; n < num4; n += 2)
                {
                    int num14 = (int) (ints[n] >> j);
                    int index = num14 & num11;
                    int num16 = (num14 >> 4) & num11;
                    AddBoth(x, n - 1, destinationArray, numArray2[index], z, numArray2[num16], count);
                }
                ShiftUp(x, 0, len, 8);
            }
            for (int k = 0x38; k >= 0; k -= 8)
            {
                for (int n = 0; n < num4; n += 2)
                {
                    int num19 = (int) (ints[n] >> k);
                    int index = num19 & num11;
                    int num21 = (num19 >> 4) & num11;
                    AddBoth(x, n, destinationArray, numArray2[index], z, numArray2[num21], count);
                }
                if (k > 0)
                {
                    ShiftUp(x, 0, len, 8);
                }
            }
            return ReduceResult(x, 0, len, m, ks);
        }

        public LongArray ModReduce(int m, int[] ks)
        {
            long[] ints = Arrays.Clone(this.m_ints);
            return new LongArray(ints, 0, ReduceInPlace(ints, 0, ints.Length, m, ks));
        }

        public LongArray ModSquare(int m, int[] ks)
        {
            int usedLength = this.GetUsedLength();
            if (usedLength == 0)
            {
                return this;
            }
            int num2 = usedLength << 1;
            long[] ints = new long[num2];
            int num3 = 0;
            while (num3 < num2)
            {
                long num4 = this.m_ints[num3 >> 1];
                ints[num3++] = Interleave2_32to64((int) num4);
                ints[num3++] = Interleave2_32to64((int) (num4 >> 0x20));
            }
            return new LongArray(ints, 0, ReduceInPlace(ints, 0, ints.Length, m, ks));
        }

        public LongArray ModSquareN(int n, int m, int[] ks)
        {
            int usedLength = this.GetUsedLength();
            if (usedLength == 0)
            {
                return this;
            }
            int num2 = (m + 0x3f) >> 6;
            long[] destinationArray = new long[num2 << 1];
            Array.Copy(this.m_ints, 0, destinationArray, 0, usedLength);
            while (--n >= 0)
            {
                SquareInPlace(destinationArray, usedLength, m, ks);
                usedLength = ReduceInPlace(destinationArray, 0, destinationArray.Length, m, ks);
            }
            return new LongArray(destinationArray, 0, usedLength);
        }

        public LongArray Multiply(LongArray other, int m, int[] ks)
        {
            int num = this.Degree();
            if (num == 0)
            {
                return this;
            }
            int num2 = other.Degree();
            if (num2 == 0)
            {
                return other;
            }
            LongArray array = this;
            LongArray array2 = other;
            if (num > num2)
            {
                array = other;
                array2 = this;
                int num3 = num;
                num = num2;
                num2 = num3;
            }
            int num4 = (num + 0x3f) >> 6;
            int bLen = (num2 + 0x3f) >> 6;
            int len = ((num + num2) + 0x3e) >> 6;
            if (num4 == 1)
            {
                long a = array.m_ints[0];
                if (a == 1L)
                {
                    return array2;
                }
                long[] c = new long[len];
                MultiplyWord(a, array2.m_ints, bLen, c, 0);
                return new LongArray(c, 0, len);
            }
            int count = ((num2 + 7) + 0x3f) >> 6;
            int[] numArray2 = new int[0x10];
            long[] destinationArray = new long[count << 4];
            int destinationIndex = count;
            numArray2[1] = destinationIndex;
            Array.Copy(array2.m_ints, 0, destinationArray, destinationIndex, bLen);
            for (int i = 2; i < 0x10; i++)
            {
                numArray2[i] = destinationIndex += count;
                if ((i & 1) == 0)
                {
                    ShiftUp(destinationArray, destinationIndex >> 1, destinationArray, destinationIndex, count, 1);
                }
                else
                {
                    Add(destinationArray, count, destinationArray, destinationIndex - count, destinationArray, destinationIndex, count);
                }
            }
            long[] z = new long[destinationArray.Length];
            ShiftUp(destinationArray, 0, z, 0, destinationArray.Length, 4);
            long[] ints = array.m_ints;
            long[] x = new long[len << 3];
            int num11 = 15;
            for (int j = 0; j < num4; j++)
            {
                long num13 = ints[j];
                int xOff = j;
                while (true)
                {
                    int index = ((int) num13) & num11;
                    num13 = num13 >> 4;
                    int num16 = ((int) num13) & num11;
                    AddBoth(x, xOff, destinationArray, numArray2[index], z, numArray2[num16], count);
                    num13 = num13 >> 4;
                    if (num13 == 0L)
                    {
                        break;
                    }
                    xOff += len;
                }
            }
            int length = x.Length;
            while ((length -= len) != 0)
            {
                AddShiftedUp(x, length - len, x, length, len, 8);
            }
            return new LongArray(x, 0, len);
        }

        private static void MultiplyWord(long a, long[] b, int bLen, long[] c, int cOff)
        {
            if ((a & 1L) != 0L)
            {
                Add(c, cOff, b, 0, bLen);
            }
            for (int i = 1; (a = a >> 1) != 0L; i++)
            {
                if ((a & 1L) != 0L)
                {
                    long num2 = AddShiftedUp(c, cOff, b, 0, bLen, i);
                    if (num2 != 0L)
                    {
                        c[cOff + bLen] ^= num2;
                    }
                }
            }
        }

        public void Reduce(int m, int[] ks)
        {
            long[] ints = this.m_ints;
            int length = ReduceInPlace(ints, 0, ints.Length, m, ks);
            if (length < ints.Length)
            {
                this.m_ints = new long[length];
                Array.Copy(ints, 0, this.m_ints, 0, length);
            }
        }

        private static void ReduceBit(long[] buf, int off, int bit, int m, int[] ks)
        {
            FlipBit(buf, off, bit);
            int n = bit - m;
            int length = ks.Length;
            while (--length >= 0)
            {
                FlipBit(buf, off, ks[length] + n);
            }
            FlipBit(buf, off, n);
        }

        private static void ReduceBitWise(long[] buf, int off, int BitLength, int m, int[] ks)
        {
            while (--BitLength >= m)
            {
                if (TestBit(buf, off, BitLength))
                {
                    ReduceBit(buf, off, BitLength, m, ks);
                }
            }
        }

        private static int ReduceInPlace(long[] buf, int off, int len, int m, int[] ks)
        {
            int num = (m + 0x3f) >> 6;
            if (len < num)
            {
                return len;
            }
            int bitLength = Math.Min((int) (len << 6), (int) ((m << 1) - 1));
            int num3 = (len << 6) - bitLength;
            while (num3 >= 0x40)
            {
                len--;
                num3 -= 0x40;
            }
            int length = ks.Length;
            int num5 = ks[length - 1];
            int num6 = (length <= 1) ? 0 : ks[length - 2];
            int toBit = Math.Max(m, num5 + 0x40);
            int num8 = (num3 + Math.Min((int) (bitLength - toBit), (int) (m - num6))) >> 6;
            if (num8 > 1)
            {
                int words = len - num8;
                ReduceVectorWise(buf, off, len, words, m, ks);
                while (len > words)
                {
                    buf[off + --len] = 0L;
                }
                bitLength = words << 6;
            }
            if (bitLength > toBit)
            {
                ReduceWordWise(buf, off, len, toBit, m, ks);
                bitLength = toBit;
            }
            if (bitLength > m)
            {
                ReduceBitWise(buf, off, bitLength, m, ks);
            }
            return num;
        }

        private static LongArray ReduceResult(long[] buf, int off, int len, int m, int[] ks) => 
            new LongArray(buf, off, ReduceInPlace(buf, off, len, m, ks));

        private static void ReduceVectorWise(long[] buf, int off, int len, int words, int m, int[] ks)
        {
            int bits = (words << 6) - m;
            int length = ks.Length;
            while (--length >= 0)
            {
                FlipVector(buf, off, buf, off + words, len - words, bits + ks[length]);
            }
            FlipVector(buf, off, buf, off + words, len - words, bits);
        }

        private static void ReduceWord(long[] buf, int off, int bit, long word, int m, int[] ks)
        {
            int num = bit - m;
            int length = ks.Length;
            while (--length >= 0)
            {
                FlipWord(buf, off, num + ks[length], word);
            }
            FlipWord(buf, off, num, word);
        }

        private static void ReduceWordWise(long[] buf, int off, int len, int toBit, int m, int[] ks)
        {
            int num = toBit >> 6;
            while (--len > num)
            {
                long num2 = buf[off + len];
                if (num2 != 0L)
                {
                    buf[off + len] = 0L;
                    ReduceWord(buf, off, len << 6, num2, m, ks);
                }
            }
            int num3 = toBit & 0x3f;
            long word = buf[off + num] >> num3;
            if (word != 0L)
            {
                buf[off + num] ^= word << num3;
                ReduceWord(buf, off, toBit, word, m, ks);
            }
        }

        private long[] ResizedInts(int newLen)
        {
            long[] destinationArray = new long[newLen];
            Array.Copy(this.m_ints, 0, destinationArray, 0, Math.Min(this.m_ints.Length, newLen));
            return destinationArray;
        }

        private static long ShiftUp(long[] x, int xOff, int count, int shift)
        {
            int num = 0x40 - shift;
            long num2 = 0L;
            for (int i = 0; i < count; i++)
            {
                long num4 = x[xOff + i];
                x[xOff + i] = (num4 << shift) | num2;
                num2 = num4 >> num;
            }
            return num2;
        }

        private static long ShiftUp(long[] x, int xOff, long[] z, int zOff, int count, int shift)
        {
            int num = 0x40 - shift;
            long num2 = 0L;
            for (int i = 0; i < count; i++)
            {
                long num4 = x[xOff + i];
                z[zOff + i] = (num4 << shift) | num2;
                num2 = num4 >> num;
            }
            return num2;
        }

        public LongArray Square(int m, int[] ks)
        {
            int usedLength = this.GetUsedLength();
            if (usedLength == 0)
            {
                return this;
            }
            int num2 = usedLength << 1;
            long[] ints = new long[num2];
            int num3 = 0;
            while (num3 < num2)
            {
                long num4 = this.m_ints[num3 >> 1];
                ints[num3++] = Interleave2_32to64((int) num4);
                ints[num3++] = Interleave2_32to64((int) (num4 >> 0x20));
            }
            return new LongArray(ints, 0, ints.Length);
        }

        private static void SquareInPlace(long[] x, int xLen, int m, int[] ks)
        {
            int num = xLen << 1;
            while (--xLen >= 0)
            {
                long num2 = x[xLen];
                x[--num] = Interleave2_32to64((int) (num2 >> 0x20));
                x[--num] = Interleave2_32to64((int) num2);
            }
        }

        private static bool TestBit(long[] buf, int off, int n)
        {
            int num = n >> 6;
            int num2 = n & 0x3f;
            long num3 = ((long) 1L) << num2;
            return ((buf[off + num] & num3) != 0L);
        }

        public bool TestBitZero() => 
            ((this.m_ints.Length > 0) && ((this.m_ints[0] & 1L) != 0L));

        public BigInteger ToBigInteger()
        {
            int usedLength = this.GetUsedLength();
            if (usedLength == 0)
            {
                return BigInteger.Zero;
            }
            long num2 = this.m_ints[usedLength - 1];
            byte[] buffer = new byte[8];
            int num3 = 0;
            bool flag = false;
            for (int i = 7; i >= 0; i--)
            {
                byte num5 = (byte) (num2 >> (8 * i));
                if (flag || (num5 != 0))
                {
                    flag = true;
                    buffer[num3++] = num5;
                }
            }
            int num6 = (8 * (usedLength - 1)) + num3;
            byte[] bytes = new byte[num6];
            for (int j = 0; j < num3; j++)
            {
                bytes[j] = buffer[j];
            }
            for (int k = usedLength - 2; k >= 0; k--)
            {
                long num9 = this.m_ints[k];
                for (int m = 7; m >= 0; m--)
                {
                    bytes[num3++] = (byte) (num9 >> (8 * m));
                }
            }
            return new BigInteger(1, bytes);
        }

        public override string ToString()
        {
            int usedLength = this.GetUsedLength();
            if (usedLength == 0)
            {
                return "0";
            }
            StringBuilder builder = new StringBuilder(Convert.ToString(this.m_ints[--usedLength], 2));
            while (--usedLength >= 0)
            {
                string str = Convert.ToString(this.m_ints[usedLength], 2);
                int length = str.Length;
                if (length < 0x40)
                {
                    builder.Append("0000000000000000000000000000000000000000000000000000000000000000".Substring(length));
                }
                builder.Append(str);
            }
            return builder.ToString();
        }

        public int Length =>
            this.m_ints.Length;
    }
}

