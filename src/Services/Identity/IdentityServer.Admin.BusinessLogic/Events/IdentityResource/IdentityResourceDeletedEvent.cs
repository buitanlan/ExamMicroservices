// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using IdentityServer.Admin.BusinessLogic.Dtos.Configuration;
using Skoruba.AuditLogging.Events;

namespace IdentityServer.Admin.BusinessLogic.Events.IdentityResource
{
    public class IdentityResourceDeletedEvent : AuditEvent
    {
        public IdentityResourceDto IdentityResource { get; set; }

        public IdentityResourceDeletedEvent(IdentityResourceDto identityResource)
        {
            IdentityResource = identityResource;
        }
    }
}