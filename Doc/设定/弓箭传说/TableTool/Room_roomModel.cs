namespace TableTool
{
    using System;

    public class Room_roomModel : LocalModel<Room_room, int>
    {
        private const string _Filename = "Room_room";

        protected override int GetBeanKey(Room_room bean) => 
            bean.RoomID;

        protected override string Filename =>
            "Room_room";
    }
}

