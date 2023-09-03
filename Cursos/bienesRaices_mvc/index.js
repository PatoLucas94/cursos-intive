// url http://localhost:3000/auth/registro
// const express = require('express') // Common JS
import express from 'express'
import csrf from 'csurf'
import cookieParser from 'cookie-parser'
import usuarioRoutes from './routes/usuarioRoutes.js'
import propiedadesRoutes from './routes/propiedadesRoutes.js'
import appRoutes from './routes/appRoutes.js'
import apiRoutes from './routes/apiRoutes.js'
import db from './config/db.js'

// Crear la app
const app = express() // Variable que contiene toda la info del servidor express

// HABILITAR LECTURA DE FORMULARIOS EXCEPTO ARCHIVOS MULTIMEDIA
app.use(express.urlencoded({extended: true}))

// HABILITAR COOKIE-PARSER
app.use(cookieParser())

// HABILITAR CSURF
app.use(csrf({cookie: true}))


// CONEXION A LA BASE DE DATOS
try{
    await db.authenticate();
    db.sync()
    console.log('Conexion correcta a la base de datos')
}catch(error){
    console.log(error)
}

// HABILITAR PUG
app.set('view engine', 'pug')
app.set('views', "./views") // en esta carpeta vas a encontrar las vistas

// Carpeta publica
app.use(express.static('public')) // Aca va a reconocer los archivos estaticos


// Routing
//app.get("/", usuarioRoutes) Con get buscas la ruta especifica, en este caso "/"
app.use("/", appRoutes)
app.use("/auth", usuarioRoutes) // Con use buscame todas las rutas que inicien con "/"
app.use("/", propiedadesRoutes)
app.use("/api", apiRoutes)

// Definir un puerto y arrancar el proyecto
const port = process.env.PORT || 3000;
app.listen(port, () => {
    console.log(`El servidor esta funcionando en el puerto: ${port}`)
})