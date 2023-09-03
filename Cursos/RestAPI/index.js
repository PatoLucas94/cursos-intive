const express = require('express')
const routes = require('./routes')
const mongoose = require('mongoose')
const bodyParser = require('body-parser')

// conectar mongo
mongoose.Promise = global.Promise
mongoose.connect('mongodb://0.0.0.0/restapis', {
    useNewUrlParser: true
})

// Crear el servidor 
const app = express()

// Habilitar body Parser
app.use(bodyParser.json())
app.use(bodyParser.urlencoded({extended: true}))


// Rutas de la app
app.use('/', routes())


// Puerto
app.listen(5000)