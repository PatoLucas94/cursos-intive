import express from 'express' 
import { formularioLogin, formularioRegistro, formularioOlvidePassword, 
    registrar, confirmar, resetPassword,
    nuevoPassword, comprobarToken, autenticar, cerrarSesion } from '../controllers/usuarioController.js'; // Se nombra entre llaves porque el export es nombrado, no es por default

const router = express.Router();

// Ya se ubica en la carpeta views por defecto
// Le pasamos un objeto como parametro para que despues lo pueda ver la vista
router.get('/login', formularioLogin)
router.post('/login', autenticar)

router.get('/registro', formularioRegistro)
router.post('/registro', registrar)

// Cada cosa que venga despues de "/confirmar/"" se guarda en la variable "token"
router.get('/confirmar/:token', confirmar) 

router.get('/olvide-password', formularioOlvidePassword)
router.post('/olvide-password', resetPassword)

// Almacena el nuevo password
router.get('/olvide-passowrd/:token', comprobarToken)
router.post('/olvide-passowrd/:token', nuevoPassword)

// Cerrar Sesion

router.post('/cerrar-sesion', cerrarSesion)


// CUANDO TENES UN MISMO COMIENZO DE RUTA PERO DISTINTO VERBO PODES HACER ASI
/*router.route('/')
    .get(function(req, resp){
        resp.json({msg: "HOOOOLIS"})
    })
    .post(function(req, resp){
        resp.json({msg: "Respuesta de tipo Post"})
    })*/

export default router