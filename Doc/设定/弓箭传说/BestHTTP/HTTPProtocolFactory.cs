namespace BestHTTP
{
    using BestHTTP.ServerSentEvents;
    using BestHTTP.WebSocket;
    using System;
    using System.IO;

    internal static class HTTPProtocolFactory
    {
        public static HTTPResponse Get(SupportedProtocols protocol, HTTPRequest request, Stream stream, bool isStreamed, bool isFromCache)
        {
            if (protocol != SupportedProtocols.WebSocket)
            {
                if (protocol == SupportedProtocols.ServerSentEvents)
                {
                    return new EventSourceResponse(request, stream, isStreamed, isFromCache);
                }
                return new HTTPResponse(request, stream, isStreamed, isFromCache);
            }
            return new WebSocketResponse(request, stream, isStreamed, isFromCache);
        }

        public static SupportedProtocols GetProtocolFromUri(Uri uri)
        {
            if ((uri == null) || (uri.Scheme == null))
            {
                throw new Exception("Malformed URI in GetProtocolFromUri");
            }
            string str = uri.Scheme.ToLowerInvariant();
            if ((str != null) && ((str == "ws") || (str == "wss")))
            {
                return SupportedProtocols.WebSocket;
            }
            return SupportedProtocols.HTTP;
        }

        public static bool IsSecureProtocol(Uri uri)
        {
            if ((uri == null) || (uri.Scheme == null))
            {
                throw new Exception("Malformed URI in IsSecureProtocol");
            }
            string str = uri.Scheme.ToLowerInvariant();
            return ((str != null) && ((str == "https") || (str == "wss")));
        }
    }
}

