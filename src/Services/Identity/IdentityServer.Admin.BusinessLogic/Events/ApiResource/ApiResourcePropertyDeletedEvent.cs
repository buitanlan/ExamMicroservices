// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using IdentityServer.Admin.BusinessLogic.Dtos.Configuration;
using Skoruba.AuditLogging.Events;

namespace IdentityServer.Admin.BusinessLogic.Events.ApiResource
{
    public class ApiResourcePropertyDeletedEvent : AuditEvent
    {
        public ApiResourcePropertyDeletedEvent(ApiResourcePropertiesDto apiResourceProperty)
        {
            ApiResourceProperty = apiResourceProperty;
        }

        public ApiResourcePropertiesDto ApiResourceProperty { get; set; }
    }
}