const {Schema, model} = require("mongoose");

const ArticuloSchema = Schema({
    titulo: {
        type: String,
        required: true
    },
    contenido: {
        type: String,
        required: true
    },
    fecha: {
        type: Date,
        default: Date.now
    },
    imagen: {
        type: String,
        default: "default.png"
    }
});

// Primer parametro: Articulo se va a llamar el modelo de ArticuloSchema 
// Segundo parametro: la clase o schema en que se basa el modelo
// Tercer parametro opcional: La coleccion que hace referencia en mongodb
module.exports = model("Articulo", ArticuloSchema, "articulos"); 