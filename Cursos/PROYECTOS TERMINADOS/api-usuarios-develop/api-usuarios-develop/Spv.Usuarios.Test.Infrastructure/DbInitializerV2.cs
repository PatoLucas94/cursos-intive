using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.DataAccess.EntityFramework;
using Spv.Usuarios.Domain.Entities;
using Spv.Usuarios.Domain.Entities.V2;
using Spv.Usuarios.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Spv.Usuarios.Test.Infrastructure
{
    public static class DbInitializerV2
    {
        private const string GenericPassword = "+amMWpgHpovNljw1rAVsGOu/tq5taso93h9ehVobfV8";

        private static readonly List<UsuarioV2> Usuarios = new List<UsuarioV2>
        {
            new UsuarioV2
            {
                UserId = 1,
                PersonId = 14155917,
                DocumentTypeId = 4,
                DocumentNumber = "12345678",
                CreatedDate = DateTime.Today.AddDays(-10),
                LastLogon = DateTime.Today,
                LastPasswordChange = DateTime.Today,
                LoginAttempts = 0,
                Password = GenericPassword,
                Username = "WWu/LIQRXGNQdYK/KKdUNaoAkT/oedNBtbNo980lfTI",
                UserStatusId = (int)UserStatus.Active,
                DocumentCountryId = 80
            },
            new UsuarioV2
            {
                UserId = 2,
                PersonId = 10002,
                DocumentTypeId = 4,
                DocumentNumber = "21234567",
                CreatedDate = DateTime.Today.AddDays(-2),
                LastLogon = DateTime.Today.AddDays(-1),
                LastPasswordChange = DateTime.Today,
                LoginAttempts = 0,
                Password = GenericPassword,
                Username = "i1rSx4gaBjStGMXwM4l5zT4Ue/yFvEqNtlPKYIxh66k",
                UserStatusId = (int)UserStatus.Inactive,
                DocumentCountryId = 80
            },
            new UsuarioV2
            {
                UserId = 3,
                PersonId = null,
                DocumentTypeId = 1,
                DocumentNumber = "31234567",
                CreatedDate = DateTime.MinValue,
                LastLogon = DateTime.MinValue,
                LastPasswordChange = DateTime.MinValue,
                LoginAttempts = 2,
                Password = GenericPassword,
                Username = "UtoMNHsGgQEVOVPBLabeaQY7pu2FJbkx7pNzOSVrz0o",
                UserStatusId = (int)UserStatus.Active,
                DocumentCountryId = 80
            },
            new UsuarioV2
            {
                UserId = 4,
                PersonId = 4,
                DocumentTypeId = 1,
                DocumentNumber = "41234567",
                CreatedDate = DateTime.MinValue,
                LastLogon = DateTime.MinValue,
                LastPasswordChange = DateTime.Today.AddDays(-181),
                LoginAttempts = 0,
                Password = GenericPassword,
                Username = "HjPcbGvVpHDBcreYTCCiqMck9WiCcYjP5hC+CN8jXMY",
                UserStatusId = (int)UserStatus.Active,
                DocumentCountryId = 80
            },
            new UsuarioV2
            {
                UserId = 5,
                PersonId = 5,
                DocumentTypeId = 1,
                DocumentNumber = "51234567",
                CreatedDate = DateTime.MinValue,
                LastLogon = DateTime.MinValue,
                LastPasswordChange = DateTime.Today.AddDays(-181),
                LoginAttempts = 0,
                Password = GenericPassword,
                Username = "ivK5IfSp4ZR18lxOCRp77oGlEVByKjcRbAXZ609IvEY",
                UserStatusId = (int)UserStatus.Blocked,
                DocumentCountryId = 80
            },
            new UsuarioV2
            {
                UserId = 6,
                PersonId = 6,
                DocumentTypeId = 1,
                DocumentNumber = "61234567",
                CreatedDate = DateTime.MinValue,
                LastLogon = DateTime.MinValue,
                LastPasswordChange = DateTime.MinValue,
                LoginAttempts = 0,
                Password = GenericPassword,
                Username = "9Nzx6HQc6dZe19ctn0UokqNPQunC5on6TKdl72wW+Ys",
                UserStatusId = (int)UserStatus.Active
            },
            new UsuarioV2
            {
                UserId = 7,
                PersonId = 7,
                DocumentTypeId = 1,
                DocumentNumber = "71234567",
                CreatedDate = DateTime.MinValue,
                LastLogon = DateTime.MinValue,
                LastPasswordChange = DateTime.MinValue,
                LoginAttempts = 0,
                Password = GenericPassword,
                Username = "2p2yUkuajp89tg1b5D5zygP4AjWtgmVG5isvJlLHwUM",
                UserStatusId = (int)UserStatus.Blocked
            },
            new UsuarioV2
            {
                UserId = 8,
                PersonId = 8,
                DocumentTypeId = 1,
                DocumentNumber = "91827346",
                CreatedDate = DateTime.MinValue,
                LastLogon = DateTime.MinValue,
                LastPasswordChange = DateTime.MinValue,
                LoginAttempts = 0,
                Password = GenericPassword,
                Username = "auST33mlXOlFktR4YNkgFJV/TMQ9DfqiDtF7twNhg6U",
                UserStatusId = (int)UserStatus.Blocked,
                DocumentCountryId = 80
            },
            new UsuarioV2
            {
                UserId = 9,
                PersonId = 63528375,
                DocumentTypeId = 1,
                DocumentNumber = "33701133",
                CreatedDate = DateTime.MinValue,
                LastLogon = DateTime.MinValue,
                LastPasswordChange = DateTime.MinValue,
                LoginAttempts = 0,
                Password = "+amMWpgHpovNljw1rAVsGOu/tq5taso93h9ehVobfV8",
                Username = "uZFY6DG+JqTZWHQzaUozviEia1wE2vUEYlICQVujlUI",
                UserStatusId = (int)UserStatus.Active,
                DocumentCountryId = 80
            },
            new UsuarioV2
            {
                UserId = 10,
                PersonId = 10,
                DocumentTypeId = 1,
                DocumentNumber = "48129347",
                CreatedDate = DateTime.MinValue,
                LastLogon = DateTime.MinValue,
                LastPasswordChange = DateTime.MinValue,
                LoginAttempts = 0,
                Password = "+amMWpgHpovNljw1rAVsGOu/tq5taso93h9ehVobfV8",
                Username = "v+PMWDoWC0ce5v3Q7i0wQHtD2wGgqQg8vkZ0kzCtlAg",
                UserStatusId = (int)UserStatus.Blocked,
                DocumentCountryId = 80
            },
            new UsuarioV2
            {
                UserId = 11,
                PersonId = 12345678,
                DocumentTypeId = 1,
                DocumentNumber = "48129348",
                CreatedDate = DateTime.MinValue,
                LastLogon = DateTime.MinValue,
                LastPasswordChange = DateTime.MinValue,
                LoginAttempts = 0,
                Password = "+amMWpgHpovNljw1rAVsGOu/tq5taso93h9ehVobfV8",
                Username = "6Q3a9ZZyuJ7dYg/iuYHr6dajwdyx+GxrDqWGVTW6nW8", // Rcbertoldo
                UserStatusId = (int)UserStatus.Blocked,
                DocumentCountryId = 80
            },
            new UsuarioV2
            {
                UserId = 12,
                PersonId = 23456781,
                DocumentTypeId = 1,
                DocumentNumber = "48129349",
                CreatedDate = DateTime.MinValue,
                LastLogon = DateTime.MinValue,
                LastPasswordChange = DateTime.MinValue,
                LoginAttempts = 0,
                Password = "G09K8GIrAGOOxUyctclssdZHWndMABdlPOYiFlBWUjE", // 4286
                Username = "zrTrQZ2uugfKtFxyBdFmKGRDx1JQHpPdu2BCtJ+RejQ", // SuspendedUser01
                UserStatusId = (int)UserStatus.Suspended,
                DocumentCountryId = 80
            }
        };

        private static readonly List<ConfiguracionV2> Configuraciones = new List<ConfiguracionV2>
        {
            new ConfiguracionV2
            {
                ConfigurationId = 1,
                Type = AppConstants.ConfigurationTypeUsers,
                Name = AppConstants.DiasParaForzarCambioDeClaveKey,
                Value = "180"
            },
            new ConfiguracionV2
            {
                ConfigurationId = 2,
                Description = "Cantidad de intentos de login en front.",
                IsSecurity = false,
                Name = AppConstants.CantidadDeIntentosDeLoginKey,
                Rol = AppConstants.ConfigurationRolConfiguration,
                Type = AppConstants.ConfigurationTypeUsers,
                Value = "5"
            },
            new ConfiguracionV2
            {
                ConfigurationId = 3,
                Description = "Cantidad de contraseñas guardadas en el histórico.",
                IsSecurity = false,
                Name = AppConstants.CantidadDeHistorialDeCambiosDeClave,
                Rol = AppConstants.ConfigurationRolConfiguration,
                Type = AppConstants.ConfigurationTypeUsers,
                Value = "5"
            },
            new ConfiguracionV2
            {
                ConfigurationId = 4,
                Type = AppConstants.ConfigurationTypeChannelsKey,
                Name = AppConstants.CantidadDeIntentosDeClaveDeCanalesKey,
                Value = "3"
            },
            new ConfiguracionV2
            {
                ConfigurationId = 5,
                Description = "Cantidad de nombres de usuario guardados en el histórico.",
                IsSecurity = false,
                Name = AppConstants.CantidadDeHistorialDeCambiosDeNombreUsuario,
                Rol = AppConstants.ConfigurationRolConfiguration,
                Type = AppConstants.ConfigurationTypeUsers,
                Value = "2"
            }
        };

        private static readonly List<EstadosUsuarioV2> EstadosUsuario = new List<EstadosUsuarioV2>
        {
            new EstadosUsuarioV2
            {
                UserStatusId = 1,
                Description = "State 1"
            },
            new EstadosUsuarioV2
            {
                UserStatusId = 2,
                Description = "State 2"
            },
            new EstadosUsuarioV2
            {
                UserStatusId = 3,
                Description = "State 3"
            },
            new EstadosUsuarioV2
            {
                UserStatusId = 4,
                Description = "State 4"
            },
            new EstadosUsuarioV2
            {
                UserStatusId = 5,
                Description = "State 5"
            },
            new EstadosUsuarioV2
            {
                UserStatusId = 6,
                Description = "State 6"
            },
            new EstadosUsuarioV2
            {
                UserStatusId = 7,
                Description = "State 7"
            }
        };

        private static readonly List<AuditoriaLogV2> AuditoriaLogs = new List<AuditoriaLogV2>
        {
            new AuditoriaLogV2
            {
                AuditLogId = 1,
                EventTypeId = 1,
                EventResultId = 1,
                DateTime = DateTime.MinValue,
                ExtendedInfo = "Info 1",
                Channel = "Channel 1",
                UserId = 1
            },
            new AuditoriaLogV2
            {
                AuditLogId = 2,
                EventTypeId = 2,
                EventResultId = 1,
                DateTime = DateTime.MinValue,
                ExtendedInfo = "Info 2",
                Channel = "Channel 2",
                UserId = 2
            }
        };

        private static readonly List<TiposEventoV2> TiposEvento = new List<TiposEventoV2>
        {
            new TiposEventoV2
            {
                EventTypeId = 1,
                Name = "Autenticación",
                Description = "Description 1"
            }
        };

        private static readonly List<ResultadosEventoV2> ResultadosEvento = new List<ResultadosEventoV2>
        {
            new ResultadosEventoV2
            {
                EventResultId = 1,
                Description = "Description 1"
            }
        };

        private static readonly List<HistorialClaveUsuariosV2> HistorialClaveUsuarios = new List<HistorialClaveUsuariosV2>
        {
            new HistorialClaveUsuariosV2
            {
                PasswordHistoryId = 1,
                AuditLogId = 1,
                UserId = 1,
                Password = "Password",
                CreationDate = DateTime.MinValue
            },
            new HistorialClaveUsuariosV2
            {
                PasswordHistoryId = 2,
                AuditLogId = 2,
                UserId = 6,
                Password = "Password",
                CreationDate = DateTime.MinValue
            },
            new HistorialClaveUsuariosV2
            {
                PasswordHistoryId = 3,
                AuditLogId = 3,
                UserId = 11,
                Password = "eJKZogM78WGu7gtO1A9Uju6+5eufMnSwC2xpYhK+/GE",
                CreationDate = DateTime.MinValue
            }
        };

        private static readonly List<HistorialUsuarioUsuariosV2> HistorialUsuarioUsuarios = new List<HistorialUsuarioUsuariosV2>
        {
            new HistorialUsuarioUsuariosV2
            {
                UsernameHistoryId = 1,
                AuditLogId = 1,
                UserId = 1,
                Username = "Username",
                CreationDate = DateTime.MinValue
            },
            new HistorialUsuarioUsuariosV2
            {
                UsernameHistoryId = 2,
                AuditLogId = 2,
                UserId = 6,
                Username = "Username",
                CreationDate = DateTime.MinValue
            },
            new HistorialUsuarioUsuariosV2
            {
                UsernameHistoryId = 3,
                AuditLogId = 3,
                UserId = 11,
                Username = "T/no86Hr63T6775Gdbnuv7gLQZcOoH/a+d85atMX82Y", // rcbertoldo
                CreationDate = DateTime.MinValue
            },
            new HistorialUsuarioUsuariosV2
            {
                UsernameHistoryId = 4,
                AuditLogId = 4,
                UserId = 11,
                Username = "IHupyfMNwxw89Runq13QKcZkWY3M6wzJCn4LnuQrCVY", // rcbertoldo01
                CreationDate = DateTime.MinValue.AddDays(1)
            }
        };

        private static readonly List<ReglaValidacionV2> ReglasValidacion = new List<ReglaValidacionV2>
        {
            new ReglaValidacionV2
            {
                ValidationRuleId = 1,
                ValidationRuleName = "LetrasYNumeros",
                ValidationRuleText = "Letras y números.",
                IsActive = true,
                ActivationDate = DateTime.Today,
                InactivationDate = null,
                IsRequired = true,
                RegularExpression = "/^(?=.*[0-9])(?=.*[A-Za-z])([A-Za-z0-9]+)$/",
                ModelId = 3,
                InputId = 1,
                CreatedDate = DateTime.Today.AddDays(-10),
                ValidationRulePriority = 5
            },
            new ReglaValidacionV2
            {
                ValidationRuleId = 2,
                ValidationRuleName = "Entre8y15Caracteres",
                ValidationRuleText = "Entre 8 y 15 caracteres.",
                IsActive = true,
                ActivationDate = DateTime.Today.AddDays(-10),
                InactivationDate = DateTime.Today,
                IsRequired = true,
                RegularExpression = "/^[A-Za-z0-9]{8,15}$/i",
                ModelId = 3,
                InputId = 1,
                CreatedDate = DateTime.Today.AddDays(-20),
                ValidationRulePriority = 4
            },
            new ReglaValidacionV2
            {
                ValidationRuleId = 3,
                ValidationRuleName = "NoMasDe3CaracteresRepetidos",
                ValidationRuleText = "No más de 3 caracteres repetidos.",
             IsActive = true,
                ActivationDate = DateTime.Today.AddDays(-10),
                InactivationDate = DateTime.Today,
                IsRequired = true,
                RegularExpression = @"/^(?!.*?([A-Za-z0-9])\1\1\1).+$/i",
                ModelId = 3,
                InputId = 1,
                CreatedDate = DateTime.Today.AddDays(-20),
                ValidationRulePriority = 3
            },
            new ReglaValidacionV2
            {
                ValidationRuleId = 4,
                ValidationRuleName = "NoMasDe3CaracteresConsecutivosAscODesc",
                ValidationRuleText = "No más de 3 caracteres consecutivos.",
                IsActive = true,
                ActivationDate = DateTime.Today.AddDays(-10),
                InactivationDate = DateTime.Today,
                IsRequired = true,
                RegularExpression = "/^(?!.*(?:0123|1234|2345|3456|4567|5678|6789|9876|8765|7654|6543|5432|4321|3210|abcd|bcde|cdef|defg|efgh|fghi|ghij|hijk|ijkl|jklm|klmn|lmno|mnop|nopq|opqr|pqrs|qrst|rstu|stuv|tuvw|uvwx|vwxy|wxyz|zyxw|yxwv|xwvu|wvut|vuts|utsr|tsru|srqp|rqpo|qpon|ponm|onml|nmlk|mlkj|lkji|kjih|jihg|ihgf|hgfe|gfed|fedc|edcb|dcba)).+$/i",
                ModelId = 3,
                InputId = 1,
                CreatedDate = DateTime.Today.AddDays(-20),
                ValidationRulePriority = 2
            },
            new ReglaValidacionV2
            {
                ValidationRuleId = 5,
                ValidationRuleName = "AlMenos1LetraMayuscula",
                ValidationRuleText = "Recomendamos tener al menos una letra mayúscula.",
                IsActive = true,
                ActivationDate = DateTime.Today.AddDays(-10),
                InactivationDate = DateTime.Today,
                IsRequired = true,
                RegularExpression = "/^(?=.*[A-Z])([A-Za-z0-9]+)$/",
                ModelId = 3,
                InputId = 1,
                CreatedDate = DateTime.Today.AddDays(-20),
                ValidationRulePriority = 1
            },
                        new ReglaValidacionV2
            {
                ValidationRuleId = 6,
                ValidationRuleName = "SoloNumeros",
                ValidationRuleText = "Solo números.",
                IsActive = true,
                ActivationDate = DateTime.Today,
                InactivationDate = null,
                IsRequired = true,
                RegularExpression = "/^(?=.*[0-9])([0-9]+)$/",
                ModelId = 3,
                InputId = 2,
                CreatedDate = DateTime.Today.AddDays(-10),
                ValidationRulePriority = 4
            },
            new ReglaValidacionV2
            {
                ValidationRuleId = 7,
                ValidationRuleName = "Solo4Caracteres",
                ValidationRuleText = "4 caracteres.",
                 IsActive = true,
                ActivationDate = DateTime.Today.AddDays(-10),
                InactivationDate = DateTime.Today,
                IsRequired = true,
                RegularExpression = "/^[A-Za-z0-9]{4}$/i",
                ModelId = 3,
                InputId = 2,
                CreatedDate = DateTime.Today.AddDays(-20),
                ValidationRulePriority = 3
            },
            new ReglaValidacionV2
            {
                ValidationRuleId = 8,
                ValidationRuleName = "NoMasDe3NumerosRepetidos",
                ValidationRuleText = "No más de 3 números repetidos.",
                 IsActive = true,
                ActivationDate = DateTime.Today.AddDays(-10),
                InactivationDate = DateTime.Today,
                IsRequired = true,
                RegularExpression = @"/^(?!.*?([0-9])\1\1\1).+$/",
                ModelId = 3,
                InputId = 2,
                CreatedDate = DateTime.Today.AddDays(-20),
                ValidationRulePriority = 2
            },
            new ReglaValidacionV2
            {
                ValidationRuleId = 9,
                ValidationRuleName = "NoMasDe3NumerosConsecutivosAscODesc",
                ValidationRuleText = "No más de 3 números consecutivos.",
                IsActive = false,
                ActivationDate = DateTime.Today.AddDays(-10),
                InactivationDate = DateTime.Today,
                IsRequired = true,
                RegularExpression = @"/^(?!.*(?:0123|1234|2345|3456|4567|5678|6789|9876|8765|7654|6543|5432|4321|3210)).+$/",
                ModelId = 3,
                InputId = 2,
                CreatedDate = DateTime.Today.AddDays(-20),
                ValidationRulePriority = 1
            },
                        new ReglaValidacionV2
            {
                ValidationRuleId = 10,
                ValidationRuleName = "Entre8y14Caracteres",
                ValidationRuleText = "Debe tener entre 8 y 14 caracteres.",
                IsActive = true,
                ActivationDate = DateTime.Today,
                InactivationDate = null,
                IsRequired = true,
                RegularExpression = "/^.{8,14}$/",
                ModelId = 1,
                InputId = 2,
                CreatedDate = DateTime.Today.AddDays(-10),
                ValidationRulePriority = 4
            },
            new ReglaValidacionV2
            {
                ValidationRuleId = 11,
                ValidationRuleName = "AlMenos1LetraMayusculaEspecial",
                ValidationRuleText = "Debe contener al menos una letra mayúscula.",
                 IsActive = true,
                ActivationDate = DateTime.Today.AddDays(-10),
                InactivationDate = DateTime.Today,
                IsRequired = true,
                RegularExpression = "/^(?=.*[A-Z]).*$/",
                ModelId = 1,
                InputId = 2,
                CreatedDate = DateTime.Today.AddDays(-20),
                ValidationRulePriority = 3
            },
            new ReglaValidacionV2
            {
                ValidationRuleId = 12,
                ValidationRuleName = "AlMenos2Numeros",
                ValidationRuleText = "Debe contener al menos dos números.",
                 IsActive = true,
                ActivationDate = DateTime.Today.AddDays(-10),
                InactivationDate = DateTime.Today,
                IsRequired = true,
                RegularExpression = @"/^(.*(\d+.*\d+).*)$/",
                ModelId = 1,
                InputId = 2,
                CreatedDate = DateTime.Today.AddDays(-20),
                ValidationRulePriority = 2
            },
            new ReglaValidacionV2
            {
                ValidationRuleId = 13,
                ValidationRuleName = "LetrasYNumerosYCaracteresEspecialesLimitados",
                ValidationRuleText = "Opcionalmente puede utilizar: punto, guión medio o guión bajo.",
                IsActive = false,
                ActivationDate = DateTime.Today.AddDays(-10),
                InactivationDate = DateTime.Today,
                IsRequired = true,
                RegularExpression = @"/^(?=.*[A-Za-z0-9\-\.\_]+)([A-Za-z0-9\.\-\_]+)$/",
                ModelId = 1,
                InputId = 2,
                CreatedDate = DateTime.Today.AddDays(-20),
                ValidationRulePriority = 1
            },
            new ReglaValidacionV2
            {
                ValidationRuleId = 14,
                ValidationRuleName = "Entre8y15Caracteres",
                ValidationRuleText = "Entre 8 y 15 caracteres.",
                IsActive = true,
                ActivationDate = DateTime.Today.AddDays(-10),
                InactivationDate = DateTime.Today,
                IsRequired = true,
                RegularExpression = "/^[A-Za-z0-9]{8,15}$/i",
                ModelId = 1,
                InputId = 1,
                CreatedDate = DateTime.Today.AddDays(-20),
                ValidationRulePriority = 4
            },
            new ReglaValidacionV2
            {
                ValidationRuleId = 15,
                ValidationRuleName = "LetrasYNumeros",
                ValidationRuleText = "Letras y números.",
                IsActive = true,
                ActivationDate = DateTime.Today,
                InactivationDate = null,
                IsRequired = true,
                RegularExpression = "/^(?=.*[0-9])(?=.*[A-Za-z])([A-Za-z0-9]+)$/",
                ModelId = 1,
                InputId = 1,
                CreatedDate = DateTime.Today.AddDays(-10),
                ValidationRulePriority = 5
            }
        };

        private static readonly List<DynamicImages> DynamicImages = new List<DynamicImages>
        {
            new DynamicImages
            {
                Id = 1,
                Nombre = "imagen1"
            },
            new DynamicImages
            {
                 Id = 2,
                Nombre = "imagen2"
            }
        };

        private static readonly List<DynamicImagesLogin> DynamicImagesLogin = new List<DynamicImagesLogin>
        {
            new DynamicImagesLogin
            {
                Id = 1,
                Nombre = "imagen1",
                IdImagen = 1,
                Link = "",
                Orden = 1,
                Habilitada = true
            },
            new DynamicImagesLogin
            {
                Id = 2,
                Nombre = "imagen2",
                IdImagen = 2,
                Link = "",
                Orden = 2,
                Habilitada = true
            }
        };

        public static void Initialize(GenericDbContextV2 context)
        {
            context.Database.EnsureCreated();

            context.Usuario.AddRange(Usuarios);
            context.Auditoria.AddRange(AuditoriaLogs);
            context.Configuracion.AddRange(Configuraciones);
            context.EstadosUsuario.AddRange(EstadosUsuario);
            context.TiposEvento.AddRange(TiposEvento);
            context.ResultadosEvento.AddRange(ResultadosEvento);
            context.HistorialClaveUsuarios.AddRange(HistorialClaveUsuarios);
            context.HistorialUsuarioUsuarios.AddRange(HistorialUsuarioUsuarios);
            context.ReglaValidacion.AddRange(ReglasValidacion);
            context.DynamicImages.AddRange(DynamicImages);
            context.DynamicImagesLogin.AddRange(DynamicImagesLogin);

            context.SaveChanges();
        }
    }
}
