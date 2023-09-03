const mongoose = require('mongoose')
require('./config/db')

const express = require('express')
const router = require('./routes')
const path = require('path')
const exphbs = require("express-handlebars")
const cookieParser = require('cookie-parser')
const session = require('express-session')
const MongoStore = require('connect-mongo')
const bodyParser = require('body-parser')
const flash = require('connect-flash')
const createError = require('http-errors')
const passport = require('./config/passport')


// Para agarrar el archivo de variables de entorno
require('dotenv').config({path: 'variables.env'})


const app = express()

// Habilitar BodyParser -- Para parsear lo que venga en el body del request
app.use(bodyParser.json())
app.use(bodyParser.urlencoded({extended: true}))

// Habilitar Handlebars como el template engine
app.engine('handlebars', 
    exphbs.engine({
        defaultLayout: 'layout',
        helpers: require('./helpers/handlebars') // Para que los handlebars se comuniquen con los helpers. Es como si le estuviera pasando todas las variables o funciones que tiene el archivo
    })    
);

app.set('view engine', 'handlebars')

// Static Files
app.use(express.static(path.join(__dirname, 'public')))

// Guardar una sesion utilizando mongoose
app.use(cookieParser())
app.use(session({
    secret: process.env.SECRETO,
    key: process.env.KEY,
    resave: false,
    saveUninitialized: false,
    store: MongoStore.create({mongoUrl: process.env.DATABASE})
}))

// Inicializar Passport
app.use(passport.initialize())
app.use(passport.session())

// Alertas y Flash messages
app.use(flash())

// Crear nuestro middleware
app.use((req, res, next) => {
    res.locals.mensajes = req.flash()
    next()
})

app.use('/', router())

// 404 pagina no existente
app.use((req, res, next) => {
    next(createError(404, 'No encontrado'))
})

// Administracion de los errores
// el primer parametro ante el manejo de error es el error
app.use((error, req, res) => {
    res.locals.mensaje = error.message
    const status = error.status || 500
    res.locals.status = status
    res.status(status)
    res.render('error')
})


// Dejar que HEROKU asigne el puerto
const host = '0.0.0.0'
const port = process.env.PORT
app.listen(port, host, () => {
    console.log('El servidor esta funcionando')
}) 
//app.listen(process.env.PUERTO)