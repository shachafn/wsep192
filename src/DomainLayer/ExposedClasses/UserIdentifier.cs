using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.ExposedClasses
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
