﻿using System;
using System.Threading.Tasks;

namespace NCS.DSS.GDPRUpdateCustomerInformation.Cosmos.Helper
{
    public interface IResourceHelper
    {
        Task<bool> DoesCustomerExist(Guid customerId);
        bool IsCustomerReadOnly();
    }
}