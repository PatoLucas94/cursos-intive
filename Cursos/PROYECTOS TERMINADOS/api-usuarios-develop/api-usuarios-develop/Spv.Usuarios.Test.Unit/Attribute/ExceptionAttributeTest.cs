using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Spv.Usuarios.Api.Exceptions;
using Spv.Usuarios.Api.Filters;
using Spv.Usuarios.Common.LogEvents;
using Spv.Usuarios.Domain.Exceptions;
using Spv.Usuarios.Domain.Services;
using Xunit;

namespace Spv.Usuarios.Test.Unit.Attribute
{
    public class ExceptionAttributeTest
    {
        [Fact]
        public void TestExceptionFilterInternalServerError()
        {
            // Arrange
            var actionContext = new ActionContext()
            {
                HttpContext = new DefaultHttpContext(),
                RouteData = new RouteData(),
                ActionDescriptor = new ActionDescriptor()
            };

            var exceptionContext = new ExceptionContext(actionContext, new List<IFilterMetadata>())
            {
                Exception = new Exception("Test Message")
            };

            var filter = new ExceptionsAttribute();

            // Act
            filter.OnException(exceptionContext);

            // Assert
            exceptionContext.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

            var result = (ObjectResult)exceptionContext.Result;

            var errorDetailModel = (ErrorDetailModel)result.Value;

            errorDetailModel.Code.Should().Be(StatusCodes.Status500InternalServerError);
            errorDetailModel.Type.Should().Be(ErrorTypeConstants.Technical);
            errorDetailModel.Detail.Should().Be("Test Message");
            errorDetailModel.Errors.Count.Should().Be(1);

            var error = errorDetailModel.Errors.First();

            error.Detail.Should().Be("");
            error.Title.Should().Be("Test Message");
            error.Code.Should().Be(StatusCodes.Status500InternalServerError.ToString());
        }

        [Fact]
        public void TestExceptionFilterInternalServerErrorConInvalidCastException()
        {
            // Arrange
            var actionContext = new ActionContext()
            {
                HttpContext = new DefaultHttpContext(),
                RouteData = new RouteData(),
                ActionDescriptor = new ActionDescriptor()
            };

            var exceptionContext = new ExceptionContext(actionContext, new List<IFilterMetadata>())
            {
                Exception = new InvalidCastException("Test InvalidCastException Message")
            };

            var filter = new ExceptionsAttribute();

            // Act
            filter.OnException(exceptionContext);

            // Assert
            exceptionContext.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

            var result = (ObjectResult)exceptionContext.Result;

            var errorDetailModel = (ErrorDetailModel)result.Value;

            errorDetailModel.Code.Should().Be(StatusCodes.Status500InternalServerError);
            errorDetailModel.Type.Should().Be(ErrorTypeConstants.Technical);
            errorDetailModel.Detail.Should().Be("Test InvalidCastException Message");
            errorDetailModel.Errors.Count.Should().Be(1);

            var error = errorDetailModel.Errors.First();

            error.Detail.Should().Be("");
            error.Title.Should().Be("Test InvalidCastException Message");
            error.Code.Should().Be(StatusCodes.Status500InternalServerError.ToString());
        }

        [Fact]
        public void TestExceptionFilterBusinessExceptionUsuarioServiceEvents()
        {
            // Arrange
            var eventUsuarioService = UsuarioServiceEvents.ExceptionCallingAutenticacion;
            var actionContext = new ActionContext()
            {
                HttpContext = new DefaultHttpContext(),
                RouteData = new RouteData(),
                ActionDescriptor = new ActionDescriptor()
            };

            var exceptionContext = new ExceptionContext(actionContext, new List<IFilterMetadata>())
            {
                Exception = new BusinessException(eventUsuarioService)
            };

            var filter = new ExceptionsAttribute();

            // Act
            filter.OnException(exceptionContext);

            // Assert
            exceptionContext.HttpContext.Response.StatusCode.Should().Be(eventUsuarioService.Id);

            var result = (ObjectResult)exceptionContext.Result;

            var errorDetailModel = (ErrorDetailModel)result.Value;

            errorDetailModel.Code.Should().Be(eventUsuarioService.Id);
            errorDetailModel.Type.Should().Be(ErrorTypeConstants.Business);
            errorDetailModel.Detail.Should().Be(eventUsuarioService.Name);
            errorDetailModel.Errors.Count.Should().Be(1);

            var error = errorDetailModel.Errors.First();

            error.Detail.Should().Be("");
            error.Title.Should().Be(eventUsuarioService.Name);
            error.Code.Should().Be(eventUsuarioService.Id.ToString());
        }

        [Fact]
        public void TestExceptionFilterBusinessException()
        {
            // Arrange
            var actionContext = new ActionContext()
            {
                HttpContext = new DefaultHttpContext(),
                RouteData = new RouteData(),
                ActionDescriptor = new ActionDescriptor()
            };

            var exceptionContext = new ExceptionContext(actionContext, new List<IFilterMetadata>())
            {
                Exception = new BusinessException("Test message", StatusCodes.Status400BadRequest)
            };

            var filter = new ExceptionsAttribute();

            // Act
            filter.OnException(exceptionContext);

            // Assert
            exceptionContext.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var result = (ObjectResult)exceptionContext.Result;

            var errorDetailModel = (ErrorDetailModel)result.Value;

            errorDetailModel.Code.Should().Be(StatusCodes.Status400BadRequest);
            errorDetailModel.Type.Should().Be(ErrorTypeConstants.Business);
            errorDetailModel.Detail.Should().Be("Test message");
            errorDetailModel.Errors.Count.Should().Be(1);

            var error = errorDetailModel.Errors.First();

            error.Detail.Should().Be("");
            error.Title.Should().Be("Test message");
            error.Code.Should().Be(StatusCodes.Status400BadRequest.ToString());
        }
    }
}
