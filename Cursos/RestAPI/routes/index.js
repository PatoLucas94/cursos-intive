const express = require('express')
const router = express.Router()
const clienteController = require('../controllers/clienteController')
const productosController = require('../controllers/productosController')
const pedidosController = require('../controllers/pedidosController')

module.exports = function(){

    // Agrega nuevos clientes via POST
    router.post('/clientes', clienteController.nuevoCliente)

    // Obtener todos los clientes
    router.get('/clientes', clienteController.mostrarClientes)

    // Muestra un cliente en especifico
    router.get('/clientes/:idCliente', clienteController.mostrarCliente)

    // Actualizar un cliente por su Id
    router.put('/clientes/:idCliente', clienteController.actualizarCliente)

    // Eliminar cliente
    router.delete('/clientes/:idCliente', clienteController.eliminarCliente)

    // PRODUCTOSSS /////////////////////

    router.post('/productos', 
                productosController.subirArchivo,
                productosController.nuevoProducto)

    // Muestra todos los productos
    router.get('/productos', productosController.mostrarProductos)

    // Muestra un producto en especifico
    router.get('/productos/:idProducto', productosController.mostrarProducto)

    // Actualizar Productos
    router.put('/productos/:idProducto', productosController.subirArchivo,
                    productosController.actualizarProducto)

    // eliminar productos
    router.delete('/productos/:idProducto', productosController.eliminarProducto)

    // PEDIDOSSSS /////////////////////
    
    // crear un nuevo pedido
    router.post('/pedidos', pedidosController.nuevoPedido)

    // mostrar todos los pedidos
    router.get('/pedidos', pedidosController.mostrarPedidos)

    // mostrar un pedido por su id
    router.get('/pedidos/:idPedido', pedidosController.mostrarPedido)

    // Actualizar Pedidos
    router.put('/pedidos/:idPedido', pedidosController.actualizarPedido)

    // Eliminar pedido
    router.delete('/pedido/:idPedido', pedidosController.eliminarPedido)

    return router
}