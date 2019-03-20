﻿using ASF.Domain.Entities;
using ASF.Domain.Services;
using ASF.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.Middleware;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ASF
{
    /// <summary>
    /// 日子记录
    /// </summary>
    public class ASFRequestLogger
    {
        private readonly DownstreamContext context;
        private readonly Permission permission;
        private readonly HttpContext httpContext;
        private readonly IServiceProvider serviceProvider;
        public ASFRequestLogger(DownstreamContext context, Permission permission)
        {
            this.context = context;
            this.permission = permission;
            this.httpContext = context.HttpContext;
            this.serviceProvider = httpContext.RequestServices;
        }
        public static Task Record(DownstreamContext context, Permission permission)
        {
            return new ASFRequestLogger(context, permission).Record();
        }
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="httpContext"></param>
        public async Task Record()
        {
            var responseData = await context.DownstreamResponse.Content.ReadAsStringAsync();
            var requestData = await context.DownstreamRequest.ToHttpRequestMessage().Content.ReadAsStringAsync();

            serviceProvider.GetRequiredService<LogOperateRecordService>().Record(permission, requestData, responseData);
            return ;
        }
       
    }
}
