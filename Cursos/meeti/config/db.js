const sequelize = require('sequelize')

module.exports = new sequelize('meet_db', '', '', {
    host: '127.0.0.1',
    port: '5432',
    dialect: 'postgres', 
    pool: {
        max: 5,
        min: 0,
        aquire: 30000,
        idle: 10000
    },
    logging: false
})