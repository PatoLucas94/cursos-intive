const {conexion} = require("./basedatos/conexion")
const express = require("express");
const cors = require("cors");

// Inicializar app
console.log('App de Node arrancada');

// Conectar a la base de datos
conexion();


// Crear servidor de Node para escuchar peticiones HTTP
const app = express();
const puerto = 3900;


// Configurar el CORS
// EL USE SIRVE PARA CARGAR CUALQUIER COSA
app.use(cors()); // Ejecutamos el cors antes de ejecutar una ruta

// Convertir a objeto JS para ya tenerlo de una
app.use(express.json()); // recibir datos con contentType app/json
app.use(express.urlencoded({extended: true})); // recibiendo por url enconded

//  RUTAS
const rutasArticulo = require("./rutas/Articulos");

app.use("/api", rutasArticulo);



// Crear Rutas hardcodeadas
app.get("/probando", (req, res) => {
    console.log("Se ha ejecutado el endpoint probando")

    return res.status(200).json([{
            curso: "Master en React",
            autor: "Patricio Gianni",
            url: "patricioGianniWeb.es/maste-react"
        },
        {
            curso: "Master en NodeJS",
            autor: "Patricio Gianni",
            url: "patricioGianniWeb.es/maste-nodejs"
        }
    ])
});

app.get("/", (req, res) => {
   // El metodo send es para enviar cualquier cosa, objetos, html etc etc
    return res.status(200).send(` 
        <h1>Empezando a crear una api rest con node</h1>
        `)
});

// Crear el servidor y escuchar peticiones
app.listen(puerto, () => {
    console.log("Servidor corriendo en el puerto " + puerto);
});
