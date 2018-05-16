using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FairWayTest.Api.UnitTests.Infrastructure
{
    public class StubHttpMessageHandlerBuilder
    {
        private readonly Dictionary<Uri, (string response, HttpStatusCode statusCode)> _uriResponseMap;

        public StubHttpMessageHandlerBuilder()
        {
            _uriResponseMap = new Dictionary<Uri, (string, HttpStatusCode)>();
        }

        public StubHttpMessageHandlerBuilder WithResponse(Uri uri, string response, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            _uriResponseMap.Add(uri, (response, statusCode));

            return this;
        }

        public StubHttpMessageHandler Build()
        {
            return new StubHttpMessageHandler(_uriResponseMap);
        }
    }

    public class StubHttpMessageHandler : HttpMessageHandler
    {
        private readonly Dictionary<Uri, (string response, HttpStatusCode statusCode)> _uriResponseMap;

        public StubHttpMessageHandler(Dictionary<Uri, (string, HttpStatusCode)> uriResponseMap)
        {
            _uriResponseMap = uriResponseMap;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_uriResponseMap.ContainsKey(request.RequestUri))
            {
                var responseMap = _uriResponseMap[request.RequestUri];

                var content = new StringContent(responseMap.response, Encoding.UTF8, "application/json");

                return Task.FromResult(new HttpResponseMessage() { Content = content, StatusCode = responseMap.statusCode });
            }

            throw new Exception("Uri did not match");
        }
    }
}