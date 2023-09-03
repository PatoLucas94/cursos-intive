const Clientes = require('../models/Clientes')

// Agrega un nuevo cliente 
exports.nuevoCliente = async(req, res, next) => {

    const cliente = new Clientes(req.body)

    try {
        // almacenar el registro
        await cliente.save()
        res.json({
            mensaje: 'Se agrego un nuevo cliente'
        })
    } catch (error) {
        console.log(error)
        next()
    }
}

// Mostrar todos los clientes
exports.mostrarClientes = async(req, res, next) => {

    try {
        const clientes = await Clientes.find({})
        res.json(clientes)
    } catch (error) {
        console.log()
        next()
    }
}

// muestra un cliente por su id
exports.mostrarCliente = async(req, res, next) => {
    const cliente = await Clientes.findById(req.params.idCliente)

    if(!cliente){
        res.json({
            mensaje: 'Ese cliente no existe'
        })
        return next()
    }

    // Mostrar el cliente
    res.json(cliente)
}

// Actualiza un cliente por su Id
exports.actualizarCliente = async(req, res, next) => {
    try {
        const cliente = await Clientes.findOneAndUpdate({_id: req.params.idCliente}, req.body, {
            new: true // Traeme el nuevo dato
        })

        res.json(cliente)
    } catch (error) {
        console.log(error)
        next()
    }
}

// Elimina un cliente por su Id
exports.eliminarCliente = async(req, res, next) => {
    try {
        await Clientes.findOneAndDelete({_id: req.params.idCliente})
        res.json({
            mensaje: 'El cliente ya se ha eliminado'
        })
    } catch (error) {
        console.log(error)
        next()
    }
}