namespace Org.BouncyCastle.Crypto.Tls
{
    using System;

    public abstract class ExtensionType
    {
        public const int server_name = 0;
        public const int max_fragment_length = 1;
        public const int client_certificate_url = 2;
        public const int trusted_ca_keys = 3;
        public const int truncated_hmac = 4;
        public const int status_request = 5;
        public const int user_mapping = 6;
        public const int client_authz = 7;
        public const int server_authz = 8;
        public const int cert_type = 9;
        public const int supported_groups = 10;
        public const int elliptic_curves = 10;
        public const int ec_point_formats = 11;
        public const int srp = 12;
        public const int signature_algorithms = 13;
        public const int use_srtp = 14;
        public const int heartbeat = 15;
        public const int application_layer_protocol_negotiation = 0x10;
        public const int status_request_v2 = 0x11;
        public const int signed_certificate_timestamp = 0x12;
        public const int client_certificate_type = 0x13;
        public const int server_certificate_type = 20;
        public const int padding = 0x15;
        public const int encrypt_then_mac = 0x16;
        public const int extended_master_secret = 0x17;
        public const int session_ticket = 0x23;
        public static readonly int negotiated_ff_dhe_groups = 0x65;
        public const int renegotiation_info = 0xff01;

        protected ExtensionType()
        {
        }
    }
}

