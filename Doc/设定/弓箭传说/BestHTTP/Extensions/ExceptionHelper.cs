namespace BestHTTP.Extensions
{
    using System;

    public static class ExceptionHelper
    {
        public static Exception ServerClosedTCPStream() => 
            new Exception("TCP Stream closed unexpectedly by the remote server");
    }
}

