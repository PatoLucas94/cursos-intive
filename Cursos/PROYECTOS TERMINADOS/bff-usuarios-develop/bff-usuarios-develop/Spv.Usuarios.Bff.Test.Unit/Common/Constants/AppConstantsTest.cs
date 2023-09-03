using System;
using FluentAssertions;
using Spv.Usuarios.Bff.Common.Constants;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Unit.Common.Constants
{
    public class AppConstantsTest
    {
        [Fact]
        public void GetTipoDocumentoDescTest()
        {
            // Arrange
            var tiposDocumento = Enum.GetValues(typeof(TipoDocumento));

            // Act - Assert
            foreach (var tipoDocumento in tiposDocumento)
            {
                var desc = AppConstants.GetTipoDocumentoDesc((int)tipoDocumento);

                switch (tipoDocumento)
                {
                    case TipoDocumento.Cuit:
                        desc.Should().Be(AppConstants.Cuit);
                        break;
                    case TipoDocumento.Cuil:
                        desc.Should().Be(AppConstants.Cuil);
                        break;
                    case TipoDocumento.Cdi:
                        desc.Should().Be(AppConstants.Cdi);
                        break;
                    case TipoDocumento.Dni:
                        desc.Should().Be(AppConstants.Dni);
                        break;
                    case TipoDocumento.Le:
                        desc.Should().Be(AppConstants.LibretaEnrolamiento);
                        break;
                    case TipoDocumento.Lc:
                        desc.Should().Be(AppConstants.LibretaCivica);
                        break;
                    case TipoDocumento.CedulaProv:
                        desc.Should().Be(AppConstants.CedulaProv);
                        break;
                    case TipoDocumento.CiPaisLimit:
                        desc.Should().Be(AppConstants.CiPaisLimit);
                        break;
                    case TipoDocumento.PjExranjera:
                        desc.Should().Be(AppConstants.PjExranjera);
                        break;
                    case TipoDocumento.Pasaporte:
                        desc.Should().Be(AppConstants.Pasaporte);
                        break;
                    case TipoDocumento.PfExtResExt:
                        desc.Should().Be(AppConstants.PfExtResExt);
                        break;
                    case TipoDocumento.ExpedJudicial:
                        desc.Should().Be(AppConstants.ExpedJudicial);
                        break;
                    case TipoDocumento.CertifMigracion:
                        desc.Should().Be(AppConstants.CertifMigracion);
                        break;
                    case TipoDocumento.Fci:
                        desc.Should().Be(AppConstants.Fci);
                        break;
                    case TipoDocumento.Vuelco:
                        desc.Should().Be(AppConstants.Vuelco);
                        break;
                    case TipoDocumento.InstFin:
                        desc.Should().Be(AppConstants.InstFin);
                        break;
                    default:
                        desc.Should().Be(AppConstants.TipoDocNoControlado);
                        break;
                }
            }
        }

        [Fact]
        public void GetTipoDocumentoNoControladoDescTest()
        {
            // Arrange
            var tipoDocumento = -1;

            // Act 
            var desc = AppConstants.GetTipoDocumentoDesc(tipoDocumento);

            // Assert
            desc.Should().Be(AppConstants.TipoDocNoControlado);

            // Arrange
            tipoDocumento = 0;

            // Act 
            desc = AppConstants.GetTipoDocumentoDesc(tipoDocumento);

            // Assert
            desc.Should().Be(AppConstants.TipoDocNoControlado);

            // Arrange
            tipoDocumento = int.MaxValue;

            // Act 
            desc = AppConstants.GetTipoDocumentoDesc(tipoDocumento);

            // Assert
            desc.Should().Be(AppConstants.TipoDocNoControlado);
        }
    }
}
