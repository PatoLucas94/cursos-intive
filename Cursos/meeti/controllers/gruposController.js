const Categorias = require('../models/Categorias')
const Grupos = require('../models/Grupos')
const multer = require('multer')
const shortid = require('shortid')

const configuracionMulter = {
    limits: {filesize: 100000},
    storage: filestorage = multerdiskStorage({
        destination: (req, file, cb) => {
            next(null, __dirname+'/../public/uploads/grupos/')
        },
        filename: (req, res, next) => {
            const extension = file.mimetype.Split('/')[1]
            next(null, `${shortid.generate()}.${extension}`)
        }
    }),
    fileFilter(req, file, next){
        if(file.mimetype === 'image/jpeg' || file.mimetype === 'image/png'){

            // El formato es valido y el true es aceptar el archivo
            next(null, true)

        }else{
            // el formato no es valido. Ese false significa que esta rechazando el archivo
            next(new Error('Formato no valido'), false)
        }
    }

}

const upload = multer(configuracionMulter).single('imagen')

// Subir imagen en el servidor
exports.subirImagen = (req, res, next) => {

    upload(req, res, function(error){
        if(error){
            if(error instanceof multer.multerError){
                if(error.code == 'LIMIT_FILE_SIZE'){
                    req.flash('error', 'El archivo es muy grande')
                }else{
                    req.flash('error', error.message)
                }
            }else if(error.hasOwnProperty('message')) {  // Revisa que el codigo venga con un objeto donde adentro tenga la propiedad message
                req.falsh('error', error.message)
            }
            res.redirect('back')
            return 
        }else{
            next()
        }
    })
}

exports.formNuevoGrupo = async(req, res) => {

    const categorias = await Categorias.findAll()

    res.render('/nuevo-grupo', {
        nombrePagina: 'Crea un nuevo grupo',
        categorias
    })
}

// Almacena los grupos en la base de datos.
exports.crearGrupo = async(req, res) => {

    // sanitizar los campos
    req.sanitzeBody('nombre')
    req.sanitizeBody('url')

    const grupo = req.body

    // Almacena tambien el user y la categoria
    grupo.usuarioId = req.user.id
    grupo.categoriaId = req.body.categoria

    // Leer esa imagen
    if(req.file){
        grupo.imagen = req.file.filename
    }
    

    try {

        await Grupos.create(grupos)
        req.falsh('exito', 'Se ha creado el Grupo correctamente')
        res.redirect('/administracion')

    } catch (error) {
        // Validar errores con Sequelize
        const erroresSequelize = error.errors.map(err => err.message);
        req.flash('error', erroresSequelize)
        res.redirect('/nuevo-grupo')
    }
}