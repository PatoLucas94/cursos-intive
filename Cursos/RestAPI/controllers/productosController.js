const Productos = require('../models/Producto')

const multer = require('multer');
const shortid = require('shortid');
const fs = require('fs');
const Producto = require('../models/Producto');

const configuracionMulter = {
    storage: fileStorage = multer.diskStorage({
        destination: (req, file, cb) => {
            cb(null, __dirname + '../../uploads/');
        },
        filename: (req, file, cb) => {
            const extension = file.mimetype.split('/')[1];
            cb(null, `${shortid.generate()}.${extension}`);
        }
    }),
    fileFilter(req, file, cb) {
        if (file.mimetype === 'image/jpeg' || file.mimetype === 'image/png') {
            cb(null, true);
        } else {
            cb(new Error('Formato no válido'))
        }
    }
}

// Pasar la configuracion y el campo
const upload = multer(configuracionMulter).single('imagen');

// Sube un archivo
exports.subirArchivo = (req, res, next) => {
    upload(req, res, function (error) {
        if (error) {
            res.json({ mensaje: error })
        }
        return next();
    })
}

// agrega nuevos productos
exports.nuevoProducto = async(req, res, next) => {
    const producto = new Productos(req.body)

    try {

        if(req.file){
            producto.imagen = req.file.filename
        }

        producto.save()
        res.json({
            mensaje: 'Se agrego un nuevo producto'
        })
    } catch (error) {
        console.log(error)
        next()
    }
}

// muestra todos los productos
exports.mostrarProductos = async(req, res, next) => {
    try {
        // Obtener todos los productos

        const productos = await Productos.find({})
        res.json(productos)
    } catch (error) {
        console.log(error)
        next()
    }
}

// Muestra un producto por su id
exports.mostrarProducto = async(req, res, next) => {
    const producto = await Producto.findById(req.params.idProducto)

    if(!producto){
        res.json({
            mensaje: 'Ese producto no existe'
        })
        return next()
    }

    // Mostrar el producto
    res.json(producto)
}

// Actualiza producto por su id
exports.actualizarProducto = async(req, res, next) => {

    try {

        // construir nuevo producto
        let nuevoProducto = req.body

        // Verificar si hay imagen nueva
        if(req.file){

            nuevoProducto.imagen = req.file.name
        }else{

            // Por si viene una imagen nueva
            let productoPorId = await Productos.findById(req.params.idProducto)

            nuevoProducto.imagen = productoPorId.imagen // el producto de la base de datos
        }

        let producto = await Productos.findOneAndUpdate({_id: req.params.idProducto}, req.body, {
            new: true
        })

        res.json(producto)
    } catch (error) {
        console.log(error)
        next()
    }
}

// elimina un producto por su id
exports.eliminarProducto =  async(req, res, next) => {

    try {
        await Productos.findOneAndDelete({_id: req.params.idProducto})
        res.json({
            mensaje: 'El Producto se ha eliminado'
        })
    } catch (error) {
        console.log(error)
        next()
    }
}