import { check, validationResult } from "express-validator"
import bcrypt from 'bcrypt'
import Usuario from "../models/Usuario.js"
import {generarJwt, generarId} from '../helpers/tokens.js'
import {emailRegistro, emailOlvidePassword} from '../helpers/emails.js'

const formularioLogin = (req, res) =>{
    res.render('auth/login', {
        pagina: "Iniciar Sesion",
        csrfToken: req.csrfToken()
    }) 
}

const autenticar = async(req, res) => {

    // Validacion
    await check('email').isEmail().withMessage('El email es Obligatorio').run(req)
    await check('password').notEmpty().withMessage('El password es obligatorio').run(req)

    let resultado = validationResult(req)

    if(!resultado.isEmpty()){
        return res.render('auth/login', {
               pagina: 'Iniciar Sesion',
               csrfToken: req.csrfToken(),
               errores: resultado.array(),
        }) 
    }

    const {email, password} = req.body

    // comprobar si el usuario existe
    const usuario = await Usuario.findOne({where: {email}})
    if(!usuario){
        return res.render('auth/login', {
            pagina: 'Iniciar Sesion',
            csrfToken: req.csrfToken(),
            errores: [{msg: 'El Usuario no existe'}]
     }) 
    }

    // comprobar si el usuario esta confirmado
    if(!usuario.confirmado){
        return res.render('auth/login', {
            pagina: 'Iniciar Sesion',
            csrfToken: req.csrfToken(),
            errores: [{msg: 'Tu Cuenta no esta confirmada'}]
     }) 
    }

    // Revisar el password
    if(!usuario.verificarPassword(password)){
        return res.render('auth/login', {
            pagina: 'Iniciar Sesion',
            csrfToken: req.csrfToken(),
            errores: [{msg: 'El password es incorrecto'}]
     }) 
    }

    // Autenticar al usuario
    const token = generarJwt({id: usuario.id, nombre: usuario.nombre})

// Almacenar en un cookie
    return res.cookie('_token', token, {
        httpOnly: true,
       // secure: true,
        //sameSite: true
    }).redirect('/mis-propiedades')

}



// el res.render es renderizar una vista
const formularioRegistro = (req, res) =>{
    res.render('auth/registro', {
        pagina: 'Crear Cuenta',
        csrfToken: req.csrfToken()
    }) 
}

// Creando un usuario
const registrar = async (req, res) => {
    
    //Validacion
    await check('nombre').notEmpty().withMessage('El nombre es obligatorio').run(req)
    await check('email').isEmail().withMessage('Eso no parece un email').run(req)
    await check('password').isLength({min: 6}).withMessage('El password debe ser al menos 6 caracteres').run(req)
    await check('repetir_password').equals(req.body.password).withMessage('Los passwords no son iguales').run(req)


    let resultado = validationResult(req)

    // Verificar que el resultado este o no vacio, 
    // si esta vacio signfica que no hay ningun error
    // si NO esta vacio, significa que hay errores
    if(!resultado.isEmpty()){
        // Errores
        return res.render('auth/registro', {
               pagina: 'Crear Cuenta',
               csrfToken: req.csrfToken(),
               errores: resultado.array(),
               usuario: {
                nombre: req.body.nombre,
                email: req.body.email
               }
        }) 
    }

    // Extraer los datos
    const {nombre, email, password} = req.body;

    // Verificar que el usuario no este duplicado

    const existeUsuario = await Usuario.findOne({where: {email}});
    if(existeUsuario){
        return res.render('auth/registro', {
            pagina: 'Crear Cuenta',
            csrfToken: req.csrfToken(),
            errores: [{msg: 'El usuario ya esta registrado'}],
            usuario: {
             nombre: req.body.nombre,
             email: req.body.email
            }
     }) 
    }
    
    const usuario = await Usuario.create({
                        nombre,
                        email,
                        password,
                        token: generarId()
                    })

    // Envia email de confirmacion
                
    emailRegistro({
        nombre: usuario.nombre,
        email: usuario.email,
        token: usuario.token
    })


    // Mostrar mensaje de confirmacion

    res.render('templates/mensaje', {
        pagina: 'Cuenta creada correctamente',
        mensaje: 'Hemos enviado un email de confirmacion, presiona en el enlace para verlo'
    })

  

}


// FUNCION QUE COMPRUEBA UNA CUENTA

const confirmar = async(req, res) => {
    const {token} = req.params; // Los parametros que vienen en la url

    // Verificar si el token es valido 
    const usuario = await Usuario.findOne({where: {token}})

    if(!usuario){
        return res.render('auth/confirmar-cuenta', {
            pagina: 'Error al confirmar tu cuenta',
            mensaje: 'Hubo en error al confirmar tu cuenta, por favor intentalo de nuevo',
            error: true
        })
    }


    // Confirmar la cuenta. Ya use el token entonces ya no lo necesito
    usuario.token = null
    usuario.confirmado = true
    await usuario.save();

    res.render('auth/confirmar-cuenta', {
        pagina: 'Cuenta confirmada',
        mensaje: 'La cuenta se confirmo correctamente'
    })

}



const formularioOlvidePassword = (req, res) =>{
    res.render('auth/olvide-password', {
        pagina: 'Recupera tu acceso a Bienes Raices',
        csrfToken: req.csrfToken(),
    }) 
}

const resetPassword = async(req, res) => {

    await check('email').isEmail().withMessage('Eso no parece un email').run(req)
    
    let resultado = validationResult(req)

    if(!resultado.isEmpty()){
        return res.render('auth/olvide-password', {
            pagina: 'Recupera tu acceso a Bienes Raices',
            csrfToken: req.csrfToken(),
            errores: resultado.array()   
        }) 
    }


    // Buscar el usuario si esta registrada y asi enviar el email

    const {email} = req.body

    const usuario = await Usuario.findOne({where: {email}})

    if(!usuario){
        return res.render('auth/olvide-password', {
            pagina: 'Recupera tu acceso a Bienes Raices',
            csrfToken: req.csrfToken(),
            errores: [{msg: 'El email no pertenece a ningun usuario'}]  
        }) 
    }

    // Generar un token y enviar el email
    usuario.token = generarId();
    await usuario.save()

    // Enviar un email
    emailOlvidePassword({
        email: usuario.email,
        nombre: usuario.nombre,
        token: usuario.token
    })

    // Renderizar un mensaje
    res.render('templates/mensaje', {
        pagina: 'Reestablece tu password',
        mensaje: 'Hemos enviado un email con las instrucciones'
    })
}

const comprobarToken = async(req, res) => {
   
    const {token} = req.params

    const usuario = await usuario.findOne({where: {token}})
    if(!usuario){
        return res.render('auth/confirmar-cuenta', {
            pagina: 'Reestablece tu password',
            mensaje: 'Hubo en error al reestablecer tu password, por favor intentalo de nuevo',
            error: true
        })
    }

    // Mostrar formulario para modificar la password
    res.render('auth/reset-password', {
        pagina: 'Reestablece Tu Password',
        csrfToken: req.csrfToken(),
    })

}

const nuevoPassword = async(req, res) => {

    // Validar el password
    await check('password').isLength({min: 6}).withMessage('El password debe ser al menos 6 caracteres').run(req)

    let resultado = validationResult(req)

    if(!resultado.isEmpty()){
        return res.render('auth/reset-password', {
               pagina: 'Reestablece tu password',
               csrfToken: req.csrfToken(),
               errores: resultado.array(),
               
        }) 
    }

    const {token} = req.params
    const {password} = req.body

    // Identificar quien hace el cambio

    const usuario = await Usuario.findOne({where: {token}})

    // Hashear el password
    const salt = await bcrypt.genSalt(10)
    usuario.password = await bcrypt.hash(password, salt);
    usuario.token = null

    await usuario.save()

    res.render('auth/confirmar-cuenta', {
        pagina: 'Password Reestablecido',
        mensaje: 'El Password se guardo correctamente'
    })
}


const cerrarSesion = async(req, resp) => {
    return res.clearCookie('_token').status(200).redirect('auth/login')
}

export {
    formularioLogin,
    formularioRegistro,
    formularioOlvidePassword,
    registrar,
    confirmar,
    resetPassword,
    comprobarToken,
    nuevoPassword,
    autenticar,
    cerrarSesion
}