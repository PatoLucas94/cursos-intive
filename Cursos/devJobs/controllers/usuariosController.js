const mongoose = require('mongoose')
const Usuarios = mongoose.model('Usuarios')
const { body, sanitizeBody, validationResult } = require('express-validator');
const multer = require('multer')
const shortid = require('shortid')


exports.subirImagen = (req, res) => {

    upload(req, res, function(error){
        if(error){

            if(error instanceof multer.MulterError){

                if(error.code === 'LIMIT_FILE_SIZE'){
                    req.flash('error', 'El archivo es muy grande: Máximo 100KB')
                }else{
                    req.flash('error', error.message)
                }

            }else {
                req.flash('error', error.message)
            }

            res.redirect('/administracion')
            return

        }else{
            return next()
        }
        
    })
}

// Opciones de MULTER
const configuracionMulter = {
    limits: {filseSize: 100000} ,
    storage: fileStorage = multer.diskStorage({
        destination: (req, file, callback) => {
            callback(null, __dirname+'../../public/uploads/perfiles')
        },
        filename: (req, res, cb) => {
            const extension = file.mimetype.split('/')[1]; // Extension de las imagenes
            cb(null, `${shortid.generate()}.${extension}}`)
        } 
    }),
    fileFilter(req, file, cb){
        if(file.mimetype === 'image/jpeg' || file.mimetype === 'image/png'){
            // El callback se ejecuta como true o false. True cuando la imagen se acepta
            cb(null, true)
        }else{
            cb(new Error('Formato no valido'), false)
        }
    },
    
}

// La "imagen" es el name del html
const upload = multer(configuracionMulter).single('imagen')



exports.formCrearCuenta = (req, res) => {
    res.render('crear-cuenta', {
        nombrePagina: 'Crea tu Cuenta en DevJobs',
        tagLine: 'Comienza a publicar tus vacantes gratis, solo debes crear una cuenta'
    })
}

exports.validarRegistro = async(req, res, next) => {

    // Sanitizar... limpiar el body ante posibles malas intenciones. Escapar los datos

    const rules = [
        body('nombre').escape(),
        body('email').escape(),
        body('password').escape(),
        body('confirmar').escape(),
        body('nombre').not().isEmpty().withMessage('El nombre es Obligatorio'),
        body('email').isEmail().withMessage('El email debe ser valido'),
        body('password').not().isEmpty().withMessage('El password no puede ir vacío'),
        body('confirmar').not().isEmpty().withMessage('Confirmar password no puede ir vacío'),
        body('confirmar').equals(req.body.password).withMessage('El password es diferente')
    ];

    await Promise.all(rules.map( validation => validation.run(req)));
    const errores = validationResult(req);
 
    if(errores.isEmpty()){
        return next();
    }

    req.flash('error', errores.array().map(error => error.msg));

    res.render('crear-cuenta', {
        nombrePagina: 'Crea tu cuenta en devJobs',
        tagline: 'Comienza a publicar tus vacantes gratis, solo debes crear una cuenta',
        mensajes: req.flash()
    });

    return;
}

exports.crearUsuario = async (req, res, next) => {

    // Crear el usuario
    const usuario = new Usuarios(req.body)

    

    try {
        await usuario.save()
        res.redirect('/iniciar-sesion')
    } catch (error) {
        req.flash('error', error) // Se ejecuta el usuariosSchema.post
        res.redirect('/crear-cuenta')
    }

    
}

// Formulario para Iniciar Sesion
exports.formIniciarSesion = (req, res) => {

    res.render('iniciar-sesion', {
        nombrePagina: 'Iniciar Sesion DevJobs'
    })
}

// Form editar el perfil del autor
exports.formEditarPerfil = (req, res) => {
    res.render('editar-perfil', {
        nombrePagina: 'Edita tu perfil en DevJobs',
        usuario: req.user.toObject(),// ahi van todos los datos del usuario
        cerrarSesion: true,
        nombre: req.user.nombre,
        imagen: req.user.imagen
         })
}

// Guardar cambios al editarPerfil
exports.editarPerfil = async(req, res) => {
    const usuarios = await Usuarios.findById(req.user._id)

    usuario.nombre = req.body.nombre
    usuario.email = req.body.email
    if(req.body.password){
        usuario.password = req.body.password
    }

    // req.file es donde viene todo lo que tenga que ver con imagenes desde el request
    if(req.file){
        usuario.imagen = req.file.filename
    }

    await usuario.save()
    req.flash('correcto', 'Cambios Guardados Correctamente')
    

    res.redirect('/administracion')
}

// Sanitizar y validar el formulario
exports.validarPerfil = (req, res, next) => {


    if(req.body.password){

        const rules = [

            body("nombre").not().isEmpty().withMessage("Agrega un nombre").escape(),
            body("email").isEmail().withMessage("Agrega un email").escape(),
     
          ];
    
    const errores = req.validationErrors()

      if(errores){
        // Recargar la vista con los errores
        req.flash('error', rules.map(error => error.msg))
        res.render('editar-perfil', {
            nombrePagina: 'Edita tu perfil en DevJobs',
            usuario: req.user.toObject(),// ahi van todos los datos del usuario
            cerrarSesion: true,
            nombre: req.user.nombre,
            imagen: req.user.imagen,
            mensajes: req.flash()
             })
      }
    next()
    
    }else {


        const rules = [
            body("nombre").not().isEmpty().withMessage("Agrega un nombre").escape(),
            body("email").isEmail().withMessage("Agrega un email").escape(),
            body("password").not().isEmpty().withMessage("Agrega un Password").escape(),
     
          ];

          const errores = req.validationErrors()

      if(errores){
        // Recargar la vista con los errores
        req.flash('error', rules.map(error => error.msg))
        res.render('editar-perfil', {
            nombrePagina: 'Edita tu perfil en DevJobs',
            usuario: req.user.toObject(),// ahi van todos los datos del usuario
            cerrarSesion: true,
            nombre: req.user.nombre,
            mensajes: req.flash()
             })
      }
    next()

    }

    
}

