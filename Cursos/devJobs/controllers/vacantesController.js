const mongoose = require('mongoose')
const Vacante = mongoose.model('Vacante')
const multer = require('multer')
const shortid = require('shortid')


exports.formularioNuevaVacante = (req, res) => {
    res.render('nueva-vacante', {
        nombrePagina: 'Nueva Vacante',
        tagLine: 'Llena el formulario y publica tu vacante',
        cerrarSesion: true,
        nombre: req.user.nombre,
        imagen: req.user.imagen,
    })
}

// Agrega las vacates a la base de datos
exports.agregarVacante = async(req, res) => {
    const vacante = new Vacante(req.body) // Se mapean los campos automaticamente
    
    // Usuario autor de la vacante
    vacante.autor = req.user._id // Aca se guarda el usuario en un request

    // crear arreglo de skills
    vacante.skills = req.body.skills.split(',')

    // Almacenarlo en la base de datos
    const nuevaVacante = await vacante.save()

    // redireccionar a la pagina para que nos lleve a la nueva vacante creada y asi poderla ver
    res.redirect(`/vacantes/${nuevaVacante.url}`) 
}

// Muestra una Vacante, estamos filtrando por la url
exports.mostrarVacante = async(req, res, next) => {

    // el populate hace el join con el autor y busca toda la info
    const vacante = await Vacante.findOne({url: req.params.url}).populate('autor').lean()

    if(!vacante) return next()

    res.render('vacante', {
        vacante, // Pasamos el objeto vacante
        nombrePagina: vacante.titulo,
        barra: true
    })


}

exports.formEditarVacante = async(req, res, next) => {

    const vacante = await Vacante.findOne({url: req.params.url}).lean()
    if(!vacante) return next()

    res.render('editar-vacante', {
        vacante,
        nombrePagina: `Editar - ${vacante.titulo}`,
        cerrarSesion: true,
        nombre: req.user.nombre,
        imagen: req.user.imagen,
    })

}

exports.editarVacante = async(req, res, next) => {

    const vacanteActualizada =  req.body
    vacanteActualizada.skills = req.body.skills.split(',')

    // el segund parametro es "con que lo vas a actualizar"... con el body que venga en el request
    const vacante = await Vacante.findOneAndUpdate({url: req.params.url}, vacanteActualizada, {
        new: true, // Que me retorne el objeto nuevo actualizado
        runValidators: true
    })

    res.redirect(`/vacantes/${vacante.url}`)
}

// Validar y sanitizar los campos de las nuevas vacantes
exports.validarVacante = (req, res, next) => {

    const rules = [
        body("titulo").not().isEmpty().withMessage("Agrega un titulo a la vacante").escape(),
        body("empresa").isEmail().withMessage("Agrega una empresa").escape(),
        body("ubicacion").not().isEmpty().withMessage("Agrega una ubicacion").escape(),
        body("contrato").not().isEmpty().withMessage("Selecciona un tipo de contrato").escape(),
        body("skills").not().isEmpty().withMessage("Agrega al menos una habilidad").escape(),
      ];

      const errores = req.validationErrors()

      if(errores){
        // Recargar la vista con los errores
        req.flash('error', rules.map(error => error.msg))
        res.render('nueva-vacante', {
            nombrePagina: 'Nueva Vacante',
            tagLine: 'Llena el formulario y publica tu vacante',
            cerrarSesion: true,
            nombre: req.user.nombre,
            mensajes: req.flas()
        })
      }

      next()
}

exports.eliminarVacante = async (req, res) => {

    const {id} = req.params
    const vacante = await Vacante.findById(id)

    if(verificarAutor(vacante, req.user)){
        // todo bien, es el usuario. Sino eliminar
        vacante.remove()
        res.status(200).send('Vacante eliminada correctamente')

    }else {
        res.status(403).send('Error')
    }

    

}


const verificarAutor = (vacante = {}, usuario = {}) => {
    if(!vacante.autor.equals(usuario._id)){
        return false
    }
}

// Subir archivos en PDF
exports.subirCV = (req, res, next) => {

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

            res.redirect('back')
            return

        }else{
            return next()
        }
        
    })
}

const configuracionMulter = {
    limits: {filseSize: 100000} ,
    storage: fileStorage = multer.diskStorage({
        destination: (req, file, callback) => {
            callback(null, __dirname+'../../public/uploads/cv')
        },
        filename: (req, res, cb) => {
            const extension = file.mimetype.split('/')[1]; // Extension de las imagenes
            cb(null, `${shortid.generate()}.${extension}}`)
        } 
    }),
    fileFilter(req, file, cb){
        if(file.mimetype === 'application/pdf'){
            // El callback se ejecuta como true o false. True cuando la imagen se acepta
            cb(null, true)
        }else{
            cb(new Error('Formato no valido'), false)
        }
    },
    
}

// La "imagen" es el name del html
const upload = multer(configuracionMulter).single('cv')


// Almacenar los candidatos en la base de datos
exports.contactar = async(req, res, next) => {
    const vacante = await Vacante.findOne({url: req.params.url})

    if(!vacante) return next()

    const nuevoCandidato = {
        nombre: req.body.name,
        email: req.body.email,
        cv: req.file.filename
    }

    // Almacenar la vacante
    vacante.candidatos.push(nuevoCandidato)
    await vacante.save()

    // mensaje y redireccion
    req.flash('correcto', 'Se envio tu curriculum correctamente')
    res.redirect('/')
}

exports.mostrarCandidatos = async (req, res, next) => {

    const vacante = await Vacante.findById(req.params.id)

    if(vacante.autor != req.user._id.toString()){
        return next()
    }

    if(!vacante) return next()

    res.render('/candidatos', {
        nombrePagina: `Candidato Vacante - ${vacante.titulo}`,
        cerrarSesion: true,
        nombre: req.user.nombre,
        imagen: req.user.image,
        candidatos: vacante.candidatos
    })


}


// Buscador de Vacantes
exports.buscarVacantes = async(req, res) => {
    const vacantes = await Vacante.find({
        $text: {
            $search: req.body.q
        }
    })

    res.render('home', {
        nombrePagina: `Resultados para la busqueda: ${req.body.q}`,
        barra: true,
        vacantes
    })
}