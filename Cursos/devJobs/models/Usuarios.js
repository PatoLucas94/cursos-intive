const mongoose = require('mongoose')
mongoose.Promise = global.Promise // Para que todos los metodos sean promesas.
const bcrypt = require('bcrypt')

const usuariosSchema = new mongoose.Schema({
    email: {
        type: String,
        unique: true,
        lowercase: true,
        trim: true,
    },
    nombre: {
        type: String,
        required: true
    },
    password: {
        type: String,
        required: true,
        trim: true
    },
    token: String,
    expira: Date,
    imagen: String
})

// Metodo para hashear los passowrds. Antes de que se guarde
usuariosSchema.pre('save', async function(next){

    // si el password ya esta hasheado no hacemos nada
    if(!this.isModified('password')){
        return next() 
    }

    // Si no esta hasheado
    const hash = await bcrypt.hash(this.password, 12)
    this.password = hash
    next()

})

// Prevenir que se inserte un registro ante un error en especifico que es de datos duplicados
// Envia una alerta
usuariosSchema.post('save', function(error, document, next){
    if(error.name === 'MongoError' && error.code === 11000){
        next('Ese correo ya está registrado') // Mensaje de error personalizado
    }else {
        next(error)
    }
})

// Autenticar Usuarios
// Agregar mas metodos a ese schema
usuariosSchema.methods = {
    compararPassword: function(password){
        return bcrypt.compareSync(password, this.password)
    }
}

module.exports = mongoose.model('Usuarios', usuariosSchema)