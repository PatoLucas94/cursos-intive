import express from 'express'
import {body} from 'express-validator'
import {admin, crear, guardar, agregarImagen, almacenarImagen, editar, guardarCambios, eliminar, 
    mostrarPropiedad, enviarMensaje, verMensajes, cambiaEstado} from '../controllers/propiedadController.js'
import protegerRuta from '../middleware/protegerRuta.js'
import upload from '../middleware/subirImagen.js'
import identificarUsuario from '../middleware/identificarUsuario.js'
const router = express.Router()

router.get('/mis-propiedades', /*protegerRuta, */ admin)
router.get('/propiedades/crear', /*protegerRuta, */ crear)
router.post('/propiedades/crear', /*protegerRuta, */

// En el body es lo que lleno el usuario

    body('titulo').notEmpty().withMessage('El Titulo del anuncio es obligatorio'),
    body('descripcion').notEmpty().withMessage('La Descripcion no puede ir vacia')
        .isLength({max: 200}).withMessage('La Descripcion es muy larga'),
    body('categoria').isNumeric().withMessage('Selecciona una categoria'),
    body('habitaciones').isNumeric().withMessage('Selecciona la cantidad de habitaciones'),
    body('estacionamiento').isNumeric().withMessage('Selecciona la cantidad de estacionamientos'),
    body('wc').isNumeric().withMessage('Selecciona la cantidad de baños'),
    body('precio').isNumeric().withMessage('Selecciona un rango de precios'),
    body('lat').notEmpty().withMessage('Ubica la propiedad en el mapa'),
    guardar

)


router.get('/propiedades/agregar-imagen/:id',  /*protegerRuta, */agregarImagen)

router.post('/propiedades/agregar-imagen/:id',  /*protegerRuta, */upload.single('imagen'), almacenarImagen)

router.get('/propiedades/editar/:id', /*protegerRuta, */
        editar
    )

router.post('/propiedades/agregar-imagen/:id', /*protegerRuta, */

// En el body es lo que lleno el usuario

    body('titulo').notEmpty().withMessage('El Titulo del anuncio es obligatorio'),
    body('descripcion').notEmpty().withMessage('La Descripcion no puede ir vacia')
        .isLength({max: 200}).withMessage('La Descripcion es muy larga'),
    body('categoria').isNumeric().withMessage('Selecciona una categoria'),
    body('habitaciones').isNumeric().withMessage('Selecciona la cantidad de habitaciones'),
    body('estacionamiento').isNumeric().withMessage('Selecciona la cantidad de estacionamientos'),
    body('wc').isNumeric().withMessage('Selecciona la cantidad de baños'),
    body('precio').isNumeric().withMessage('Selecciona un rango de precios'),
    body('lat').notEmpty().withMessage('Ubica la propiedad en el mapa'),
    guardarCambios

)

router.post('/propiedades/eliminar/:id', /*protegerRuta, */ eliminar)

// AREA PUBLICA 

router.get('/propiedad/:id', 
    identificarUsuario,
    mostrarPropiedad
)

// Almacenar Los Mensajes

router.post('/propiedad/:id', 
    identificarUsuario,
    body('mensaje').isLength({min: 10}).withMessage('El mensaje no puede ir vacio o es muy corto'),
    enviarMensaje
)

router.get('/mensajes/:id', /*protegerRuta, */ verMensajes)

router.put('/propiedades/:id',  /*protegerRuta, */  cambiaEstado)

export default router 