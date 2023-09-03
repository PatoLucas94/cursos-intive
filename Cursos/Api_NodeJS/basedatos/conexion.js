const mongoose = require("mongoose"); // Agarrar la dependencia de mongoose d ela carpeta module

const conexion = async() => {

    try{

       await mongoose.connect("mongodb://127.0.0.1:27017/mi_blog"); 

       console.log("Conectado correctamente a la base de datos mi_blog");

    }catch(error){
        console.log(error);
        throw new Error("No se ha podido conectar a la base de datos");
    }


}


module.exports = {
    conexion
}