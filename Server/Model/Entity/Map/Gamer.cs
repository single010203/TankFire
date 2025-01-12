﻿namespace Model
{
    [ObjectEvent]
    public class GamerEvent : ObjectEvent<Gamer>, IAwake<long>
    {
        public void Awake(long id)
        {
            this.Get().Awake(id);
        }
    }

    public sealed class Gamer : Entity
    {
        public long UserID { get; private set; }
        public bool IsReady { get; set; }
        public bool isOffline { get; set; }
        public Unit unit { get; set; }
        public void Awake(long id)
        {
            this.UserID = id;
        }
    }
}
