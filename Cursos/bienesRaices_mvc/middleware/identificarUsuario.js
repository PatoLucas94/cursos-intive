import jwt from 'jsonwebtoken'
import Usuario from '../models/Usuario.js'

const identificarUsuario = async(req, res, next) => {
   // Identificar si hay un token en la cookies 
   
   const token = req.cookies._token
    if(!token){
        req.Usuario = null
        return next()
    }

    // Comprobar el token

    try {

        const decoded = jwt.verify(token, process.env.JWT_SECRET)
        const usuario = await Usuario.scope('eliminarPassword').findByPk(decoded.id)

        // Almacenar el usuario al req

        if(usuario){
            req.usuario = usuario
        }
        
        return next()

    } catch (error) {
        console.log(error)
        return res.clearCookie('_token').redirect('auth/login')
    }

}