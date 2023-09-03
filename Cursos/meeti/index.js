const express = require('express')
const router = require('./routes/index')
const path = require('path')
const expressLayouts = require('express-ejs-layouts')
const bodyParser = require('body-parser')
const flash = require('connect-flash')
const session = require('express-session')
const cookieParser = require('cookie-parser')
const expressValidator = require('express-validator')
const passport = require('./config/passport')
const { pass } = require('./config/emails')


// Configuracion y modelos de la bd
const db = require('./config/db')
    require('./models/Usuarios')
    require('./models/Categorias')
    require('./models/Grupos')
    db.sync().then(() => console.log('DB Conectada')).catch((error) => {console.log(error)})

// variables de desarrollo    
require('dotenv').config({path: 'variables.env'})


// Aplicacion Principal
const app = express()

// BodyParser de formularios
app.use(bodyParser.json())
app.use(bodyParser.urlencoded({
    extended: true
}))

// Express Validator(validacion con bastantes funciones)
app.use(expressValidator())


// Habilitar EJS como template engine
app.use(expressLayouts)
app.set('view engine', 'ejs')

// Ubicacion de las Vistas
app.set('views', path.join(__dirname, './views'))

// Archivos Estaticos
app.use(express.static('public'))

// habilitar cookieParser
app.use(cookieParser())

// Crear la sesion
app.use(session({
    secret: process.env.SECRETO,
    key: process.env.KEY,
    resave: false,
    saveUninitialized: false
}))

// Inicializar passport
app.use(passport.initialize())
app.use(passport.session())

// Agrega flash Messages
app.use(flash())

// Middlewares propios (usuarioLogueado, flash messages, fecha Actual)
app.use((req, res, next) => {
    req.locals.mensajes = req.flash()
    const fecha = new Date()
    res.locals.year = fecha.getFullYear()
    next()
})

// Routing 
app.use('/', router())



// Agrega el puerto
app.listen(process.env.PORT, () => {
    console.log('El servidor esta funcionando')
})
