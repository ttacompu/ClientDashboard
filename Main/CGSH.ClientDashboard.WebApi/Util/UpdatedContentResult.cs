﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace CGSH.ClientDashboard.WebApi.Util
{
    public class UpdatedContentResult : IHttpActionResult
    {
        HttpRequestMessage _request;
        dynamic _content;

        public UpdatedContentResult(HttpRequestMessage request, Object content=null) {
            _request = request;
            _content = content;
        }
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_request.CreateResponse(System.Net.HttpStatusCode.Accepted, (object)_content));
        }
    }

    public abstract class CommonApiController : ApiController
    {
        public class ForbiddenResult : IHttpActionResult
        {
            private readonly HttpRequestMessage _request;
            private readonly string _reason;

            public ForbiddenResult(HttpRequestMessage request, string reason)
            {
                _request = request;
                _reason = reason;
            }

            public ForbiddenResult(HttpRequestMessage request)
            {
                _request = request;
                _reason = "Forbidden";
            }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                var response = _request.CreateResponse(HttpStatusCode.Forbidden, _reason);
                return Task.FromResult(response);
            }
        }

        public class NoContentResult : IHttpActionResult
        {
            private readonly HttpRequestMessage _request;
            private readonly string _reason;

            public NoContentResult(HttpRequestMessage request, string reason)
            {
                _request = request;
                _reason = reason;
            }

            public NoContentResult(HttpRequestMessage request)
            {
                _request = request;
                _reason = "No Content";
            }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                var response = _request.CreateResponse(HttpStatusCode.NoContent, _reason);
                return Task.FromResult(response);
            }
        }
    }
}