using System.Threading.Tasks;
using Spv.Usuarios.Common.Dtos.UsuariosService.Input;
using Spv.Usuarios.Common.Dtos.UsuariosService.Output;
using Spv.Usuarios.Domain.Services;

namespace Spv.Usuarios.Service.Interface
{
    public interface IUsuariosService
    {
        Task<IResponse<AutenticacionModelOutput>> AutenticarAsync(
            IRequestBody<AutenticacionModelInput> autenticacionModel
        );

        Task<IResponse<AutenticacionClaveNumericaModelOutput>> AutenticarConClaveNumericaAsync(
            IRequestBody<AutenticacionClaveNumericaModelInput> autenticacionClaveNumericaModel
        );

        Task<IResponse<MigracionModelOutput>> MigrarAsync(IRequestBody<MigracionModelInput> migracionModel);

        Task<IResponse<PerfilModelOutput>> ObtenerPerfilAsync(IRequestBody<PerfilModelInput> perfilModel);

        Task<IResponse<PerfilModelOutput>> ObtenerPerfilAsync(IRequestBody<PerfilModelInputV2> perfilModelV2);

        Task<IResponse<PerfilMigradoModelOutput>> ObtenerPerfilAsync(
            IRequestBody<PerfilMigradoModelInput> perfilMigradoModel
        );

        Task<IResponse<PerfilMigradoModelOutput[]>> ObtenerPerfilesAsync(
            IRequestBody<PerfilMigradoModelInput> perfilMigradoModel
        );

        Task<IResponse<RegistracionModelOutput>> RegistrarV1Async(
            IRequestBody<RegistracionModelInput> registracionModel
        );

        Task<IResponse<RegistracionModelOutput>> RegistrarV2Async(
            IRequestBody<RegistracionModelInputV2> registracionModel
        );

        Task<IResponse<ValidacionExistenciaModelOutput>> ValidarExistenciaAsync(
            IRequestBody<ValidacionExistenciaModelInput> validacionExistenciaModel
        );

        Task<IResponse<ValidacionExistenciaHbiModelOutput>> ValidarExistenciaHbiAsync(
            IRequestBody<ValidacionExistenciaHbiModelInput> validacionExistenciaHbiModel
        );

        Task<IResponse<ValidacionExistenciaBtaModelOutput>> ValidarExistenciaBtaAsync(
            IRequestBody<ValidacionExistenciaBtaModelInput> validacionExistenciaBtaModel
        );

        Task<IResponse<CambioDeClaveModelOutput>> ModificarClaveAsync(
            IRequestBody<CambioDeClaveModelInputV2> cambioDeClaveModelV2
        );

        Task<IResponse<CambioDeCredencialesModelOutput>> ModificarCredencialesAsync(
            IRequestBody<CambioDeCredencialesModelInputV2> cambioDeCredencialesModelV2
        );

        Task<IResponse<object>> CambiarEstadoAsync(IRequestBody<CambioEstadoModelInput> cambioEstadoModel);

        Task<IResponse<ActualizarPersonIdModelOutput>> ActualizarPersonIdAsync(
            IRequestBody<ActualizarPersonIdModelInput> actualizarPersonIdModel
        );

        Task<IResponse<PerfilModelOutput>> ObtenerUsuarioAsync(
            IRequestBody<ObtenerUsuarioModelInput> obtenerUsuarioModel
        );
    }
}
