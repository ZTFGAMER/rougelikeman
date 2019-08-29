namespace TableTool
{
    using System;

    public class Room_levelModel : LocalModel<Room_level, int>
    {
        private const string _Filename = "Room_level";

        protected override int GetBeanKey(Room_level bean) => 
            bean.LevelID;

        protected override string Filename =>
            "Room_level";
    }
}

