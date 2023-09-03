import { Sequelize } from "sequelize";
import dotenv from 'dotenv';
dotenv.config({path: '.env'})

// Nombre de la base de datos, usuario, contraseña, objeto con parametros
// Entran como variable de entorno en .env
const db = new Sequelize(process.env.BD_NOMBRE, process.env.BD_USER, 'abrilPATO2023#', {
    host: process.env.BD_HOST,
    port: 3306,
    dialect: 'mysql',
    define: {
        timestamps: true
    },
    pool: {
        max: 5,
        min: 0,
        acquire: 30000,
        idle: 10000
    },
    operatorsAliases: false
});

export default db;