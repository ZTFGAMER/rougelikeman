namespace TableTool
{
    using System;

    public class Room_colorstyleModel : LocalModel<Room_colorstyle, int>
    {
        private const string _Filename = "Room_colorstyle";

        protected override int GetBeanKey(Room_colorstyle bean) => 
            bean.ID;

        protected override string Filename =>
            "Room_colorstyle";
    }
}

