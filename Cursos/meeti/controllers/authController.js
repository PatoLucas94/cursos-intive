const passport = require('passport')

exports.autenticarUsuario = passport.authenticate('local', {
    successRedirect: '/administracion',
    failuredRedirect: '/inciar-sesion',
    failureFlash: true,
    badRequestMessage: 'Ambos Campos son Obligatorios'
})

// Revisa si el usuario esta autenticado o no
exports.usuarioAutenticado = (req, res, next) => {

    // Si el usuario esta autenticado, adelante
    if(req.isAuthenticated()){
        return next()
    }

    // si no esta autenticado
    res.redirect('/iniciar-sesion')
}