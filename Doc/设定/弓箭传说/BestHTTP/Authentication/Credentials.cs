namespace BestHTTP.Authentication
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public sealed class Credentials
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AuthenticationTypes <Type>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <UserName>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Password>k__BackingField;

        public Credentials(string userName, string password) : this(AuthenticationTypes.Unknown, userName, password)
        {
        }

        public Credentials(AuthenticationTypes type, string userName, string password)
        {
            this.Type = type;
            this.UserName = userName;
            this.Password = password;
        }

        public AuthenticationTypes Type { get; private set; }

        public string UserName { get; private set; }

        public string Password { get; private set; }
    }
}

