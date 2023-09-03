using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Spv.Usuarios.Common.Constants;
using Spv.Usuarios.Common.Dtos.UsuariosService.Input;
using Spv.Usuarios.Common.Dtos.UsuariosService.Output;
using Spv.Usuarios.DataAccess.ExternalWebService.Helpers;
using Spv.Usuarios.DataAccess.ExternalWebService.Interfaces;

namespace Spv.Usuarios.DataAccess.ExternalWebService
{
    public class BtaRepository : IBtaRepository
    {
        private readonly IBtaHelper _btaHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public BtaRepository(
                  IBtaHelper btaHelper,
                  ILogger<BtaRepository> logger)
        {
            _btaHelper = btaHelper;
        }

        public async Task<ObtenerPinModelOutput> ObtenerPinAsync(string numeroDocumento, int tipoDocumento, int paisDocumento , string token)
        {
            ObtenerPinModelOutput pinBTA;

            var path = ObtenerPathConBase(_btaHelper.ObtenerPinPath());

            var bodybti = new BtinreqPin()
            {
                Canal = BtaConstants.Canal,
                Token = token,
                Usuario = BtaConstants.Usuario,
                Requerimiento = BtaConstants.Requerimiento,
                Device = BtaConstants.Device
            };
            var body = new ObtenerPinModelInput() 
            {
                Btinreq = bodybti,
                PaisDocumento = paisDocumento.ToString(),
                TipoDocumento = tipoDocumento.ToString(),
                NumeroDocumento = numeroDocumento,
                Modo = BtaConstants.Modo
            };

            var response = await _btaHelper.PostAsync(
              path,
              body
            );

            if (response.IsSuccessStatusCode)
            {
                await using var responseStream = await response.Content.ReadAsStreamAsync();
                pinBTA = await JsonSerializer.DeserializeAsync<ObtenerPinModelOutput>(responseStream);
            }
            else
            {
                pinBTA = null;
            }

            return pinBTA;

        }

    public async Task<TokenBtaModelOutput> ObtenerToken()
        {
            TokenBtaModelOutput tokenBTA;

            var path = ObtenerPathConBase(_btaHelper.TokenBtaPath());

            var bodybti = new Btinreq()
            {
                Device = BtaConstants.Device,
                Usuario = BtaConstants.Usuario,
                Canal = BtaConstants.Canal,
                Requerimiento = BtaConstants.Requerimiento
            };
            var body = new TokenBtaModelInput()
            {
                UserId = BtaConstants.UserId,
                UserPassword = _btaHelper.UserPassword(),
                Btinreq = bodybti
            };

            var response =  await _btaHelper.PostAsync(
                path,
                body
            );

            if (response.IsSuccessStatusCode)
            {
                await using var responseStream = await response.Content.ReadAsStreamAsync();
                tokenBTA = await JsonSerializer.DeserializeAsync<TokenBtaModelOutput>(responseStream);
            }
            else
            {
                tokenBTA = null;
            }

            return tokenBTA;
        }

        private string ObtenerPathConBase(string path)
        {
            return string.Concat(_btaHelper.BtaBasePath(), path);
        }
    }
}
