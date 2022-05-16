﻿// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

namespace IdentityServer.Admin.BusinessLogic.Identity.Dtos.Identity.Interfaces
{
    public interface IBaseRoleDto
    {
        object Id { get; }
        bool IsDefaultId();
    }
}