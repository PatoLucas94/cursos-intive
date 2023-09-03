using System;
using System.Collections.Generic;
using System.Linq;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.DataAccess.EntityFramework;
using Spv.Usuarios.Domain.Entities;
using Spv.Usuarios.Domain.Enums;
using Spv.Usuarios.Test.Infrastructure.Builders;

namespace Spv.Usuarios.Test.Infrastructure
{
    /// <summary>
    /// DbInitializer
    /// </summary>
    public static class DbInitializer
    {
        private static int TotalUsuarios { get; set; }
        private static List<int> IdsUsuarios { get; } = new List<int>();

        private static readonly List<Usuario> Usuarios = new List<Usuario>
        {
            new Usuario
            {
                UserId = 1,
                UserName = "UsuarioTest1",
                Password = "+amMWpgHpovNljw1rAVsGOu/tq5taso93h9ehVobfV8", // Info1212
                DocumentNumber = "11222333",
                UserStatusId = (byte)UserStatus.Active,
                Name = "Usuario",
                LastName = "Test1",
                CustomerNumber = "0800435125487",
                LastLogon = DateTime.MinValue,
                DocumentCountryId = "080",
                DocumentTypeId = 4
            },
            new Usuario
            {
                UserId = 2,
                UserName = "UsuarioTest2",
                Password = "+amMWpgHpovNljw1rAVsGOu/tq5taso93h9ehVobfV8", // Info1212
                DocumentNumber = "11222333",
                UserStatusId = (byte)UserStatus.Blocked,
                Name = "Usuario",
                LastName = "Test2",
                CustomerNumber = "0800435125487",
                LastLogon = DateTime.MinValue
            },
            new Usuario
            {
                UserId = 3,
                UserName = "UsuarioTest3",
                Password = "+amMWpgHpovNljw1rAVsGOu/tq5taso93h9ehVobfV8", // Info1212
                DocumentNumber = "11222333",
                UserStatusId = (byte)UserStatus.Inactive,
                Name = "Usuario",
                LastName = "Test3",
                CustomerNumber = "0800435125487",
                LastLogon = DateTime.MinValue
            },
            new Usuario
            {
                UserId = 4,
                UserName = "UsuarioTest4",
                Password = "+amMWpgHpovNljw1rAVsGOu/tq5taso93h9ehVobfV8", // Info1212
                DocumentNumber = "11222333",
                UserStatusId = (byte)UserStatus.Active,
                LastPasswordChange = DateTime.Today.AddDays(-181),
                Name = "Usuario",
                LastName = "Test3",
                CustomerNumber = "0800435125487",
                LastLogon = DateTime.MinValue
            },
            new Usuario
            {
                UserId = 5,
                CustomerNumber = "0800412345678",
                SessionId = "Numero_de_session",
                UserName = "UsuarioTest5",
                Password = "+amMWpgHpovNljw1rAVsGOu/tq5taso93h9ehVobfV8", // Info1212
                UserStatusId = (byte)UserStatus.Active,
                Name = "Nombre_usuario",
                LastName = "Apellido_usuario",
                LastPasswordChange = DateTime.Today.AddDays(-181),
                CreatedDate = DateTime.Today.AddDays(-50),
                DocumentCountryId = "080",
                DocumentTypeId = 4,
                DocumentNumber = "12345678",
                LastLogon = DateTime.Today.AddDays(-50),
                LoginAttempts = 0,
                LoginAttemptsDate = DateTime.Today.AddDays(-50),
                MobileEnabled = true,
                ReceiptExtract = true,
                ReceiptExtractDate = DateTime.Today.AddDays(-50),
                IsEmployee = false,
                CUIL = "20123456785",
                IsResident = true,
                FullControl = true,
                Culture = "",
                SecurityQuestion = "",
                SecurityAnswer = "",
                ChannelSource = "MOB",
                BenefitClubId = 1,
                MarcaUDF = true,
                MarcaUDFDate = DateTime.Today.AddDays(-50)
            },
            new Usuario
            {
                UserId = 6,
                UserName = "UsuarioTest6",
                Password = "+amMWpgHpovNljw1rAVsGOu/tq5taso93h9ehVobfV8", // Info1212
                DocumentNumber = "11222333",
                UserStatusId = (byte)UserStatus.Active,
                LastPasswordChange = DateTime.Today.AddDays(-181),
                Name = "Usuario",
                LastName = "Test6",
                CustomerNumber = "0800435125487",
                LastLogon = DateTime.MinValue
            },
            new Usuario
            {
                UserId = 7,
                UserName = "UsuarioTest7",
                Password = "+amMWpgHpovNljw1rAVsGOu/tq5taso93h9ehVobfV8", // Info1212
                DocumentNumber = "11222333",
                UserStatusId = (byte)UserStatus.Pending,
                LastPasswordChange = DateTime.Today.AddDays(-181),
                Name = "Usuario",
                LastName = "Test7",
                CustomerNumber = "0800435125487",
                LastLogon = DateTime.MinValue
            },
            new Usuario
            {
                UserId = 8,
                UserName = "UsuarioTest8",
                Password = "+amMWpgHpovNljw1rAVsGOu/tq5taso93h9ehVobfV8", // Info1212
                DocumentCountryId = "080",
                DocumentTypeId = 4,
                DocumentNumber = "11222333",
                UserStatusId = (byte)UserStatus.Active,
                LoginAttempts = 2,
                Name = "Usuario",
                LastName = "Test8",
                CustomerNumber = "0800435125487",
                LastLogon = DateTime.MinValue
            },
            new Usuario
            {
                UserId = 9,
                UserName = "User Test Migrated",
                Password = "+amMWpgHpovNljw1rAVsGOu/tq5taso93h9ehVobfV8", // Info1212
                DocumentNumber = "91827346",
                DocumentTypeId = 1,
                UserStatusId = (byte)UserStatus.Active,
                LoginAttempts = 0,
                Name = "Usuario",
                LastName = "Migrated",
                CustomerNumber = "0800435125487",
                LastLogon = DateTime.MinValue,
                DocumentCountryId = "080"
            },
            new Usuario
            {
                UserId = 10,
                UserName = "User Test 10",
                Password = "+amMWpgHpovNljw1rAVsGOu/tq5taso93h9ehVobfV8", // Info1212
                DocumentCountryId = "80",
                DocumentTypeId = 4,
                DocumentNumber = "24789456",
                UserStatusId = (byte)UserStatus.Active,
                LoginAttempts = 2,
                Name = "Usuario",
                LastName = "Test 10",
                CustomerNumber = "0800424789456",
                LastLogon = DateTime.MinValue
            },
            new Usuario
            {
                UserId = 11,
                UserName = "User Test 11",
                Password = "+amMWpgHpovNljw1rAVsGOu/tq5taso93h9ehVobfV8", // Info1212
                DocumentCountryId = "80",
                DocumentTypeId = 4,
                DocumentNumber = "73648274",
                UserStatusId = (byte)UserStatus.Active,
                LoginAttempts = 2,
                Name = "Usuario",
                LastName = "Test 11",
                CustomerNumber = "0800424789456",
                LastLogon = DateTime.MinValue
            },
            new Usuario
            {
                UserId = 12,
                UserName = "UserTest12",
                Password = "+amMWpgHpovNljw1rAVsGOu/tq5taso93h9ehVobfV8", // Info1212
                DocumentCountryId = "80",
                DocumentTypeId = 4,
                DocumentNumber = "10121212",
                LastLogon = DateTime.MinValue,
                CreatedDate = DateTime.MinValue
            },
            new Usuario
            {
                UserId = 13,
                UserName = "UsuarioTest13",
                Password = "+amMWpgHpovNljw1rAVsGOu/tq5taso93h9ehVobfV8", // Info1212
                DocumentNumber = "13131313",
                UserStatusId = (byte)UserStatus.Active,
                Name = "Usuario",
                LastName = "Test13",
                CustomerNumber = "0800413131313",
                LastLogon = DateTime.MinValue,
                DocumentCountryId = "080",
                DocumentTypeId = 4
            },
            new Usuario
            {
                UserId = 14,
                UserName = "UsuarioTest14",
                Password = "+amMWpgHpovNljw1rAVsGOu/tq5taso93h9ehVobfV8", // Info1212
                DocumentNumber = "12345678",
                UserStatusId = (byte)UserStatus.Active,
                Name = "Usuario",
                LastName = "Test14",
                CustomerNumber = "0800413131313",
                LastLogon = DateTime.MinValue,
                DocumentCountryId = "080",
                DocumentTypeId = 4
            }
        };

        private static readonly List<DatosUsuario> DatosUsuarios = new List<DatosUsuario>
        {
            DatosUsuarioInitializerBuilder.GetDatosUsuarioWithDefaultValues(1, 1, "852", "test1@test.com"),
            DatosUsuarioInitializerBuilder.GetDatosUsuarioWithDefaultValues(2, 2, "10002", "test2@test.com"),
            DatosUsuarioInitializerBuilder.GetDatosUsuarioWithDefaultValues(3, 3, null, "test3@test.com"),
            DatosUsuarioInitializerBuilder.GetDatosUsuarioWithDefaultValues(4, 4, "10004", "test4@test.com"),
            DatosUsuarioInitializerBuilder.GetDatosUsuarioWithDefaultValues(5, 5, "10005", "test5@test.com"),
            DatosUsuarioInitializerBuilder.GetDatosUsuarioWithDefaultValues(8, 8, "10008", "test8@test.com"),
            DatosUsuarioInitializerBuilder.GetDatosUsuarioWithDefaultValues(9, 9, "12345678", "test8@test.com"),
            DatosUsuarioInitializerBuilder.GetDatosUsuarioWithDefaultValues(10, 10, null, "test10@test.com"),
            DatosUsuarioInitializerBuilder.GetDatosUsuarioWithDefaultValues(11, 11, null, "test11@test.com"),
            DatosUsuarioInitializerBuilder.GetDatosUsuarioWithDefaultValues(12, 12, "12121212", "test12@test.com"),
            DatosUsuarioInitializerBuilder.GetDatosUsuarioWithDefaultValues(13, 13, "13131313", "test13@test.com"),
            DatosUsuarioInitializerBuilder.GetDatosUsuarioWithDefaultValues(14, 14, "14155917", "holamundo@gmail.com"),
        };

        private static readonly List<Configuracion> Configuraciones = new List<Configuracion>
        {
            new Configuracion
            {
                ConfigurationId = 1,
                Type = AppConstants.ConfigurationTypeUsers,
                Name = AppConstants.DiasParaForzarCambioDeClaveKey,
                Value = "180"
            },
            new Configuracion
            {
                ConfigurationId = 2,
                Type = AppConstants.ConfigurationTypeUsers,
                Name = AppConstants.CantidadDeIntentosDeLoginKey,
                Value = "3"
            },
            new Configuracion
            {
                ConfigurationId = 3,
                Type = AppConstants.ConfigurationTypeChannelsKey,
                Name = AppConstants.CantidadDeIntentosDeClaveDeCanalesKey,
                Value = "3"
            },
            new Configuracion
            {
                ConfigurationId = 4,
                Name = AppConstants.CantidadDeHistorialDeCambiosDeClave,
                Type = AppConstants.ConfigurationTypeUsers,
                Value = "3"
            },
            new Configuracion
            {
                ConfigurationId = 5,
                Name = AppConstants.RegistracionNuevoModeloHabilitado,
                Type = AppConstants.ConfigurationTypeDigitalCredentials,
                Value = "true"
            },
            new Configuracion
            {
                ConfigurationId = 6,
                Name = AppConstants.LogInDefaultDisabledMessage,
                Type = AppConstants.ConfigurationTypeDigitalCredentials,
                Value = "true"
            },
            new Configuracion
            {
                ConfigurationId = 7,
                Name = AppConstants.LogInDisabledMessage,
                Type = AppConstants.ConfigurationTypeDigitalCredentials,
                Value = "true"
            }
            ,
            new Configuracion
            {
                ConfigurationId = 8,
                Name = AppConstants.LogInDisabled,
                Type = AppConstants.ConfigurationTypeDigitalCredentials,
                Value = "true"
            }
            ,
            new Configuracion
            {
                ConfigurationId = 9,
                Name = AppConstants.TerminosYCondicionesHabilitado,
                Type = AppConstants.ConfigurationTypeDigitalCredentials,
                Value = "true"
            }
        };

        private static readonly List<AuditoriaLog> Auditorias = new List<AuditoriaLog>
        {
            new AuditoriaLog
            {
                ActionId = (int)AuditAction.LogOn,
                ActionResultId = (int)AuditActionResult.LoggedOn,
                DateTime = DateTime.MinValue,
                UserId = 5
            }
        };

        private static readonly List<UsuarioRegistrado> ClaveCanales = new List<UsuarioRegistrado>
        {
            UsuarioRegistradoInitializerBuilder.GetUsuarioRegistradoWithDefaultValues("12345", false, "ClaveInactiva", DateTime.MinValue, 0, "ChannelKey"),
            UsuarioRegistradoInitializerBuilder.GetUsuarioRegistradoWithDefaultValues("6789", true, "0193845290767024384", DateTime.MinValue, 1, "ChannelKey"),
            UsuarioRegistradoInitializerBuilder.GetUsuarioRegistradoWithDefaultValues("101112", true, "089132475438926439", DateTime.MinValue, 5, "ChannelKey"),
            UsuarioRegistradoInitializerBuilder.GetUsuarioRegistradoWithDefaultValues("131415", true, "4325897829057629", DateTime.MinValue, 1, "E31EF55584799620"),
            UsuarioRegistradoInitializerBuilder.GetUsuarioRegistradoWithDefaultValues("161718", true, "94746382649103", DateTime.MaxValue, 0, "0F91B0E6379813D2"),
            UsuarioRegistradoInitializerBuilder.GetUsuarioRegistradoWithDefaultValues("192021", true, "94746382649103", DateTime.MaxValue, 6, "0F91B0E6379813D2"),
            UsuarioRegistradoInitializerBuilder.GetUsuarioRegistradoWithDefaultValues("222324", true, "94746382649103", DateTime.MaxValue, 6, "0F91B0E6379813D2"),
            UsuarioRegistradoInitializerBuilder.GetUsuarioRegistradoWithDefaultValues("252627", true, "94746382649103", DateTime.MaxValue, 6, "0F91B0E6379813D2")
        };

        private static readonly List<HistorialClaveUsuarios> HistorialClaveUsuarios = new List<HistorialClaveUsuarios>
        {
            new HistorialClaveUsuarios
            {
                PasswordHistoryId = 1,
                UserId = 1,
                Password = "DzvVZ1a7J+SUZ/nI83o/OQXzKOhmAbiGSEu7LtNaXWc",
                CreationDate = DateTime.MinValue
            },
            new HistorialClaveUsuarios
            {
                PasswordHistoryId = 2,
                UserId = 6,
                Password = "+amMWpgHpovNljw1rAVsGOu/tq5taso93h9ehVobfV8", // Info1212
                CreationDate = DateTime.MinValue
            },
            new HistorialClaveUsuarios
            {
                PasswordHistoryId = 3,
                UserId = 13,
                Password = "+amMWpgHpovNljw1rAVsGOu/tq5taso93h9ehVobfV8", // Info1212
                CreationDate = DateTime.Now.AddDays(1)
            },
            new HistorialClaveUsuarios
            {
                PasswordHistoryId = 4,
                UserId = 13,
                Password = "BZ38lOxjf7XOw+WoQ466hR8RHy6XCrClrf0kKhuJNeo", // Info1213
                CreationDate = DateTime.Now.AddDays(2)
            },
            new HistorialClaveUsuarios
            {
                PasswordHistoryId = 5,
                UserId = 13,
                Password = "ZnZibQpuudsAf+wJNhcwtcDwqbz3qQ3/YI8rcV9mOnA", // Info1214
                CreationDate = DateTime.Now.AddDays(3)
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

        public static void Initialize(GenericDbContext context)
        {
            context.Database.EnsureCreated();

            context.Usuario.AddRange(Usuarios);

            TotalUsuarios = Usuarios.Count;

            for (var i = 0; i < TotalUsuarios; i++)
            {
                IdsUsuarios.Add(Usuarios.ElementAt(i).UserId);
            }

            context.Configuracion.AddRange(Configuraciones);
            context.Auditoria.AddRange(Auditorias);
            context.DatosUsuario.AddRange(DatosUsuarios);
            context.HistorialClaveUsuarios.AddRange(HistorialClaveUsuarios);
            context.UsuarioRegistrado.AddRange(ClaveCanales);
            context.DynamicImages.AddRange(DynamicImages);
            context.DynamicImagesLogin.AddRange(DynamicImagesLogin);
            context.SaveChanges();
        }
    }
}
