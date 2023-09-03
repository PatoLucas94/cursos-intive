import {unlink} from 'node:fs/promises'
import {validationResult} from 'express-validator'
import {Precio, Categoria, Propiedad, Mensaje, Usuario} from '../models/index.js'
import { esVendedorm, formatearFecha } from '../helpers/index.js'

const admin = async(req, res) => {

    // Leer QueryString
    const {pagina: paginaActual} = req.query

    const expresion = /^[1-9]$/ // Acepta digitos del 0 al 9, que inicie y termine con digitos
    
    // Probar una expresion regular
    if(!expresion.test(paginaActual)){
        return res.redirect('/mis-propiedades?pagina=1')
    }

    try {
        const {id} = req.usuario

        // Limites y OFFsets para paginador

        const limit = 10;
        const offset = ((paginaActual * limit) - limit) // Saltearse registros
        // (2 * 10) - 10 = Me salteo los primeros 10 y te muestro del 11 al 20

        const [propiedades, total] = await Promise.all([
            Propiedad.findAll({
                limit,
                offset,
                where: {
                    usuarioId: id
                },
                include: [
                    {model: Categoria, as: 'categoria'},
                    {model: Precio, as: 'precio'},
                    {model: Mensaje, as: 'mensaje'} 
                ]
            }),
            Propiedad.count({ // cuantas propiedades hay de este usuario
                where: {
                    usuarioId : id
                }
            })
        ])

        res.render('propiedades/admin',{
            pagina: 'Mis propiedades',
            propiedades,
            csrfToken: req.csrfToken(),
            paginas: Math.ceil(total / limit),
            paginaActual: Number(paginaActual),
            total,
            offset,
            limit
        })
    
  
    } catch (error) {
        console.log(error)
    }
}

      // formulario para crear una nueva propiedad
const crear = async (req, res) => {
    
        // Consultar modelo de precios y categorias
        const [categorias, precios] = await Promise.all([
            Categoria.findAll(),
            Precio.findAll()
        ])
    
        return res.render('propiedades/crear', {
            pagina: 'Crear Propiedad',
            csrfToken: req.csrfToken(),
            categorias,
            precios,
            datos: {}
            
        }) 

}


    


const guardar = async(req, res) => {
    // Validacion

    let resultado = validationResult(req)

    if(!resultado.isEmpty()){

    // Consultar modelo de precios y categorias
     const [categorias, precios] = await Promise.all([
        Categoria.findAll(),
        Precio.findAll()
    ])

    return res.render('propiedades/crear', {
            pagina: 'Crear Propiedad',
            csrfToken: req.csrfToken(),
            categorias,
            precios,
            errores: resultado.array(),
            datos: req.body
            
        }) 
    }

    // Crear un registro, es decir propiedad
    // con esos dos puntos estas renombrando la variable en la misma desestructuracion
    const {titulo, descripcion, habitaciones, estacionamiento, wc, calle, lat, lng, precio: precioId, categoria: categoriaId} = req.body
    const {id: usuarioId} = req.usuario


    try{

        const propiedadGuardada = await Propiedad.create({
            titulo, 
            descripcion,
            habitaciones, 
            estacionamiento, 
            wc,
            calle, 
            lat, 
            lng,
            precioId,
            categoriaId,
            usuarioId,
            imagen:''

        })

        const {id} = propiedadGuardada

        res.redirect(`/propiedades/agregar-imagen/${id}`)

    }catch(error){
        console.log(error)
    }

}

const agregarImagen = async(req, res) => {

    const {id} = req.params
    const propiedad = await Propiedad.findByPk(id)
    // Validar que la propiedad exista
    
    if(!propiedad){
        return res.redirect('/mis-propiedades')
    }

    // validar que la propiedad no este publicada
    if(!propiedad.publicado){
        return res.redirect('/mis-propiedades')
    }

    // validar que la propiedad le pertenece al asuario que visita la pagina
    if(req.usuario.id.toString() !== propiedad.usuarioId.toString()){
        return res.redirect('/mis-propiedades')
    }

    res.render('propiedades/agreagr-imagen', {
        pagina: `Agregar Imagen: ${propiedad.titulo}`,
        csrfToken: req.csrfToken(),
        propiedad
    })
}

const almacenarImagen = async (req, res, next) => {
    const {id} = req.params
    const propiedad = await Propiedad.findByPk(id)
    // Validar que la propiedad exista
    
    if(!propiedad){
        return res.redirect('/mis-propiedades')
    }

    // validar que la propiedad no este publicada
    if(!propiedad.publicado){
        return res.redirect('/mis-propiedades')
    }

    // validar que la propiedad le pertenece al asuario que visita la pagina
    if(req.usuario.id.toString() !== propiedad.usuarioId.toString()){
        return res.redirect('/mis-propiedades')
    }

    try {   
        // almacenar la imagen y publicar la propiedad 
        propiedad.imagen = req.file.filename
        propiedad.publicado = 1

        await propiedad.save()

        next()

    } catch (error) {
        console.log(error)
    }
}

const editar = async(req, res) => {

    const {id} = req.params

    // Validar que la propiedad exista

    const propiedad = await Propiedad.findByPk(id)

    if(!propiedad){
        return res.redirect('/mis-propiedades')
    }

    // Revisar que quien visita la url es quien creo la propiedad

    if(propiedad.usuarioId.toString() !== req.usuario.id.toString()){
        res.redirect('mis-propiedades')
    }

    // Consultar modelo de precios y categorias
    const [categorias, precios] = await Promise.all([
        Categoria.findAll(),
        Precio.findAll()
    ])

    return res.render('propiedades/editar', {
        pagina: `Editar Propiedad: ${propiedad.titulo}`,
        csrfToken: req.csrfToken(),
        categorias,
        precios,
        datos: propiedad
        
    }) 
}

const guardarCambios = async (req, res) => {

    // Verificar toda la validacion

     let resultado = validationResult(req)

     if(!resultado.isEmpty()){
 
     // Consultar modelo de precios y categorias
      const [categorias, precios] = await Promise.all([
         Categoria.findAll(),
         Precio.findAll()
     ])
 
     return res.render('propiedades/editar', {
        pagina: 'Editar Propiedad',
        csrfToken: req.csrfToken(),
        categorias,
        precios,
        errores: resultado.array(),
        datos: req.body
        
    }) 
     } 

    const {id} = req.params

    // Validar que la propiedad exista

    const propiedad = await Propiedad.findByPk(id)

    if(!propiedad){
        return res.redirect('/mis-propiedades')
    }

    // Revisar que quien visita la url es quien creo la propiedad

    if(propiedad.usuarioId.toString() !== req.usuario.id.toString()){
        res.redirect('mis-propiedades')
    }

    // Reescribir el objeto y actualizarlo
    try {
        const {titulo, descripcion, habitaciones, estacionamiento, wc, calle, lat, lng, precio: precioId, categoria: categoriaId} = req.body
        
        propiedad.set({
            titulo,
            descripcion,
            habitaciones,
            estacionamiento,
            wc,
            calle,
            lat,
            lng,
            precioId,
            categoriaId
        })

        await propiedades.save()

        res.redirect('/mis-propiedades')

    } catch (error) {
        console.log(error)
    }
}

const eliminar = async(req, res) => {
    const {id} = req.params

    // Validar que la propiedad exista

    const propiedad = await Propiedad.findByPk(id)

    if(!propiedad){
        return res.redirect('/mis-propiedades')
    }

    // Revisar que quien visita la url es quien creo la propiedad

    if(propiedad.usuarioId.toString() !== req.usuario.id.toString()){
        res.redirect('mis-propiedades')
    }

    // Eliminar la imagen
    await unlink(`public/uploads/${propiedad.imagen}`)

    // Eliminar la propiedad

    await propiedad.destroy()
    res.redirect('/mis-propiedades')
 
}

// MUESTRA UNA PROPIEDAD    

const mostrarPropiedad = async(req, res) => {

    const {id} = req.params

    // Validar que la propiedad exista

    const propiedad = await Propiedad.findByPk(id, {
        include: [
            {model: Precio, as: 'precio'},
            {model: Categoria, as: 'categoria'}
        ]
    })

    

    if(!propiedad || !propiedad.publicado){
        res.redirect('404')
    }



    res.render('propiedades/mostrar', {
        propiedad,
        pagina: propiedad.titulo,
        csrfToken: req.csrfToken(),
        usuario: req.usuario,
        esVendedor: esVendedor(req.usuarioId?.id, propiedad.usuarioId)
    })
}

const enviarMensaje = async(req, res) => {
    const {id} = req.params

    // Validar que la propiedad exista

    const propiedad = await Propiedad.findByPk(id, {
        include: [
            {model: Precio, as: 'precio'},
            {model: Categoria, as: 'categoria'}
        ]
    })

    if(!propiedad){
        res.redirect('404')
    }

    // Renderizar por si tenemos errores

     // Validacion

     let resultado = validationResult(req)

     if(!resultado.isEmpty()){
        return res.render('propiedades/mostrar', {
                propiedad,
                pagina: propiedad.titulo,
                csrfToken: req.csrfToken(),
                usuario: req.usuario,
                esVendedor: esVendedor(req.usuarioId?.id, propiedad.usuarioId),
                errores: resultado.array()
        })
     
     }

     // Almacenar el Mensaje

     const {mensaje} = req.body
     const {id: propiedadId} = req.params
     const {id: usuarioId} = req.usuario

     await Mensaje.create({
        mensaje,
        propiedadId,
        usuarioId
     })


     res.redirect('/')

    
}

// Leer mensajes recibidos

const verMensajes = async(req, res) => {

    const {id} = req.params

    // Validar que la propiedad exista
    // Propiedad lo cruza con mensaje, y mensaje con usuario
    const propiedad = await Propiedad.findByPk(id, {
                include: [
                    {model: Mensaje, as: 'mensaje',
                        include: [
                            {model: Usuario.scope('eliminarPassword'), as: 'usuario'}
                        ]
                } 
                ]
    })

    if(!propiedad){
        return res.redirect('/mis-propiedades')
    }

    // Revisar que quien visita la url es quien creo la propiedad

    if(propiedad.usuarioId.toString() !== req.usuario.id.toString()){
        res.redirect('mis-propiedades')
    }

    res.render('propiedades/mensajes', {
        pagina: 'Mensajes',
        mensajes: propiedad.mensajes,
        formatearFecha
    })
}

// Modifica el estado de la propiedad
const cambiaEstado = async(req, resp) => {
    const {id} = req.params

    // Validar que la propiedad exista

    const propiedad = await Propiedad.findByPk(id)

    if(!propiedad){
        return res.redirect('/mis-propiedades')
    }

    // Revisar que quien visita la url es quien creo la propiedad

    if(propiedad.usuarioId.toString() !== req.usuario.id.toString()){
        res.redirect('mis-propiedades')
    }

    // Actualizar
    propiedad.publicado = !propiedad.publicado
    await propiedad.save()

    res.json({
        resultado: 'ok'
    })

}

export {
    admin,
    crear,
    guardar,
    agregarImagen,
    almacenarImagen,
    editar,
    guardarCambios,
    eliminar,
    mostrarPropiedad,
    enviarMensaje,
    verMensajes,
    cambiaEstado
}

