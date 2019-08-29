namespace TableTool
{
    using System;

    public class Room_eventdemontext2loseModel : LocalModel<Room_eventdemontext2lose, int>
    {
        private const string _Filename = "Room_eventdemontext2lose";

        protected override int GetBeanKey(Room_eventdemontext2lose bean) => 
            bean.EventID;

        protected override string Filename =>
            "Room_eventdemontext2lose";
    }
}

