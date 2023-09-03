import {DataTypes} from "sequelize"
import bcrypt from 'bcrypt'
import db from "../config/db.js"

// Creando una tabla de nombre "usuarios" con sus columnas
const Usuario = db.define('usuarios', {
    nombre: {
        type: DataTypes.STRING,
        allowNull: false
    },
    email: {
        type: DataTypes.STRING,
        allowNull: false
    },
    password: {
        type: DataTypes.STRING,
        allowNull: false
    },
    token: DataTypes.STRING,
    confirmado: DataTypes.BOOLEAN

}, {
    hooks: {
        beforeCreate: async function(usuario){
            const salt = await bcrypt.genSalt(10)
            usuario.password = await bcrypt.hash(usuario.password, salt);
        }
    },
    scopes: {
        eliminarPassword: {
            attributes: {
                exclude: ['password', 'token', 'confirmado', 'createdAt', 'updatedAt']
            }
        }
    }
})

// METODOS PERSONALIZADOS
// No se usa arrow function aca por un tema de scope. Yo necesito utilizar aca la palabra "this" para hacer
// referencia a la propiedad de usuario

Usuario.prototype.verificarPassword =  function(password1){
    return bcrypt.compareSync(password1, this.password)
}

export default Usuario