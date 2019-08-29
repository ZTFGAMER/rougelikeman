namespace Umeng
{
    using System;

    public static class JSON
    {
        public static JSONNode Parse(string aJSON) => 
            JSONNode.Parse(aJSON);
    }
}

