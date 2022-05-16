// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using IdentityServer.Admin.BusinessLogic.Dtos.Key;
using Skoruba.AuditLogging.Events;
using IdentityServer.Admin.BusinessLogic.Dtos.Grant;

namespace IdentityServer.Admin.BusinessLogic.Events.Key
{
    public class KeyDeletedEvent : AuditEvent
    {
        public KeyDto Key { get; set; }

        public KeyDeletedEvent(KeyDto key)
        {
            Key = key;
        }
    }
}