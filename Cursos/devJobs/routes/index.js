const express = require('express')
const router = express.Router()
const homeController = require('../controllers/homeController.js')
const vacantesController = require('../controllers/vacantesController.js')
const usuariosController = require('../controllers/usuariosController.js')
const authController = require('../controllers/authController.js')

module.exports = () => {
    router.get("/", homeController.mostrarTrabajos)

    // Crear vacantes
    router.get("/vacantes/nueva", 
        authController.verificarUsuario, // Si pasa la verificacion, pasa al siguiente middleware
        vacantesController.formularioNuevaVacante)
    router.post("/vacantes/nueva",
        authController.verificarUsuario,
        vacantesController.validarVacante,
        vacantesController.agregarVacante)

    // Mostrar Vacante (singular)
    router.get('/vacantes/:url', vacantesController.mostrarVacante)

    // Editar Vacante
    router.get("/vacantes/editar/:url", 
        authController.verificarUsuario,
        vacantesController.formEditarVacante)
    router.post("/vacantes/editar/:url", 
        authController.verificarUsuario,
        vacantesController.validarVacante,
        vacantesController.editarVacante)

    // Eliminar vacantes
    router.delete('/vacantes/eliminar/:id', 
                  vacantesController.eliminarVacante)

    // Crear Cuentas
    router.get('/crear-cuenta', usuariosController.formCrearCuenta)
    router.post('/crear-cuenta', 
                    usuariosController.validarRegistro, // Una vez que termina de validar pasa al siguiente middleware y asi
                    usuariosController.crearUsuario
    )

    // Autenticar usuarios
    router.get('/iniciar-sesion', usuariosController.formIniciarSesion)
    router.post('/iniciar-sesion', authController.autenticarUsuario)

    // Resetear password (emails)
    router.get('/reestablecer-password', 
                authController.formReestablecerPassword)
    router.post('/reestablecer-password', 
                authController.enviarToken)

    // Resetear password y almacenar en la base de datos
    router.get('/reestablecer-password/:token', 
                authController.reestablecerPassword)
    router.post('/reestablecer-password/:token', 
                authController.guardarPassword)

    // Cerrar sesion
    router.get("/cerrar-sesion", 
                authController.verificarUsuario,
                authController.cerrarSesion)

    // Panel de Administracion
    router.get("/administracion", 
        authController.verificarUsuario,
        authController.mostrarPanel)

    // Editar Perfil del Autor
    router.get('/editar-perfil', 
        authController.verificarUsuario,
        usuariosController.formEditarPerfil)

    router.post('/editar-perfil',
                authController.verificarUsuario,
                //usuariosController.validarPerfil,
                usuariosController.subirImagen,
                usuariosController.editarPerfil)

    // Recibir mensajes de candidatos
    router.post('/vacantes/:url',
                vacantesController.subirCV,
                vacantesController.contactar        
    )

    // Muestra los candidatos por vacante
    router.get('/candidatos/:id', 
               authController.verificarUsuario,
               vacantesController.mostrarCandidatos
            )

    // Buscador de vacantes
    router.post('/buscador', vacantesController.buscarVacantes)
    return router
}