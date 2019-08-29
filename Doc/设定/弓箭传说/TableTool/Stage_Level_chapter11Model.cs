namespace TableTool
{
    using System;

    public class Stage_Level_chapter11Model : LocalModel<Stage_Level_chapter11, string>
    {
        private const string _Filename = "Stage_Level_chapter11";

        protected override string GetBeanKey(Stage_Level_chapter11 bean) => 
            bean.RoomID;

        protected override string Filename =>
            "Stage_Level_chapter11";
    }
}

