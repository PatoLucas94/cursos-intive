const express = require("express");
const router = express.Router();

const ArticuloControlador = require("../controladores/Articulo");


// Rutas de prueba

router.get("/ruta-de-prueba", ArticuloControlador.prueba);
router.get("/curso", ArticuloControlador.curso);

// RUTA Util
router.post("/crear", ArticuloControlador.crear);
router.get("/articulos", ArticuloControlador.listar);

module.exports = router;


