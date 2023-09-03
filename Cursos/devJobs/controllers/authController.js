const passport = require('passport')
const mongoose = require('mongoose')
const Vacante = mongoose.model('Vacante')
const Usuarios = mongoose.model('Usuarios')
const crypto = require('crypto')
const enviarEmail = require('../handlers/email')


exports.autenticarUsuario = passport.authenticate('local', {
    successRedirect: '/administracion',
    failureRedirect: '/iniciar-sesion',
    failureFlash: true,
    badRequestMessage: 'Ambos Campos Son Obligatorios'
})


// Revisar si el usuario está autenticado o no
exports.verificarUsuario = (req, res, next) => {

    // revisar el usuario
    if(req.isAuthenticated()){
        return next() // Esta autenticado
    }

    // redireccionar
    res.redirect('/iniciar-sesion')

}


exports.mostrarPanel = async(req, res) => {

    // consultar el usuario autenticado
    const vacantes = await Vacante.find({autor: req.user._id}).lean()

    console.log(req.user._id)
    console.log(vacantes)

    res.render('administracion', {
        nombrePagina: 'Panel de Administracion',
        tagLine: 'Crea y Administra tus vacantes desde aqui',
        cerrarSesion: true,
        nombre: req.user.nombre,
        imagen: req.user.imagen,
        vacantes
    })
} 

exports.cerrarSesion = (req, res) => {

    req.logout() // Asi se cierra sesion
    req.flash('correcto', 'Cerraste Sesion Correctamente')
    return res.redirect('/iniciar-sesion')
}

// Formulario para reiniciar el password
exports.formReestablecerPassword = (req, res) => {
    res.render('reestablecer-password', {
        nombrePagina: 'Reestablece tu Password',
        tagLine: 'Si ya tienes una cuenta pero olvidaste tu password, coloca tu email'
    })
}

// genera el token en la tabla de Usuario
exports.enviarToken = async(req, res) => {
    const usuario =  await Usuarios.findOne({email: req.body.email})

    if(!usuario){
        req.flash('error', 'No existe esa cuenta')
        return res.redirect('/iniciar-sesion')
    }

    // el usuario existe, generar token
    usuario.token = crypto.randomBytes(20).toString('hex')
    usuario.expira = Date.now() + 3600000

    // guardar el usuario
    await usuario.save()
    const resetUrl = `http:${req.headers.host}/reestablecer-password/${usuario.token}`

    // Enviar email
    await enviarEmail.enviar({
        usuario,
        subject: 'Password Reset',
        resetUrl,
        archivo: 'reset'
    })

    req.flash('correcto', 'Revisa tu Email para las indicaciones')
    res.redirect('/iniciar-sesion')


}

// Valida si el token es valido y el usuario existe, muestra la vista
exports.reestablecerPassword = async(req, res) => {
    const usuario = await Usuarios.findOne({
        token: req.params.token,
        expira: {
            $gt: Date.now() // Es como decirle una hora mas
        }
    })

    if(!usuario){
        req.flash('error', 'Formulario ya no es valido, intenta de nuevo')
        return res.redirect('/reestablecer-password')
    }

    // Todo bien, mostrar el formulario

    res.render('/nuevo-password', {
        nombrePagina: 'Nuevo Password'
    })
}

// Almacena el nuevo password en la base de datos
exports.guardarPassword = async(req, res) => {

    const usuario = await Usuarios.findOne({
        token: req.params.token,
        expira: {
            $gt: Date.now() // Es como decirle una hora mas
        }
    })

    // no existe el usuario o el token es invalido
    if(!usuario){
        req.flash('error', 'Formulario ya no es valido, intenta de nuevo')
        return res.redirect('/reestablecer-password')
    }

    // Asignar nuevo password y limpiar valores previos
    usuario.password = req.body.password
    usuario.token = undefined
    usuario.expira = undefined

    // guardar en la base de datos
    await usuario.save()

    // redirigir
    req.flash('correcto', 'Password Modificado Correctamente')
    res.redirect('/iniciar-sesion')
}