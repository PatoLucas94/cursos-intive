using System;
using Spv.Usuarios.Domain.Entities;

namespace Spv.Usuarios.Test.Infrastructure.Builders
{
    public class UsuarioRegistradoInitializerBuilder
    {
        protected UsuarioRegistradoInitializerBuilder()
        {
        }

        private static UsuarioRegistrado WithDefaultValues()
        {
            return new UsuarioRegistrado
            {
                DocumentTypeId = 80,
            };
        }

        public static UsuarioRegistrado GetUsuarioRegistradoWithDefaultValues(
            string documentNumber,
            bool active,
            string cardNumber,
            DateTime expiredDateTime,
            int forgotPasswordAttempts,
            string channelKey)
        {
            var usuarioRegistrado = WithDefaultValues();
            usuarioRegistrado.DocumentNumber = documentNumber;
            usuarioRegistrado.Active = active;
            usuarioRegistrado.CardNumber = cardNumber;
            usuarioRegistrado.ExpiredDateTime = expiredDateTime;
            usuarioRegistrado.ForgotPasswordAttempts = forgotPasswordAttempts;
            usuarioRegistrado.ChannelKey = channelKey;

            return usuarioRegistrado;
        }

        public static UsuarioRegistrado GetUsuarioRegistradoWithoutDefaultValues(
            string documentNumber,
            bool active,
            string cardNumber,
            DateTime expiredDateTime,
            int forgotPasswordAttempts,
            string channelKey)
        {
            var usuarioRegistrado = new UsuarioRegistrado
            {
                DocumentNumber = documentNumber,
                Active = active,
                CardNumber = cardNumber,
                ExpiredDateTime = expiredDateTime,
                ForgotPasswordAttempts = forgotPasswordAttempts,
                ChannelKey = channelKey
            };

            return usuarioRegistrado;
        }
    }
}
