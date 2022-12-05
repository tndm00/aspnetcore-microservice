﻿using Shared.Services.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
    public interface IEmailService<in T> where T : class
    {
        Task SendEmailAsync(T request, CancellationToken cancellationToken = new CancellationToken());
    }
}
