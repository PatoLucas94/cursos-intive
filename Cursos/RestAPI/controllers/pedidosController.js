const Pedidos = require('../models/Pedidos')

exports.nuevoPedido = async(req, res, next) => {

    const pedido = new Pedidos(req.body)
    try {
        await pedido.save()
        res.json({
            mensaje: 'Se agrego un nuevo pedido'
        })
    } catch (error) {
        console.log(error)
        next()
    }
}

exports.mostrarPedidos = async(req, res, next) => {
    // Los populate es para que se llene el objeto. En el caso del cliente es ahi nomas. 
    // pero para los productos tiene que bajar un nivel, entonces se hace de esa forma
    try {
        const pedidos = await Pedidos.find({}).populate('cliente').populate({
            path: 'pedido.producto',
            model: 'Productos'
        })
        res.json(pedidos)
    } catch (error) {
        console.log(error)
        next()
    }
}

exports.mostrarPedido = async(req, res, next) => {

    const pedido = await Pedidos.findById(req.params.idPedido).populate('cliente').populate({
        path: 'pedido.producto',
        model: 'Productos'
    })

    if(!pedido){
        res.json({
            mensaje: 'Ese pedido no existe'
        })
        return next()
    }

    // mostrar el pedido
    res.json(pedido)


}

exports.actualizarPedido = async(req, res, next) => {

    try {
        let pedido = await Pedidos.findOneAndUpdate({_id: req.params.idPedido}, req.body, {
            new: true
        }).populate('cliente')
          .populate({
                path: 'pedido.producto',
                model: 'Productos'
            })

        res.json(pedido)
    } catch (error) {
        console.log(error)
        next()
    }
}

exports.eliminarPedido = async(req, res, next) => {
    
    try {
        await Pedidos.findOneAndDelete({_id: req.params.idPedido})

        res.json({
            mensaje: 'Se ha eliminado correctamente el pedido'
        })
    } catch (error) {
        console.log(error)
        next()
    }
    
    
}