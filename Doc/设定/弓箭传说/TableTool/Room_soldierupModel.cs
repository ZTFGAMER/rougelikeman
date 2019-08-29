namespace TableTool
{
    using System;

    public class Room_soldierupModel : LocalModel<Room_soldierup, int>
    {
        private const string _Filename = "Room_soldierup";

        protected override int GetBeanKey(Room_soldierup bean) => 
            bean.RoomID;

        protected override string Filename =>
            "Room_soldierup";
    }
}

