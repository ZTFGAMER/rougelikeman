namespace Org.BouncyCastle.Crypto.Tls
{
    using System;

    public abstract class AlertDescription
    {
        public const byte close_notify = 0;
        public const byte unexpected_message = 10;
        public const byte bad_record_mac = 20;
        public const byte decryption_failed = 0x15;
        public const byte record_overflow = 0x16;
        public const byte decompression_failure = 30;
        public const byte handshake_failure = 40;
        public const byte no_certificate = 0x29;
        public const byte bad_certificate = 0x2a;
        public const byte unsupported_certificate = 0x2b;
        public const byte certificate_revoked = 0x2c;
        public const byte certificate_expired = 0x2d;
        public const byte certificate_unknown = 0x2e;
        public const byte illegal_parameter = 0x2f;
        public const byte unknown_ca = 0x30;
        public const byte access_denied = 0x31;
        public const byte decode_error = 50;
        public const byte decrypt_error = 0x33;
        public const byte export_restriction = 60;
        public const byte protocol_version = 70;
        public const byte insufficient_security = 0x47;
        public const byte internal_error = 80;
        public const byte user_canceled = 90;
        public const byte no_renegotiation = 100;
        public const byte unsupported_extension = 110;
        public const byte certificate_unobtainable = 0x6f;
        public const byte unrecognized_name = 0x70;
        public const byte bad_certificate_status_response = 0x71;
        public const byte bad_certificate_hash_value = 0x72;
        public const byte unknown_psk_identity = 0x73;
        public const byte inappropriate_fallback = 0x56;

        protected AlertDescription()
        {
        }

        public static string GetName(byte alertDescription)
        {
            switch (alertDescription)
            {
                case 40:
                    return "handshake_failure";

                case 0x29:
                    return "no_certificate";

                case 0x2a:
                    return "bad_certificate";

                case 0x2b:
                    return "unsupported_certificate";

                case 0x2c:
                    return "certificate_revoked";

                case 0x2d:
                    return "certificate_expired";

                case 0x2e:
                    return "certificate_unknown";

                case 0x2f:
                    return "illegal_parameter";

                case 0x30:
                    return "unknown_ca";

                case 0x31:
                    return "access_denied";

                case 50:
                    return "decode_error";

                case 0x33:
                    return "decrypt_error";

                case 60:
                    return "export_restriction";

                case 110:
                    return "unsupported_extension";

                case 0x6f:
                    return "certificate_unobtainable";

                case 0x70:
                    return "unrecognized_name";

                case 0x71:
                    return "bad_certificate_status_response";

                case 0x72:
                    return "bad_certificate_hash_value";

                case 0x73:
                    return "unknown_psk_identity";

                case 20:
                    return "bad_record_mac";

                case 0x15:
                    return "decryption_failed";

                case 0x16:
                    return "record_overflow";

                case 70:
                    return "protocol_version";

                case 0x47:
                    return "insufficient_security";

                case 0:
                    return "close_notify";

                case 10:
                    return "unexpected_message";

                case 30:
                    return "decompression_failure";

                case 80:
                    return "internal_error";

                case 0x56:
                    return "inappropriate_fallback";

                case 90:
                    return "user_canceled";

                case 100:
                    return "no_renegotiation";
            }
            return "UNKNOWN";
        }

        public static string GetText(byte alertDescription)
        {
            object[] objArray1 = new object[] { GetName(alertDescription), "(", alertDescription, ")" };
            return string.Concat(objArray1);
        }
    }
}

