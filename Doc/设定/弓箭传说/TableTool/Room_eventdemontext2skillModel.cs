namespace TableTool
{
    using System;

    public class Room_eventdemontext2skillModel : LocalModel<Room_eventdemontext2skill, int>
    {
        private const string _Filename = "Room_eventdemontext2skill";

        protected override int GetBeanKey(Room_eventdemontext2skill bean) => 
            bean.EventID;

        protected override string Filename =>
            "Room_eventdemontext2skill";
    }
}

