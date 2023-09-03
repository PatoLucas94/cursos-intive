import multer from 'multer'
import path from 'path'
import {generarId} from '../helpers/tokens.js'

const storage = multer.diskStorage({
    destination: function(req, file, cb){ // donde se van a guardar los archivos
        cb(null, './public/uploads/')
    },
    filename: function(req, file, cb){
        cb(null, generarId() + path.extname(file.originalname)) // Se subio correctamente la imagen. El nombre del archivo y donde va a estar
    }
})


const upload = multer({
    storage
})


export default upload