const validator = require("validator");
const Articulo = require("../modelos/Articulo")

const prueba = (req, res) => {

    return res.status(200).json({
        mensaje: "Soy una accion de prueba en mi controlador de articulos"
    });
}

const curso =  (req, res) => {
    console.log("Se ha ejecutado el endpoint probando")

    return res.status(200).json([{
            curso: "Master en React",
            autor: "Patricio Gianni",
            url: "patricioGianniWeb.es/maste-react"
        },
        {
            curso: "Master en NodeJS",
            autor: "Patricio Gianni",
            url: "patricioGianniWeb.es/maste-nodejs"
        }
    ])
}


const crear = (req, res) => {

    // Recoger los parametros a guardar
    let parametros = req.body;

    // Validar los datos
    try{

        let validarTitulo = !validator.isEmpty(parametros.titulo) && // Si esto no esta vacio
                            validator.isLength(parametros.titulo, {min: 5, max: undefined}); 

        let validarContenido = !validator.isEmpty(parametros.contenido);

        if(!validarTitulo || !validarContenido){
            throw new Error("No se ha validado la informacion");
        }

    }catch(error){
        return res.status(400).json({
            status: error,
            mensaje: "Faltan datos para enviar",
        })
    }

    // Crear el objeto a guardar
    const articulo = new Articulo(parametros); // forma automatica

    // Asignar valores a objeto basado en el modelo (manual o automatico)
    // manual
    //articulo.titulo = parametros.titulo;

    // Guardar el articulo en la base de datos
    /*articulo.save((error, articuloGuardado) => {
        
        if(error || !articuloGuardado){
            return res.status(400).json({
                status: error,
                mensaje: "No se ha guardado el articulo",
            })
        }*/
    // Devolver el resultado
     articulo.save().then((articuloGuardado) => {

        if(!articuloGuardado){
            return res.status(400).json({
                status: error,
                mensaje: "No se ha guardado el articulo",
            })
        }

        return res.status(200).json({
            status: "success",
            articulo: articuloGuardado,
            mensaje: "El articulo se ha guardado con exito"

     })
    })

   

    
}

const listar = (req, res) => {

    let consulta = Articulo.find({})
                           .sort({fecha: 1})
                           .then((error, articulos) => {
 

        return res.status(200).send({
            status: "success",
            articulos
        })

    }); // .find es como un Select * from 

   

}

module.exports = {
    prueba,
    curso,
    crear,
    listar
}