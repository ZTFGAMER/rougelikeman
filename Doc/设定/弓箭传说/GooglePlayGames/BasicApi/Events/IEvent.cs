namespace GooglePlayGames.BasicApi.Events
{
    using System;

    public interface IEvent
    {
        string Id { get; }

        string Name { get; }

        string Description { get; }

        string ImageUrl { get; }

        ulong CurrentCount { get; }

        EventVisibility Visibility { get; }
    }
}

