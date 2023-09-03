const mongoose = require('mongoose')
const Vacante = mongoose.model('Vacante')

exports.mostrarTrabajos = async(req, res, next) => {

    const vacantes = await Vacante.find().lean(); 
    //lean() ---> Pasame la clase simple, solo sus propiedades y no todo el documento.

    if(!vacantes) return next()

    res.render('home', {
        nombrePagina: 'DevJobs',
        tagLine: 'Encuentra y Publica trabajos para desarrolladores web',
        barra: true,
        boton: true,
        vacantes
    })
}