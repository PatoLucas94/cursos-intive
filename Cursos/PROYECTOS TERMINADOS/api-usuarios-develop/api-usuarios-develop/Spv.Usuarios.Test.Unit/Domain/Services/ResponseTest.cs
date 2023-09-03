using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Spv.Usuarios.Domain.Services;
using Spv.Usuarios.Domain.Utils;
using Xunit;
using Xunit.Sdk;

namespace Spv.Usuarios.Test.Unit.Domain.Services
{
    public class ResponseTest
    {
        [Fact]
        public void SeCreanLosResponses()
        {
            // Arrange
            var headers = new Dictionary<string, string>(0);
            var escenarios = new (IResponse<object> sut, int statusCode, bool isOk)[]
            {
                (Responses.Ok("Ok"), StatusCodes.Status200OK, true),
                (Responses.Created("Created"), StatusCodes.Status201Created, true),
                (Responses.Accepted("Accepted"), StatusCodes.Status202Accepted, true),
                (Responses.Ok(StatusCodes.Status202Accepted, new Dictionary<string, string>(0), "Ok"), StatusCodes.Status202Accepted, true),
                (Responses.Conflict<string>("Conflict"), StatusCodes.Status409Conflict, false),
                (Responses.NotFound<string>("NotFound"), StatusCodes.Status404NotFound, false),
                (Responses.PageNotFound<string>("NotFound"), StatusCodes.Status404NotFound, false),
                (Responses.InternalServerError<string>(new Exception("InternalServerError")), StatusCodes.Status500InternalServerError, false),
                (Responses.ServerError<string>(StatusCodes.Status503ServiceUnavailable, new Dictionary<string, string>(0), new Exception("InternalServerError")), StatusCodes.Status503ServiceUnavailable, false)
            };

            // Act - Assert
            foreach (var (sut, statusCode, isOk) in escenarios)
            {
                sut.Should().NotBeNull();
                sut.IsOk.Should().Be(isOk);
                sut.StatusCode.Should().Be(statusCode);
                AssertHeaders(sut, headers);
            }
        }

        [Fact]
        public void OkConHeaders()
        {
            // Arrange
            var headers = new Dictionary<string, string> { { "h1", "v1" }, { "h2", "v2" } };

            // Act
            var sut = Responses.Ok("Ok", headers);

            // Assert
            sut.IsOk.Should().BeTrue();
            sut.StatusCode.Should().Be(200);
            AssertHeaders(sut, headers);
        }

        [Fact]
        public void PaginationEsOk()
        {
            // Arrange
            static IReadOnlyDictionary<string, string> Headers(int totalNumberOfRecords) => new Dictionary<string, string> { { "X-Total-Count", $"{totalNumberOfRecords}" } };

            var escenarios = new (IReadOnlyCollection<string> data, int totalNumberOfRecords, int statusCode)[]
            {
                (new[] {"Ok"}, 1, 200),
                (new string[0], 1, 204),
                (new[] {"Ok"}, 5, 206),
            };

            // Act - Assert
            foreach (var (data, totalNumberOfRecords, statusCode) in escenarios)
            {
                var sut = Responses.Page(data, totalNumberOfRecords);

                sut.IsOk.Should().BeTrue();
                sut.StatusCode.Should().Be(statusCode);
                AssertHeaders(sut, Headers(totalNumberOfRecords));
            }
        }

        [Fact]
        public void PageNotFoundEs404()
        {
            // Arrange - Act
            var sut = Responses.PageNotFound<string>("Not found");

            // Assert
            sut.IsOk.Should().BeFalse();
            sut.StatusCode.Should().Be(404);
        }

        [Fact]
        public void OnOkEsInvocadoParaOkResponses()
        {
            var run = false;
            void Handle(object o) => run = true;

            foreach (var response in OkResponses())
            {
                run = false;
                response.OnOk(Handle);
                run.Should().BeTrue($"{response} debió ejecutar el método pasado a OnOk");
            }
        }

        [Fact]
        public void OnOkRequiereHandler()
        {
            foreach (var response in AllResponses())
            {
                AssertActionArg<ArgumentNullException>(() => response.OnOk(null), "onOk");
            }
        }

        [Fact]
        public void AcceptInvocaLaAccionAdecuada()
        {
            var run = false;
            void Handle(object o) => run = true;

            foreach (var response in OkResponses())
            {
                run = false;
                response.Accept(Handle, Fail("onClientError"), Fail("onServerError"));
                run.Should().BeTrue($"{response} debió ejecutar el método pasado a OnOk");
            }

            foreach (var response in ClientErrorResponses())
            {
                run = false;
                response.Accept(Fail("onOk"), Handle, Fail("onServerError"));
                run.Should().BeTrue($"{response} debió ejecutar el método pasado a Accept");
            }

            foreach (var response in ServerErrorResponses())
            {
                run = false;
                response.Accept(Fail("onOk"), Fail("onClientError"), Handle);
                run.Should().BeTrue($"{response} debió ejecutar el método pasado a Accept");
            }
        }

        [Fact]
        public void AcceptRequiereHandlers()
        {
            static void Handle(object o) { /* sin código */ }
            foreach (var response in AllResponses())
            {
                AssertActionArg<ArgumentNullException>(() => response.Accept(null, Handle, Handle), "onOk");
                AssertActionArg<ArgumentNullException>(() => response.Accept(Handle, null, Handle), "onClientError");
                AssertActionArg<ArgumentNullException>(() => response.Accept(Handle, Handle, null), "onServerError");
            }
        }

        [Fact]
        public void MatchEvaluaLaFuncionAdecuada()
        {
            foreach (var response in OkResponses())
            {
                response.Match(MapTo(true), FailMapper<bool>("onClientError"), FailMapper<bool>("onServerError")).Should().BeTrue($"'{response}' should have invoked the correct function in Match");
            }

            foreach (var response in ClientErrorResponses())
            {
                response.Match(FailMapper<bool>("onOk"), MapTo(true), FailMapper<bool>("onServerError")).Should().BeTrue($"'{response}' should have invoked the correct function in Match");
            }

            foreach (var response in ServerErrorResponses())
            {
                response.Match(FailMapper<bool>("onOk"), FailMapper<bool>("onClientError"), MapTo(true)).Should().BeTrue($"'{response}' should have invoked the correct function in Match");
            }
        }

        [Fact]
        public void MatchRequiereCases()
        {
            foreach (var response in AllResponses())
            {
                AssertFuncArg<ArgumentNullException>(() => response.Match(null, MapTo(0), MapTo(0)), "okCase");
                AssertFuncArg<ArgumentNullException>(() => response.Match(MapTo(0), null, MapTo(0)), "clientErrorCase");
                AssertFuncArg<ArgumentNullException>(() => response.Match(MapTo(0), MapTo(0), null), "serverErrorCase");
            }
        }

        [Fact]
        public void MapOkInvocaMapperYRetornaOkConPayloadProducidoPorMapper()
        {
            const string expectedPayload = "Wow";
            foreach (var response in OkResponses())
            {
                var actual = response.Map(MapTo(expectedPayload));
                actual.IsOk.Should().Be(response.IsOk);
                actual.StatusCode.Should().Be(response.StatusCode);
                AssertHeaders(actual, response.Headers);
                actual.Should().BeAssignableTo<IOkResponse<string>>();
                actual.As<IOkResponse<string>>().Payload.Should().Be(expectedPayload);
            }
        }

        [Fact]
        public void MapClientErrorNoLlamaAlMapperYRetornaClientError()
        {
            foreach (var response in ClientErrorResponses())
            {
                var actual = response.Map(FailMapper<string>("mapper"));
                actual.IsOk.Should().Be(response.IsOk);
                actual.StatusCode.Should().Be(response.StatusCode);
                AssertHeaders(actual, response.Headers);
                actual.Should().BeAssignableTo<IClientErrorResponse<string>>();
                actual.As<IClientErrorResponse<string>>().Message.Should().Be(response.Message);
                actual.As<IClientErrorResponse<string>>().ErrorType.Should().Be(response.ErrorType);
                actual.As<IClientErrorResponse<string>>().InternalCode.Should().Be(response.InternalCode);
            }
        }

        [Fact]
        public void MapServerErrorNoLlamaAlMapperYRetornaServerError()
        {
            foreach (var response in ServerErrorResponses())
            {
                var actual = response.Map(FailMapper<string>("mapper"));
                actual.IsOk.Should().Be(response.IsOk);
                actual.StatusCode.Should().Be(response.StatusCode);
                AssertHeaders(actual, response.Headers);
                actual.Should().BeAssignableTo<IServerErrorResponse<string>>();
                actual.As<IServerErrorResponse<string>>().Exception.Should().Be(response.Exception);
            }
        }

        [Fact]
        public void MapRequiereMapper()
        {
            foreach (var response in AllResponses())
            {
                AssertFuncArg<ArgumentNullException>(() => response.Map<bool>(null), "mapper");
            }
        }

        [Fact]
        public void BindOkProduceLaRespuestaQueRetornaBinderConLosHeadersAgregados()
        {
            var headers = new Dictionary<string, string> { { "In", "Rainbows" }, { "The", "Bends" } };

            IEnumerable<Func<object, IResponse<string>>> binders = new[]
            {
                (Func<object, IResponse<string>>) (o => Responses.Ok(StatusCodes.Status200OK, headers, o.ToString())),
                o => Responses.ClientError<string>(StatusCodes.Status400BadRequest, headers, "ClientError"),
                o => Responses.ServerError<string>(StatusCodes.Status500InternalServerError, headers, new Exception("ServerError"))
            };

            foreach (var binder in binders)
            {
                foreach (var response in OkResponses())
                {
                    var expected = binder("ok");
                    var actual = response.Bind(binder);
                    actual.IsOk.Should().Be(expected.IsOk);
                    actual.StatusCode.Should().Be(expected.StatusCode);
                    AssertHeaders(actual, new Dictionary<string, string>(response.Headers.Concat(headers)));
                    actual.Should().BeAssignableTo<IResponse<string>>();
                }
            }
        }

        [Fact]
        public void BindClientErrorNoLlamaAlBinderYRetornaClientError()
        {
            foreach (var response in ClientErrorResponses())
            {
                var actual = response.Bind(FailBinder<string>());
                actual.IsOk.Should().Be(response.IsOk);
                actual.StatusCode.Should().Be(response.StatusCode);
                AssertHeaders(actual, response.Headers);
                actual.Should().BeAssignableTo<IClientErrorResponse<string>>();
                actual.As<IClientErrorResponse<string>>().Message.Should().Be(response.Message);
                actual.As<IClientErrorResponse<string>>().ErrorType.Should().Be(response.ErrorType);
                actual.As<IClientErrorResponse<string>>().InternalCode.Should().Be(response.InternalCode);
            }
        }

        [Fact]
        public void BindServerErrorsNoLlamaAlBinderYRetornaServerError()
        {
            foreach (var response in ServerErrorResponses())
            {
                var actual = response.Bind(FailBinder<string>());
                actual.IsOk.Should().Be(response.IsOk);
                actual.StatusCode.Should().Be(response.StatusCode);
                AssertHeaders(actual, response.Headers);
                actual.Should().BeAssignableTo<IServerErrorResponse<string>>();
                actual.As<IServerErrorResponse<string>>().Exception.Should().Be(response.Exception);
            }
        }

        [Fact]
        public void BindRequiereBinder()
        {
            foreach (var response in AllResponses())
            {
                AssertFuncArg<ArgumentNullException>(() => response.Bind<bool>(null), "binder");
            }
        }

        [Fact]
        public async Task BindAsyncParaOkProduceLaRespuestaQueRetornaBinderConLosHeadersAgregados()
        {
            var headers = new Dictionary<string, string> { { "In", "Rainbows" }, { "The", "Bends" } };

            IEnumerable<Func<object, Task<IResponse<string>>>> binders = new[]
            {
                (Func<object, Task<IResponse<string>>>) (o => Task.FromResult(Responses.Ok(StatusCodes.Status200OK, headers, o.ToString()))),
                o => Task.FromResult(Responses.ClientError<string>(StatusCodes.Status400BadRequest, headers, "ClientError")),
                o => Task.FromResult(Responses.ServerError<string>(StatusCodes.Status500InternalServerError, headers, new Exception("ServerError")))
            };

            foreach (var binder in binders)
            {
                foreach (var response in OkResponses())
                {
                    var expected = await binder("ok");
                    var actual = await response.BindAsync(binder);
                    actual.IsOk.Should().Be(expected.IsOk);
                    actual.StatusCode.Should().Be(expected.StatusCode);
                    AssertHeaders(actual, new Dictionary<string, string>(response.Headers.Concat(headers)));
                    actual.Should().BeAssignableTo<IResponse<string>>();
                }
            }
        }

        [Fact]
        public async Task BindAsyncClientErrorNoLlamaAlBinderYRetornaClientError()
        {
            foreach (var response in ClientErrorResponses())
            {
                var actual = await response.BindAsync(FailAsyncBinder<string>());
                actual.IsOk.Should().Be(response.IsOk);
                actual.StatusCode.Should().Be(response.StatusCode);
                AssertHeaders(actual, response.Headers);

                actual.Should().BeAssignableTo<IClientErrorResponse<string>>();
                actual.As<IClientErrorResponse<string>>().Message.Should().Be(response.Message);
                actual.As<IClientErrorResponse<string>>().ErrorType.Should().Be(response.ErrorType);
                actual.As<IClientErrorResponse<string>>().InternalCode.Should().Be(response.InternalCode);
            }
        }

        [Fact]
        public async Task BindAsyncServerErrorsNoLlamaAlBinderYRetornaServerError()
        {
            foreach (var response in ServerErrorResponses())
            {
                var actual = await response.BindAsync(FailAsyncBinder<string>());
                actual.IsOk.Should().Be(response.IsOk);
                actual.StatusCode.Should().Be(response.StatusCode);
                actual.Should().BeAssignableTo<IServerErrorResponse<string>>();
                actual.As<IServerErrorResponse<string>>().Exception.Should().Be(response.Exception);
                AssertHeaders(actual, response.Headers);
            }
        }

        [Fact]
        public void BindAsyncRequiereBinder()
        {
            foreach (var response in AllResponses())
            {
                AssertFuncArg<ArgumentNullException>(() => response.BindAsync<bool>(null).Result, "binder");
            }
        }

        private static IEnumerable<IResponse<object>> AllResponses() => OkResponses().OfType<IResponse<object>>().Concat(ClientErrorResponses()).Concat(ServerErrorResponses());

        private static IEnumerable<IOkResponse<object>> OkResponses() => new IResponse<object>[]
        {
            Responses.Ok("Ok"),
            Responses.Ok(209, new Dictionary<string, string>{{"Ok", "Computer"}, {"Kid", "A" }}, "Ok"),
            Responses.Created("Created"),
            Responses.Page(new[] {"Ok"}, 1),
            Responses.Page(new string[0], 1),
            Responses.Page(new[] {"Ok"}, 5),
            Responses.Created("Created")
        }.Cast<IOkResponse<object>>();

        private static IEnumerable<IClientErrorResponse<object>> ClientErrorResponses() => new IResponse<object>[]
        {
            Responses.ClientError<string>(StatusCodes.Status409Conflict, new Dictionary<string, string>(0), "Ok"),
            Responses.Conflict<string>("Conflict"),
            Responses.NotFound<string>("NotFound"),
            Responses.PageNotFound<string>("PageNotFound")
        }.Cast<IClientErrorResponse<object>>();

        private static IEnumerable<IServerErrorResponse<object>> ServerErrorResponses() => new IResponse<object>[]
        {
            Responses.ServerError<string>(StatusCodes.Status503ServiceUnavailable, new Dictionary<string, string>(0), new Exception("ServerError")),
            Responses.InternalServerError<string>(new Exception("InternalServerError"))
        }.Cast<IServerErrorResponse<object>>();


        private static Func<object, T> FailMapper<T>(string name = null) => _ =>
        {
            var p = name ?? "mapper";
            throw new XunitException($"'{p}' no debió haber sido invocado");
        };

        private static Func<object, IResponse<T>> FailBinder<T>() => _ =>
        {
            throw new XunitException("'binder' no debió haber sido invocado");
        };

        private static Func<object, Task<IResponse<T>>> FailAsyncBinder<T>() => _ =>
        {
            throw new XunitException("'binder' no debió haber sido invocado");
        };

        private Func<object, T> MapTo<T>(T result) => o => result;

        private static Action<object> Fail(string name) => _ => throw new XunitException($"'{name}' no debió haber sido invocado");


        private static void AssertHeaders(IResponse sut, IReadOnlyDictionary<string, string> expectedHeaders)
        {
            sut.Headers.Should().HaveCount(expectedHeaders.Count, sut.ToString());
            foreach (var (key, value) in expectedHeaders)
            {
                sut.Headers.Keys.Should().Contain(key);
                sut.Headers[key].Should().Be(value);
            }
        }

        private static void AssertActionArg<TException>(Action action, string parameterName) where TException : Exception
        {
            AssertFuncArg<TException>(() => { action(); return false; }, parameterName);
        }

        private static void AssertFuncArg<TException>(Func<object> func, string parameterName) where TException : Exception
        {
            Result.Of(func)
                .OnOk(r => throw new XunitException($"debió haber requerido '{parameterName}'."))
                .OnError(e => e.GetBaseException().Should().BeOfType<TException>())
                .OnError(e => e.Message.Should().Contain($"'{parameterName}'"));
        }
    }
}
