// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using IdentityServer.Admin.BusinessLogic.Dtos.Configuration;
using Skoruba.AuditLogging.Events;

namespace IdentityServer.Admin.BusinessLogic.Events.ApiScope
{
    public class ApiScopeAddedEvent : AuditEvent
    {
        public ApiScopeDto ApiScope { get; set; }

        public ApiScopeAddedEvent(ApiScopeDto apiScope)
        {
            ApiScope = apiScope;
        }
    }
}