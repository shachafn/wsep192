using System;

namespace ApplicationCore.Entities
{
    public struct UserIdentifier
    {
        public Guid Guid { get; private set; }
        public bool IsGuest { get; private set; }

        public UserIdentifier(Guid guid, bool isGuest)
        {
            Guid = guid;
            IsGuest = isGuest;
        }
    }
}
