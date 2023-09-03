const passport = require('passport')
const localStrategy = require('passport-local').Strategy
const mongoose = require('mongoose')
const Usuarios = mongoose.model('Usuarios')


// VALIDAR EL USUARIO
// done... errores, usuarios, mensaje
passport.use(new localStrategy({
    usernameField: 'email',
    passwordField: 'password'
}, async(email, password, done) => {

    const usuario = await Usuarios.findOne({email})

    if(!usuario) return done(null, false, {
        message: 'Usuario No Existente'
    })

    // El usuario existe, ahora verificamos el password
    const verificarPassword = usuario.compararPassword(password)
    if(!verificarPassword) return done(null, false, {
        message: 'Password Incorrecto'
    })

    // El usuaro existe y el password es correcto
    return done(null, usuario)
}))

passport.serializeUser((usuario, done) => done(null, usuario._id))

passport.deserializeUser(async(id, done) => {

    const usuario = await Usuarios.findById(id)
    return done(null, usuario)
})

module.exports = passport